using UnityEngine;
using System.Collections;

public class Cs_PlayerController : MonoBehaviour
{
    float f_Speed_Curr;
    float f_Speed_Max = 3f;
    float f_Speed_Acceleration = 15f;

    float f_TimeToMaxSpeed = 4f;

	// Use this for initialization
	void Start ()
    {
	
	}

    float f_AngleVert;
    float f_AngleHoriz;
    void MouseInput()
    {
        // Horizontal Mouse Input
        f_AngleHoriz += Input.GetAxis("Mouse X");

        // Vertical Mouse Input
        f_AngleVert += Input.GetAxis("Mouse Y");

        f_AngleVert = Mathf.Clamp(f_AngleVert, -80f, 80f);

        // Rotate player object for AngleHoriz

        // Rotate Camera object for AngleVert

    }

    void PlayerInput()
    {
        // Movement Vector
        Vector3 v3_InputVector = new Vector3();

        float f_SpeedTemp = f_Speed_Max;

        // Creeping. Walk slow.
        if(Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            f_SpeedTemp /= 2f;
        }
        // Sprint. Move fast.
        else if(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            f_SpeedTemp *= 2f;
        }

        if( Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) )
        {
            v3_InputVector.z = 1;
        }
        else if( Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) )
        {
            v3_InputVector.z = -1;
        }

        if( Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) )
        {
            v3_InputVector.x = -1;
        }
        else if( Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) )
        {
            v3_InputVector.x = 1;
        }

        if( v3_InputVector != new Vector3())
        {
            ManageSpeed( true );

            v3_InputVector.Normalize();
        }
        else
        {
            ManageSpeed( false );
        }

        Vector3 v3_PrevVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        // Add Personal Velocities
        if (b_IsJumping) v3_PrevVelocity += Jump(); // Jump bool is reset at the end of each frame

        Vector3 v3_NewVelocity = Vector3.Lerp(v3_PrevVelocity, v3_InputVector * f_SpeedTemp, Time.deltaTime * f_Speed_Acceleration);

        gameObject.GetComponent<Rigidbody>().velocity = v3_NewVelocity;
    }

    void ManageSpeed( bool b_IncreaseSpeed_ = false )
    {
        if(b_IncreaseSpeed_)
        {
            f_Speed_Curr += Time.deltaTime * f_Speed_Acceleration;

            if (f_Speed_Curr > f_Speed_Max) f_Speed_Curr = f_Speed_Max;
        }
        else
        {
            f_Speed_Curr -= Time.deltaTime * f_Speed_Acceleration;

            if (f_Speed_Curr < 0 ) f_Speed_Curr = 0f;
        }
    }

    #region Jump abilities
    bool b_IsJumping = false;
    void Set_Jump()
    {
        b_IsJumping = true;
    }

    public float f_JumpHeight;
    Vector3 Jump()
    {
        Vector3 v3_JumpVelocity = gameObject.transform.up * f_JumpHeight;

        return v3_JumpVelocity;
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {
        PlayerInput();

        // Deactivate abilities
        b_IsJumping = false;
	}

    void LateUpdate()
    {
        MouseInput();
    }
}
