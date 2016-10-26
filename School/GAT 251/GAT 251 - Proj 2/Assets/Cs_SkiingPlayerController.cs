using UnityEngine;
using System.Collections;

public class Cs_SkiingPlayerController : MonoBehaviour
{
    GameObject go_RaycastPoint_1;
    GameObject go_RaycastPoint_2;
    GameObject go_RaycastPoint_3;
    GameObject go_RaycastPoint_4;

    // Physics Materials
    PhysicMaterial physMat_Ski;
    PhysicMaterial physMat_Walk;

    // Jump bool. Resets on collision with ground
    bool b_CanJump;

    [SerializeField] float f_MaxSpeed;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(transform.rotation.eulerAngles);

        f_xRot = transform.eulerAngles.y;
        f_xRot_Curr = f_xRot;

        go_RaycastPoint_1 = transform.Find("RaycastPoint_1").gameObject;
        go_RaycastPoint_2 = transform.Find("RaycastPoint_2").gameObject;
        go_RaycastPoint_3 = transform.Find("RaycastPoint_3").gameObject;
        go_RaycastPoint_4 = transform.Find("RaycastPoint_4").gameObject;

        physMat_Ski  = (PhysicMaterial)Resources.Load("PhysMat_Ski");
        physMat_Walk = (PhysicMaterial)Resources.Load("PhysMat_Walk");

        go_Camera = transform.Find("MainCamera").gameObject;

        Cursor.lockState = CursorLockMode.Locked;

        //gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    Vector3 v3_Velocity;
    float f_JumpTimer;

    // Update is called once per frame
    void Update ()
    {
        //print("Current speed: " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);

        // Update mouse look
        MouseInput();

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

            PlayerInput();
        }
        #endregion
    }

    float f_LookSensitivity = 5f;
    float f_lookSmoothDamp = 0.1f;
    float f_yRot;
    float f_yRot_Curr;
    float f_yRot_Vel;
    float f_xRot;
    float f_xRot_Curr;
    float f_xRot_Vel;
    GameObject go_Camera;
    void MouseInput()
    {
        #region Mouse Horizontal

        // Update Mouse State
        f_xRot += Input.GetAxis("Mouse X") * f_LookSensitivity;

        // Smooth it out
        f_xRot_Curr = Mathf.SmoothDamp(f_xRot_Curr, f_xRot, ref f_xRot_Vel, f_lookSmoothDamp);

        #endregion

        #region Mouse Vertical

        // Update Mouse State
        f_yRot += Input.GetAxis("Mouse Y") * f_LookSensitivity;

        // Clamp the angles (Vertical only)
        f_yRot = Mathf.Clamp(f_yRot, -90, 90);

        // Smooth it out
        f_yRot_Curr = Mathf.SmoothDamp(f_yRot_Curr, f_yRot, ref f_yRot_Vel, f_lookSmoothDamp);

        #endregion

        // Apply vertical rotation to Camera
        go_Camera.transform.rotation = Quaternion.Euler(-f_yRot_Curr, f_xRot_Curr, 0);

        // Apply horizontal rotation to gameObject
        gameObject.transform.rotation = Quaternion.Euler(0, f_xRot_Curr, 0);
    }

    float f_Speed = 0.0f;
    float f_MaxRunSpeed = 10f;
    float f_Acceleration = 25f;
    void PlayerInput()
    {
        Vector3 v3_OldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        // If the player is in the air, do not manipulate their movement velocity
        RaycastHit hit = CheckRaycasts();
        //print(hit.distance);

        if(hit.distance < 2f)
        {
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
            }
            else
            {
                f_Speed -= Time.deltaTime * f_Acceleration;

                if (f_Speed < 0) f_Speed = 0;
            }

            // Aligning vector to that of the player's rotation
            Vector3 v3_FinalVelocity = Vector3.Lerp(v3_OldVelocity, gameObject.transform.rotation * v3_InputVelocity * f_Speed, 0.1f);

            // Restore y velocity
            v3_FinalVelocity.y = f_JumpVelocity;

            // Project upon a plane
            v3_NewVelocity = Vector3.ProjectOnPlane(v3_NewVelocity, -hit.normal);

            // Set final rotation
            gameObject.GetComponent<Rigidbody>().velocity = v3_FinalVelocity;
        }
        else
        {
            // We're above the ground. Disable the ability to jump.
            b_CanJump = false;
        }
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

        // This checks to be sure there is ground below us & it is within a certain distance
        if (hit.distance < 1.1f && (hit.normal != new Vector3()))
        {
            if (v3_Velocity == new Vector3())
            {
                // Set PhysicsMaterial
                gameObject.GetComponent<Collider>().material = physMat_Ski;

                if (!(f_MaxSpeed <= 0))
                {
                    // v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;
                    v3_Velocity = new Vector3(v3_Velocity.x, 0, v3_Velocity.z);
                    v3_Velocity = Vector3.ProjectOnPlane(v3_Velocity, hit.normal);
                    v3_Velocity.Normalize();
                    v3_Velocity *= f_MaxSpeed;
                }
                else
                {
                    v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;
                }
            }

            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= v3_Velocity.magnitude)
            {
                gameObject.GetComponent<Rigidbody>().velocity = v3_Velocity;
            }
        }
        #endregion

        Vector3 v3_AirPush = new Vector3();
        // Apply basic velocities based on player input
        if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            // Apply left movement
            v3_AirPush.x = -1;
            // gameObject.GetComponent<Rigidbody>().AddForce(-gameObject.transform.right * 2f);

            print("Pushing: Left");
        }
        else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            // gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * 2f);
            v3_AirPush.x = 1;
            print("Pushing: Right");
        }

        if(Input.GetKey(KeyCode.S))
        {
            v3_AirPush.z = -1;
            print("Pushing: Back");
        }

        if(v3_AirPush != new Vector3())
        {
            v3_AirPush.Normalize();

            Vector3 v3_FinalAirPush = gameObject.transform.rotation * v3_AirPush;

            gameObject.GetComponent<Rigidbody>().AddForce(v3_FinalAirPush * 2f);
        }
    }

    RaycastHit CheckRaycasts()
    {
        // outHit is what we'll be sending out from the function
        RaycastHit outHit;

        // tempHit is what we'll compare against
        RaycastHit tempHit;

        int i_LayerMask = LayerMask.GetMask("Ground");

        // Set the default as outHit automatically
        Physics.Raycast(go_RaycastPoint_1.transform.position, -transform.up, out outHit, float.PositiveInfinity, i_LayerMask);

        // Begin comparing against the other three. Find the shortest distance
        if (Physics.Raycast(go_RaycastPoint_2.transform.position, -transform.up, out tempHit, float.PositiveInfinity, i_LayerMask))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        if (Physics.Raycast(go_RaycastPoint_3.transform.position, -transform.up, out tempHit, float.PositiveInfinity, i_LayerMask))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        if (Physics.Raycast(go_RaycastPoint_4.transform.position, -transform.up, out tempHit, float.PositiveInfinity, i_LayerMask))
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
