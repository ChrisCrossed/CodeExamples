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

    bool b_CanJump;
    int i_GravityVelocityMultiplier;
    
    // Use this for initialization
    void Start ()
    {
        b_CanJump = true;

        i_GravityVelocityMultiplier = 1;
        f_MoveSpeedMultiplier = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Input_Keyboard();
        Input_Controller();

        // Check if the player's allowed to jump again
        UpdateJump();

    }

    void PlayerMovement(Vector3 v3_Direction_, bool b_Jump_, float f_Magnitude_ = 1)
    {
        // Old velocity
        Vector3 v3_oldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        // Lerp prior velocity into new velocity
        Vector3 v3_newVelocity = Vector3.Lerp(v3_oldVelocity, v3_Direction_ * MAX_MOVESPEED_FORWARD * f_MoveSpeedMultiplier * f_Magnitude_, 1 / ACCELERATION );

        // Return gravity
        if(!b_Jump_)
        {
            v3_newVelocity.y = v3_oldVelocity.y;
        }
        else
        {
            v3_newVelocity.y = ( v3_oldVelocity.y * i_GravityVelocityMultiplier ) + JUMP_HEIGHT;
        }

        // Apply velocity to player
        gameObject.GetComponent<Rigidbody>().velocity = v3_newVelocity;
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
            if( Input.GetKeyDown( KeyCode.Space ))
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
                f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 2, 0.5f);
            }
        }
        else
        {
            f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 1, 0.5f);
        }
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
        if (state.Buttons.LeftStick == ButtonState.Pressed)
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
        #endregion

        // Normalize vector
        v3_PlayerInput.Normalize();

        // Pass into PlayerMovement
        PlayerMovement( v3_PlayerInput, b_Jump, f_Magnitude );
    }

    void UpdateJump()
    {
        RaycastHit hit;

        if( Physics.Raycast( gameObject.transform.position, -transform.up, out hit, 1.5f))
        {
            if( hit.distance < 1.01f )
            {
                b_CanJump = true;
            }
            else
            {
                b_CanJump = false;
            }
        }

        print("Distance: " + hit.distance);

    }
}
