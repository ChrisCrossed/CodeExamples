using UnityEngine;
using System.Collections;

public class Cs_FPSController : MonoBehaviour
{
    // Player Speed Reference
    public float MAX_MOVESPEED_FORWARD;
    public float MAX_MOVESPEED_REVERSE;
    public float ACCELERATION;
    float f_playerSpeed_Vert;
    float f_playerSpeed_Horiz;

    // Use this for initialization
    void Start ()
    {
        f_playerSpeed_Vert = 0;
        f_playerSpeed_Horiz = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Input_Keyboard();
        Input_Controller();
    }

    void PlayerMovement(Vector3 v3_Direction)
    {
        // Old velocity
        Vector3 v3_oldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        Vector3 v3_newVelocity = Vector3.Lerp(v3_oldVelocity, v3_Direction * MAX_MOVESPEED_FORWARD, 1 / ACCELERATION );

        gameObject.GetComponent<Rigidbody>().velocity = v3_newVelocity;
    }

    void Input_Keyboard()
    {
        // Create a new Vector3
        Vector3 v3_PlayerInput = new Vector3();

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

        // Normalize vector
        v3_PlayerInput.Normalize();

        // Pass into PlayerMovement
        PlayerMovement( v3_PlayerInput );
    }

    void Input_Controller()
    {
        // Create a new Vector3

        // Determine movement vector based on player input

        // Normalize vector

        // Pass into PlayerMovement
    }
}
