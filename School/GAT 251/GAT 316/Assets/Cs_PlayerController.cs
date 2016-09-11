using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Cs_PlayerController : MonoBehaviour
{
    // PLAYER STATS & INFORMATION
    public float MAX_PLAYER_SPEED;
    public float ACCELERATION;

    // Player variables
    Vector3 v3_CurrentVelocity;

    // Controller vs. Keyboard - Last Used
    bool b_ControllerUsedLast;

    // Controller Input
    GamePadState state;
    GamePadState prevState;
    GamePadState CONTROLLER_START_STATE;
    public PlayerIndex playerOne = PlayerIndex.One;

    // Use this for initialization
    void Start ()
    {
        CONTROLLER_START_STATE = GamePad.GetState(playerOne);
    }
	
	// Update is called once per frame
	void Update ()
    {
        

        // Input_Keyboard();
        Input_Controller();
	}

    void PlayerMovement( Vector3 v3_InputVector_, float f_Magnitude_ )
    {
        // Grab previous velocity to compare against
        Vector3 v3_PreviousVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        Vector3 v3_NewVelocity = v3_InputVector_ * MAX_PLAYER_SPEED * f_Magnitude_;

        v3_CurrentVelocity = Vector3.Lerp(v3_PreviousVelocity, v3_NewVelocity, ACCELERATION * Time.deltaTime);

        gameObject.GetComponent<Rigidbody>().velocity = v3_CurrentVelocity;
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

        float f_Magnitude = v3_InputVector.magnitude;
        
        // If the player speed isn't 0, apply preset speeds
        if ( f_Magnitude != 0f)
        {
            if      (f_Magnitude < 0.15f)   f_Magnitude = 0f;
            else if (f_Magnitude < 0.35f)   f_Magnitude = 0.35f;
            else if (f_Magnitude < 0.7f)    f_Magnitude = 0.7f;
            else    f_Magnitude = 1.0f;
        }

        // Normalize
        v3_InputVector.Normalize();
        #endregion

        #region Magnitude

        #endregion

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

        // Normalize
        v3_InputVector.Normalize();
        #endregion

        // Pass information into PlayerMovement()
        PlayerMovement(v3_InputVector, 1);
    }
}
