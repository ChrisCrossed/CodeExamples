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
    float f_JumpTimer;

    // Update is called once per frame
    void Update ()
    {
        // print("Current speed: " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        #region PlayerSliding

        // On the first moment the spacebar is pressed, jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // Otherwise, if the button is held down, the player is skiing
        else if (Input.GetKey(KeyCode.Space))
        {
            // Increment a timer to be sure the whole jump can be fulfilled
            if(f_JumpTimer < 0.5f) f_JumpTimer += Time.deltaTime;

            if(f_JumpTimer > 0.5f) Ski();
        }
        // If player is not skiing
        else
        {
            // Set PhysicsMaterial
            gameObject.GetComponent<Collider>().material = physMat_Walk;
            
            /*
            if (v3_Velocity != new Vector3())
            {
                v3_Velocity = new Vector3();

                // print("Reset Velocity");
            }
            */

            PlayerInput();
        }
        #endregion
    }

    float f_Speed = 0.0f;
    float f_MaxRunSpeed = 10f;
    float f_Acceleration = 20f;
    void PlayerInput()
    {
        Vector3 v3_OldVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        float f_JumpVelocity = v3_OldVelocity.y;

        Vector3 v3_InputVelocity = new Vector3();

        if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            v3_InputVelocity.z = 1;
        }
        else if(!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            v3_InputVelocity.z = -1;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            v3_InputVelocity.x = -1;
        }
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            v3_InputVelocity.x = 1;
        }

        v3_InputVelocity.Normalize();
        
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D))
        {
            f_Speed += Time.deltaTime * f_Acceleration;

            if (f_Speed > f_MaxRunSpeed) f_Speed = f_MaxRunSpeed;

            // v3_NewVelocity *= f_Speed;
        }
        else
        {
            f_Speed -= Time.deltaTime * f_Acceleration;

            if (f_Speed < 0) f_Speed = 0;
        }

        // Aligning vector to that of the player's rotation
        // Vector3 v3_FinalVelocity = gameObject.transform.rotation * v3_InputVelocity * f_Speed;
        Vector3 v3_FinalVelocity = Vector3.Lerp(v3_OldVelocity, gameObject.transform.rotation * v3_InputVelocity * f_Speed, f_Acceleration);

        // Restore y velocity
        v3_FinalVelocity.y = f_JumpVelocity;

        RaycastHit hit;
        int i_LayerMask = LayerMask.GetMask("Ground");
        Physics.Raycast(gameObject.transform.position, -transform.up, out hit, float.PositiveInfinity, i_LayerMask);

        // Project upon a plane
        v3_NewVelocity = Vector3.ProjectOnPlane(v3_NewVelocity, -hit.normal);

        // Set final rotation
        gameObject.GetComponent<Rigidbody>().velocity = v3_FinalVelocity;
    }

    float f_JumpMagnitude_Curr;
    [SerializeField] float f_MaxJumpMagnitude;
    Vector3 v3_NewVelocity;
    void Jump()
    {
        if(b_CanJump)
        {
            Vector3 v3_CurrVelocity = gameObject.GetComponent<Rigidbody>().velocity;
            v3_CurrVelocity.y = 5;

            gameObject.GetComponent<Rigidbody>().velocity = v3_CurrVelocity;

            b_CanJump = false;

            f_JumpTimer = 0;
        }
    }

    void Ski()
    {
        #region Ski (if in the air)
        // Raycast down and grab the angle of the terrain
        RaycastHit hit = CheckRaycasts();

        print(hit.distance);

        // This checks to be sure there is ground below us & it is within a certain distance
        if (hit.distance < 1.0f && (hit.normal != new Vector3()))
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

                    // print("Set Velocity: " + v3_Velocity.magnitude);
                }
                else
                {
                    v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;

                    // print("Set Velocity: " + v3_Velocity.magnitude);
                }
            }

            // print("Speed: " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);

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

                // print("Reset Velocity");
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
