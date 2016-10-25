using UnityEngine;
using System.Collections;

public class Cs_SkiingPlayerController : MonoBehaviour
{
    float f_Velocity;
    Vector3 v3_CurrentVector;
    bool b_GravityApplied;

    GameObject go_RaycastPoint_1;
    GameObject go_RaycastPoint_2;
    GameObject go_RaycastPoint_3;
    GameObject go_RaycastPoint_4;

    // Physics Materials
    PhysicMaterial physMat_Ski;
    PhysicMaterial physMat_Walk;

    // Jump bool. Resets on collision with ground
    bool b_CanJump;
    bool b_IsSkiing;

    [SerializeField] float f_MaxSpeed;

	// Use this for initialization
	void Start ()
    {
        go_RaycastPoint_1 = transform.Find("RaycastPoint_1").gameObject;
        go_RaycastPoint_2 = transform.Find("RaycastPoint_2").gameObject;
        go_RaycastPoint_3 = transform.Find("RaycastPoint_3").gameObject;
        go_RaycastPoint_4 = transform.Find("RaycastPoint_4").gameObject;

        physMat_Ski  = (PhysicMaterial)Resources.Load("PhysMat_Ski");
        physMat_Walk = (PhysicMaterial)Resources.Load("PhysMat_Walk");
    }

    Vector3 v3_Velocity;
    // Update is called once per frame
    void Update ()
    {
        // print("Current speed: " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        print(b_CanJump);

        #region PlayerSliding

        // If player is skiing
        if(Input.GetKey(KeyCode.Space))
        {
            if(b_CanJump)
            {
                Jump();

                b_CanJump = false;
            }
            else
            {
                Ski();
            }
        }
        // If player is not skiing
        else
        {
            // Set PhysicsMaterial
            gameObject.GetComponent<Collider>().material = physMat_Walk;

            if (v3_Velocity != new Vector3())
            {
                v3_Velocity = new Vector3();

                print("Reset Velocity");
            }
        }

        /*
        if(Input.GetKey(KeyCode.W))
        {
            if(gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 10)
            {
                f_Velocity += Time.deltaTime;

                v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity + (gameObject.transform.forward * f_Velocity);

                gameObject.GetComponent<Rigidbody>().velocity = v3_Velocity;

                print((gameObject.transform.forward * Time.deltaTime * 5));
            }
        }
        */

        // Vector3 v3_CurrPos = gameObject.transform.position;
        //gameObject.transform.position = hit.point + (gameObject.transform.up * 1);

        // Apply a forward vector and push the player in that direction
        // gameObject.GetComponent<Rigidbody>().velocity = v3_GroundVector * 10;
        #endregion
    }

    float f_JumpMagnitude_Curr;
    [SerializeField] float f_MaxJumpMagnitude;
    void Jump()
    {
        // While the player is holding down the space bar, keep 'jumping'.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Apply velocity
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.up, Color.red, 5.0f);
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * f_JumpMagnitude_Curr * gameObject.GetComponent<Rigidbody>().mass);

            // f_JumpMagnitude_Curr -= Time.deltaTime * 5;
        }
    }

    void Ski()
    {
        #region Ski (if in the air)
        // Raycast down and grab the angle of the terrain
        RaycastHit hit = CheckRaycasts();

        // This checks to be sure there is ground below us & it is within a certain distance
        if (hit.distance <= 1.5f && (hit.normal != new Vector3()))
        {
            if (v3_Velocity == new Vector3())
            {
                // Set PhysicsMaterial
                gameObject.GetComponent<Collider>().material = physMat_Ski;

                if (!(f_MaxSpeed <= 0))
                {
                    v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;
                    v3_Velocity.Normalize();
                    v3_Velocity *= f_MaxSpeed;

                    print("Set Velocity: " + v3_Velocity.magnitude);
                }
                else
                {
                    v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;

                    print("Set Velocity: " + v3_Velocity.magnitude);
                }
            }

            print("Speed: " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);

            // This works, using the direction they're moving.
            Vector3 v3_GroundVector = Vector3.ProjectOnPlane(gameObject.GetComponent<Rigidbody>().velocity, hit.normal);

            // Normalizes the vector
            v3_GroundVector.Normalize();

            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= v3_Velocity.magnitude)
            {
                gameObject.GetComponent<Rigidbody>().velocity = v3_GroundVector * v3_Velocity.magnitude;
            }
        }
        else
        {
            if (v3_Velocity != new Vector3())
            {
                v3_Velocity = new Vector3();

                print("Reset Velocity");
            }
        }
        #endregion
    }

    RaycastHit CheckRaycasts()
    {
        // outHit is what we'll be sending out from the function
        RaycastHit outHit;

        // tempHit is what we'll compare against
        RaycastHit tempHit;

        // Set the default as outHit automatically
        Physics.Raycast(go_RaycastPoint_1.transform.position, -transform.up, out outHit, 1.5f);

        // Begin comparing against the other three. Find the shortest distance
        if (Physics.Raycast(go_RaycastPoint_2.transform.position, -transform.up, out tempHit, 1.5f))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        if (Physics.Raycast(go_RaycastPoint_3.transform.position, -transform.up, out tempHit, 1.5f))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        if (Physics.Raycast(go_RaycastPoint_4.transform.position, -transform.up, out tempHit, 1.5f))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        // Return the shortest hit distance
        return outHit;
    }

    void OnCollisionEnter( Collision collision_ )
    {
        if(!b_CanJump)
        {
            int i_LayerMask = LayerMask.GetMask("Ground");

            float f_RaycastDistance = 0.1f;

            GameObject go_RaycastPoint_Jump = GameObject.Find("RaycastPoint_Jump");

            RaycastHit hit;

            Physics.Raycast(go_RaycastPoint_Jump.transform.position, -gameObject.transform.up, out hit, f_RaycastDistance, i_LayerMask);

            if(hit.distance <= f_RaycastDistance)
            {
                // Reset jump capabilities
                b_CanJump = true;

                f_JumpMagnitude_Curr = f_MaxJumpMagnitude;
            }
        }
    }
}
