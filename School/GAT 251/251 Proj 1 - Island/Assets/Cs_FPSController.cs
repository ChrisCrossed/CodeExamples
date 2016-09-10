using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Cs_FPSController : MonoBehaviour
{
    // Player Speed Reference
    public float MAX_MOVESPEED_FORWARD;
    float f_MoveSpeedMultiplier;
    public float ACCELERATION;
    public float JUMP_HEIGHT;
    
    GamePadState state;
    GamePadState prevState;
    public PlayerIndex playerIndex = PlayerIndex.One;

    bool b_Keyboard;
    public bool b_MouseSmoothing = true;
    float f_LookSensitivity = 5f;
    float f_lookSmoothDamp = 0.1f;
    float f_yRot_Previous;
    float f_yRot;
    float f_yRot_Curr;
    float f_yRot_Vel;

    bool b_CanJump;
    GameObject go_RaycastObj;
    float f_JumpTimer;
    int i_GravityVelocityMultiplier;
    bool b_Sprinting;
    float f_RayCast_DownwardDistance;

    Camera[] playerCam;
    float f_FOV;
    public float f_NORMAL_FOV;
    public float F_SPRINTING_FOV;

    // Use this for initialization
    void Start ()
    {
        SetMouseSmoothing(b_MouseSmoothing, 0.1f);

        b_Keyboard = false;
        b_CanJump = true;
        go_RaycastObj = gameObject.transform.Find("JumpRaycast").gameObject;
        f_RayCast_DownwardDistance = 0.25f;

        i_GravityVelocityMultiplier = 1;
        f_MoveSpeedMultiplier = 1;

        playerCam = gameObject.GetComponentsInChildren<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        b_Keyboard = KeyboardCheck(b_Keyboard);

        if (b_Keyboard) Input_Keyboard(); else Input_Controller();

        // Check if the player's allowed to jump again
        UpdateJump();
    }

    bool KeyboardCheck( bool b_KeyboardPressed )
    {
        // Update Mouse State
        f_yRot_Previous = f_yRot;
        f_yRot += Input.GetAxis("Mouse Y") * f_LookSensitivity;

        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.Space) ||
            f_yRot_Previous != f_yRot)
        {
            return true;
        }

        return false;
    }

    void PlayerMovement(Vector3 v3_Direction_, bool b_Jump_, float f_Magnitude_ = 1)
    {
        // Old velocity
        Vector3 v3_oldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        // Combine (not multiply) the player's current rotation (Quat) into the input vector (Vec3)
        Vector3 v3_FinalRotation = gameObject.transform.rotation * v3_Direction_;

        // Lerp prior velocity into new velocity
        Vector3 v3_newVelocity = Vector3.Lerp(v3_oldVelocity, v3_FinalRotation * MAX_MOVESPEED_FORWARD * f_MoveSpeedMultiplier * f_Magnitude_ , 1 / ACCELERATION );

        // Return gravity
        if (!b_Jump_)
        {
            // Synthetic terminal velocity
            RaycastHit hit;

            // Raycast straight down 
            Physics.Raycast(go_RaycastObj.transform.position, -transform.up, out hit);

            // Apply fake gravity (synthetic Terminal Velocity) - Note: RigidBody gravity is OFF
            if( hit.distance > f_RayCast_DownwardDistance) v3_newVelocity.y = v3_oldVelocity.y - (Time.deltaTime * 20);
        }
        else
        {
            // Apply a jump
            v3_newVelocity.y = JUMP_HEIGHT;

            ResetJump();
        }

        // Determine direction to push against ramp
        Vector3 v3_PushDirection = RampDirection();

        // Apply velocity to player
        v3_newVelocity = Vector3.ProjectOnPlane(v3_newVelocity, v3_PushDirection);

        gameObject.GetComponent<Rigidbody>().velocity = v3_newVelocity;

        // gameObject.GetComponent<Rigidbody>().AddForce(v3_PushDirection);
    }

    Vector3 RampDirection()
    {
        // Stop player from sliding
        RaycastHit hit;

        // Raycast straight down 
        Physics.Raycast(gameObject.transform.position, -transform.up, out hit, f_RayCast_DownwardDistance);

        // Return the opposite direction against the ramp
        return -hit.normal;
    }

    void Input_Keyboard()
    {
        // Create a new Vector3
        Vector3 v3_PlayerInput = new Vector3();

        #region Input
        // Determine movement vector based on player input
        if( Input.GetKey( KeyCode.W ) && !Input.GetKey( KeyCode.S ))
        {
            v3_PlayerInput.z = 1;
        }
        else if( Input.GetKey( KeyCode.S ) && !Input.GetKey( KeyCode.W ))
        {
            v3_PlayerInput.z = -1;
        }

        if( Input.GetKey( KeyCode.A ) && !Input.GetKey( KeyCode.D ))
        {
            v3_PlayerInput.x = -1;
        }
        else if( Input.GetKey( KeyCode.D ) && !Input.GetKey( KeyCode.A ))
        {
            v3_PlayerInput.x = 1;
        }
        #endregion

        #region Jump
        bool b_Jump = false;
        if( b_CanJump )
        {
            if( Input.GetKey( KeyCode.Space ))
            {
                b_Jump = true;
                b_CanJump = false;
            }
        }
        #endregion

        #region Sprint
        if( Input.GetKey( KeyCode.LeftControl ))
        {
            if(b_CanJump)
            {
                f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 1.5f, 0.5f);

                b_Sprinting = true;
            }
        }
        else
        {
            f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 1, 0.5f);

            b_Sprinting = false;
        }
        
        TEMPORARY_CAMERA_SYSTEM();
        #endregion

        #region Rotation/Looking (Mouse)

        f_yRot = Mathf.Clamp( f_yRot, -90, 90 );

        f_yRot_Curr = Mathf.SmoothDamp( f_yRot_Curr, f_yRot, ref f_yRot_Vel, f_lookSmoothDamp );

        Quaternion quat_CurrRot = Quaternion.Euler(0, f_yRot_Curr, 0);

        #endregion

        // Normalize vector
        v3_PlayerInput.Normalize();

        // Pass into PlayerMovement
        PlayerMovement( v3_PlayerInput, b_Jump );
    }

    void Input_Controller()
    {
        // Update controller information
        prevState = state;
        state = GamePad.GetState( playerIndex );

        #region Input
        // Create a new Vector3
        Vector3 v3_PlayerInput = new Vector3();

        // Determine movement vector based on player
        v3_PlayerInput.x = state.ThumbSticks.Left.X;
        v3_PlayerInput.z = state.ThumbSticks.Left.Y;

        // Capture controller Analog magnitude
        float f_Magnitude = v3_PlayerInput.magnitude;
        #endregion

        #region Jump
        bool b_Jump = false;
        if( b_CanJump )
        {
            if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
            {
                b_Jump = true;
                b_CanJump = false;
            }
        }
        #endregion

        #region Sprint
        if(state.Buttons.LeftStick == ButtonState.Pressed)
        {
            b_Sprinting = true;
        }

        Vector2 MagnitudeTest = new Vector2();
        MagnitudeTest.x = state.ThumbSticks.Left.X;
        MagnitudeTest.y = state.ThumbSticks.Left.Y;

        if ( MagnitudeTest.magnitude < 0.5f )
        {
            b_Sprinting = false;
        }

        if( b_Sprinting )
        {
            if (b_CanJump)
            {
                f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 2, 0.5f);
            }
        }
        else
        {
            f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 1, 0.5f);
        }

        TEMPORARY_CAMERA_SYSTEM();

        #endregion

        // Normalize vector
        v3_PlayerInput.Normalize();

        // Pass into PlayerMovement
        PlayerMovement( v3_PlayerInput, b_Jump, f_Magnitude );
    }

    void UpdateJump()
    {
        // Raycast Out object
        RaycastHit hit;

        if( f_JumpTimer < 0.2f )
        {
            f_JumpTimer += Time.deltaTime;

            if (f_JumpTimer > 0.2f) f_JumpTimer = 0.2f;
        }
        else
        {
            if( Physics.Raycast( go_RaycastObj.transform.position, -transform.up, out hit, 0.3f ))
            {
                b_CanJump = true;
            }
        }
    }

    void ResetJump()
    {
        f_JumpTimer = 0.0f;

        b_CanJump = false;
    }

    void Look( Quaternion q_CameraVertRot_ )
    {
        playerCam[0].transform.rotation = q_CameraVertRot_;
    }

    void TEMPORARY_CAMERA_SYSTEM()
    {
        float f_LerpTime = 0.1f;

        if( b_Sprinting )
        {
            f_FOV = Mathf.Lerp(playerCam[0].fieldOfView, F_SPRINTING_FOV, f_LerpTime);
        }
        else
        {
            f_FOV = Mathf.Lerp(playerCam[0].fieldOfView, f_NORMAL_FOV, f_LerpTime);
        }

        playerCam[0].fieldOfView = f_FOV;
    }

    void SetMouseSmoothing(bool b_IsMouseSmooth_, float f_lookSmoothDamp_ = 0.1f)
    {
        // If Mouse isn't smooth, turn off the smoothing. Otherwise, set to default.
        if (!b_IsMouseSmooth_) f_lookSmoothDamp = 0.05f; else f_lookSmoothDamp = f_lookSmoothDamp_;
    }
}
