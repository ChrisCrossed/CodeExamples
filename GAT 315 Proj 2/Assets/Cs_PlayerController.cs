using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Controller input

enum CurrentGear
{
    Zero    = 1,
    One     = 2,
    Two     = 3,
    Three   = 4,
    Four    = 5,
    Off     = -1
}

public class Cs_PlayerController : MonoBehaviour
{
    GamePadState state;
    GamePadState prevState;
    public PlayerIndex playerIndex = PlayerIndex.One;

    float f_CurrSpeed;
    public float f_MaxSpeed_GearZero = 10f;
    public float f_Acceleration = 5f;
    float f_CurrSpeed_Min;
    float f_CurrSpeed_Max;
    CurrentGear enum_CurrGear;

    // Use this for initialization
    void Start ()
    {
        f_CurrSpeed = 0f;

        enum_CurrGear = CurrentGear.Off;

    }

	// Update is called once per frame
	void Update ()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        if (Input.GetKeyDown(KeyCode.P) && enum_CurrGear == CurrentGear.Off)
        {
            print("Activated");
            enum_CurrGear = CurrentGear.Zero;
            SetGearMinMax(0);
        }

        if(enum_CurrGear != CurrentGear.Off)
        {
            Update_Speed();
            TurnCycle();
        }
	}

    void Update_Speed()
    {
        #region Cap player speed
        if (f_CurrSpeed < f_CurrSpeed_Min) f_CurrSpeed += f_Acceleration * Time.deltaTime;
        if (f_CurrSpeed > f_CurrSpeed_Max) f_CurrSpeed -= f_Acceleration * Time.deltaTime;

        // Cap the speed
        /*if(f_CurrSpeed > f_CurrSpeed_Min && f_CurrSpeed < f_CurrSpeed_Min + 0.1f)
        {
            f_CurrSpeed = f_CurrSpeed_Min;
        }
        if(f_CurrSpeed < f_CurrSpeed_Max && f_CurrSpeed > f_CurrSpeed_Max - 0.1f)
        {
            f_CurrSpeed = f_CurrSpeed_Max;
        }*/
        #endregion

        bool b_RightTrigger = (state.Triggers.Right >= 0.5f);
        bool b_LeftTrigger = (state.Triggers.Left >= 0.5f) && !b_RightTrigger;

        // Thumbstick 
        if (b_RightTrigger)
        {
            // Increase speed based on Acceleration
            f_CurrSpeed += (f_Acceleration / GetIntFromEnum(enum_CurrGear)) * Time.deltaTime;
        }
        else if(b_LeftTrigger)
        {
            // Decrease speed based on Acceleration * 5 (Brakes)
        }
        else
        {
            // Decrease speed based on Acceleration (Natural slow)
        }

        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * f_CurrSpeed;
        print(f_CurrSpeed);
    }

    int GetIntFromEnum(CurrentGear currGear_)
    {
        if (currGear_ == CurrentGear.Zero)  return 1;
        if (currGear_ == CurrentGear.One)   return 2;
        if (currGear_ == CurrentGear.Two)   return 3;
        if (currGear_ == CurrentGear.Three) return 4;
        if (currGear_ == CurrentGear.Four)  return 5;

        return 1;
    }

    void SetGearMinMax(int newGear_)
    {
        f_CurrSpeed_Min = newGear_ * f_MaxSpeed_GearZero;
        print("Min: " + f_CurrSpeed_Min);
        f_CurrSpeed_Max = (newGear_ + 1) * f_MaxSpeed_GearZero;
        print("Max: " + f_CurrSpeed_Max);
    }

    void ToggleDriveMode()
    {

    }

    void TurnCycle()
    {
        if(state.ThumbSticks.Left.X <= -0.1f || state.ThumbSticks.Left.X >= 0.1f)
        {
            var currRot = gameObject.transform.eulerAngles;
            currRot.y += state.ThumbSticks.Left.X;
            gameObject.transform.eulerAngles = currRot;
        }
    }

    void ToggleGear(bool b_IncreaseGear_)
    {

    }

    void Penalize()
    {

    }
	
}
