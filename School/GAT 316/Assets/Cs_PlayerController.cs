using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Cs_PlayerController : MonoBehaviour
{
    // PLAYER STATS & INFORMATION
    public float MAX_PLAYER_SPEED;
    public float ACCELERATION;
    [Range( 0, 1 )]
    public float f_Magnitude_Sneak;
    [Range(0, 1)]
    public float f_Magnitude_Brisk;
    [Range(0, 2)]
    public float f_Magnitude_Sprint;
    
    // Player variables
    Vector3 v3_CurrentVelocity;
    Vector3 v3_PriorVector;
    bool b_IsSprinting = false;

    // Controller vs. Keyboard - Last Used
    bool b_ControllerUsedLast;
    float f_DoubleTapForSprintTimer;

    // Controller Input
    GamePadState state;
    GamePadState prevState;
    public PlayerIndex playerOne = PlayerIndex.One;

    // Raycast Objects
    GameObject go_SlopeRaycast;

    // Use this for initialization
    void Start ()
    {
        go_SlopeRaycast = transform.FindChild("SlopeRaycast").gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        

        Input_Keyboard();
        // Input_Controller();
	}

    void PlayerMovement( Vector3 v3_InputVector_, float f_Magnitude_ )
    {
        // Grab previous velocity to compare against
        Vector3 v3_PreviousVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        Vector3 v3_NewVelocity = v3_InputVector_ * MAX_PLAYER_SPEED * f_Magnitude_;

        v3_CurrentVelocity = Vector3.Lerp(v3_PreviousVelocity, v3_NewVelocity, ACCELERATION * Time.deltaTime);

        // Receive the ramp angle below the player
        RaycastHit rayHit = EvaluateGroundVector();

        Vector3 v3_FinalVelocity = Vector3.ProjectOnPlane( v3_CurrentVelocity, rayHit.normal );

        if(rayHit.distance >= 0.265f)
        {
            v3_FinalVelocity.y = v3_PreviousVelocity.y - (Time.deltaTime * 50);
        }

        gameObject.GetComponent<Rigidbody>().velocity = v3_FinalVelocity;
    }

    void Input_Controller()
    {
        // Capture latest input
        prevState = state;
        state = GamePad.GetState(playerOne);

        #region Movement
        // Create new temporary Vector3 to apply Controller input
        Vector3 v3_InputVector = new Vector3();

        // Accept Left Analog Stick input, apply into Vector3
        v3_InputVector.x = state.ThumbSticks.Left.X;
        v3_InputVector.z = state.ThumbSticks.Left.Y;
        #endregion

        #region Sprinting
        float f_Magnitude = 0f;

        if( !b_IsSprinting)
        {
            if (state.Buttons.LeftStick == ButtonState.Pressed && prevState.Buttons.LeftStick == ButtonState.Released) b_IsSprinting = true;
        }
        else
        {
            if (v3_InputVector.magnitude < 0.1f) b_IsSprinting = false;
        }
        
        // If the player speed isn't 0, apply preset speeds
        if (v3_InputVector.magnitude != float.Epsilon)
        {
            print(v3_InputVector.magnitude);

            if      (v3_InputVector.magnitude < 0.15f)   f_Magnitude = 0;
            else if (v3_InputVector.magnitude < 0.5f)   f_Magnitude = f_Magnitude_Sneak;
            else    f_Magnitude = f_Magnitude_Brisk;

            if (b_IsSprinting)   f_Magnitude = f_Magnitude_Sprint;
        }
        #endregion

        // Normalize
        v3_InputVector.Normalize();

        // Pass information into PlayerMovement()
        PlayerMovement(v3_InputVector, f_Magnitude);

    }

    void Input_Keyboard()
    {
        #region Movement
        // Create new temporary Vector3 to apply keyboard input
        Vector3 v3_InputVector = new Vector3();

        // Accept keyboard input, apply into Vector3
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) v3_InputVector.z = 1;
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) v3_InputVector.z = -1;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) v3_InputVector.x = -1;
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) v3_InputVector.x = 1;
        #endregion

        #region Sprinting
        float f_Magnitude = 0f;

        #region Shift Key
        if (!b_IsSprinting)
        {
            if (Input.GetKeyDown( KeyCode.LeftShift )) b_IsSprinting = true;
        }
        else
        {
            if (v3_InputVector.magnitude < 0.1f) b_IsSprinting = false;
        }
        #endregion

        #region Double Tap Direction

        // Reference Variable
        float f_TIME_ALLOWANCE = 0.4f;

        // Decrement the timer if it's currently 'active'
        if( f_DoubleTapForSprintTimer > 0.0f)
        {
            // Decrement
            f_DoubleTapForSprintTimer -= Time.deltaTime;

            // Clamp
            if (f_DoubleTapForSprintTimer < 0.0f) f_DoubleTapForSprintTimer = 0.0f;
        }

        // If the player pressed a button, evaluate if we BEGIN sprinting
        if( Input.GetKeyDown( KeyCode.W ) ||
            Input.GetKeyDown( KeyCode.S ) ||
            Input.GetKeyDown( KeyCode.A ) ||
            Input.GetKeyDown( KeyCode.D ))
        {
            // First press
            if( f_DoubleTapForSprintTimer == 0.0f )
            {
                // Set timer to the Alloted Time to wait
                f_DoubleTapForSprintTimer = f_TIME_ALLOWANCE;

                v3_PriorVector = v3_InputVector;
            }
            else
            {
                // This is the second (or up) time we pressed a movement button within the time limit. Sprint.
                b_IsSprinting = true;
            }
        }

        // If we're already sprinting and are holding a directional key down, keep the sprint timer up. Allows walking into sprinting.
        if(b_IsSprinting)
        {
            if ( Input.GetKey(KeyCode.W) ||
                 Input.GetKey(KeyCode.S) ||
                 Input.GetKey(KeyCode.A) ||
                 Input.GetKey(KeyCode.D))
            {
                f_DoubleTapForSprintTimer = f_TIME_ALLOWANCE;
            }
        }

        // print("Sprinting: " + b_IsSprinting + ", Timer: " + f_DoubleTapForSprintTimer);
        #endregion

        // If the player speed isn't 0, apply preset speeds
        if (v3_InputVector.magnitude != float.Epsilon)
        {
            if (v3_InputVector.magnitude < 0.15f) f_Magnitude = 0;
            else if (v3_InputVector.magnitude < 0.5f) f_Magnitude = f_Magnitude_Sneak;
            else f_Magnitude = f_Magnitude_Brisk;

            if (b_IsSprinting) f_Magnitude = f_Magnitude_Sprint;
        }
        #endregion

        // Normalize
        v3_InputVector.Normalize();

        // Pass information into PlayerMovement()
        PlayerMovement(v3_InputVector, f_Magnitude);

        v3_PriorVector = v3_InputVector;
    }

    RaycastHit EvaluateGroundVector()
    {
        RaycastHit hit;

        Physics.Raycast(go_SlopeRaycast.transform.position, -transform.up, out hit);

        return hit;
    }
}
