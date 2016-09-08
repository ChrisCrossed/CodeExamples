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
        PlayerMovement();
    }

    void PlayerMovement()
    {
        // Capture the current y velocity for ease
        float f_yVelocity = gameObject.GetComponent<Rigidbody>().velocity.y;

        // Create a new temporary velocity
        Vector3 v3_newVelocity = new Vector3();

        // Reset Angular drag
        gameObject.GetComponent<Rigidbody>().drag = 0f;

        // Set Buttons Pressed
        bool b_Forward = Input.GetKey(KeyCode.W);
        bool b_Backward = Input.GetKey(KeyCode.S);
        bool b_StrafeLeft = Input.GetKey(KeyCode.A);
        bool b_StrafeRight = Input.GetKey(KeyCode.D);

        print( b_Forward );
        print( b_Backward );

        #region Forward & Backward
        // When the player presses forward, push forward
        if ( b_Forward && !b_Backward)
        {
            // Increase player speed
            if( f_playerSpeed_Vert < MAX_MOVESPEED_FORWARD)
            {
                // Negate player speed before continuing
                if( f_playerSpeed_Vert < 0 )
                {
                    gameObject.GetComponent<Rigidbody>().drag = 5f;
                }

                f_playerSpeed_Vert += Time.deltaTime * ACCELERATION;
            }
        }
        else if( b_Backward && !b_Forward )
        {
            if( f_playerSpeed_Vert > -MAX_MOVESPEED_REVERSE )
            {
                // Negate player speed before continuing
                if ( f_playerSpeed_Vert > 0 )
                {
                    gameObject.GetComponent<Rigidbody>().drag = 5f;
                }

                f_playerSpeed_Vert -= Time.deltaTime * ACCELERATION;
            }   
        }
        else if( !b_Forward && !b_Backward )
        {
            gameObject.GetComponent<Rigidbody>().drag = 5f;

            if ( f_playerSpeed_Vert != 0 )
            {
                f_playerSpeed_Vert /= 4;
            }

            if( f_playerSpeed_Vert >= -0.5 && f_playerSpeed_Vert <= 0.5 )
            {
                f_playerSpeed_Vert = 0;
            }
        }

        v3_newVelocity += transform.forward * f_playerSpeed_Vert;
        #endregion

        #region Strafing
        
        // When the player presses forward, push forward
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            // Increase player speed
            if (f_playerSpeed_Horiz < MAX_MOVESPEED_FORWARD)
            {
                // Negate player speed before continuing
                if (f_playerSpeed_Horiz < 0)
                {
                    gameObject.GetComponent<Rigidbody>().drag = 5f;
                }

                f_playerSpeed_Horiz += Time.deltaTime * ACCELERATION;
            }
        }
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (f_playerSpeed_Horiz > -MAX_MOVESPEED_REVERSE)
            {
                // Negate player speed before continuing
                if (f_playerSpeed_Horiz > 0)
                {
                    gameObject.GetComponent<Rigidbody>().drag = 5f;
                }

                f_playerSpeed_Horiz -= Time.deltaTime * ACCELERATION;
            }
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Rigidbody>().drag = 5f;

            if (f_playerSpeed_Horiz != 0)
            {
                f_playerSpeed_Horiz /= 4;
            }

            if (f_playerSpeed_Horiz >= -0.5 && f_playerSpeed_Horiz <= 0.5)
            {
                f_playerSpeed_Horiz = 0;
            }
        }

        v3_newVelocity += transform.right * f_playerSpeed_Horiz;
        #endregion

        // Return gravity effects to the player
        v3_newVelocity.y += f_yVelocity;

        // Apply the new velocity to the player
        // gameObject.GetComponent<Rigidbody>().velocity = v3_newVelocity;
        if( gameObject.GetComponent<Rigidbody>().velocity.magnitude < MAX_MOVESPEED_FORWARD )
        {
            gameObject.GetComponent<Rigidbody>().AddForce(v3_newVelocity);
        }

        print(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
    }
}
