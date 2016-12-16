using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public struct PlayerInput
{
    // Game Settings
    private bool _b_zDir; // Forward or Backward
    private bool _b_xDir; // Strafe Left or Right
    private bool _b_IsJumping;
}

public class Cs_InputManager : MonoBehaviour
{
    // Scripts
    Cs_PlayerController_Infantry PlayerCont_Infantry;

    // Controller scripts
    GamePadState p1_State;
    GamePadState p1_PrevState;
    PlayerIndex p1 = PlayerIndex.One;

    // Keyboard Button Presets
    KeyCode kc_Forward;
    KeyCode kc_Backward;
    KeyCode kc_StrafeLeft;
    KeyCode kc_StrafeRight;
    KeyCode kc_Jump;
    KeyCode kc_Sprint;
    KeyCode kc_Crouch;
    KeyCode kc_Use;
    KeyCode kc_WeaponOne;
    KeyCode kc_WeaponTwo;
    KeyCode kc_Grenade;
    KeyCode kc_Reload;

    // Use this for initialization
	void Start ()
    {
		
	}

    void ControllerInput()
    {
        p1_PrevState = p1_State;
        p1_State = GamePad.GetState(p1);
    }

    float f_yRot;
    float f_xRot;
    bool KeyboardCheck()
    {
        #region Keyboard Input
        if( Input.GetKey(kc_Forward) ||
            Input.GetKey(kc_Backward) ||
            Input.GetKey(kc_StrafeLeft) ||
            Input.GetKey(kc_StrafeRight) ||
            Input.GetKey(kc_Jump) ||
            Input.GetKey(kc_Sprint) ||
            Input.GetKey(kc_Crouch) ||
            Input.GetKey(kc_Use) ||
            Input.GetKey(kc_WeaponOne) ||
            Input.GetKey(kc_WeaponTwo) ||
            Input.GetKey(kc_Grenade) ||
            Input.GetKey(kc_Reload))
        {
            return true;
        }
        #endregion

        #region Mouse Input
        float f_yRot_Prev = f_yRot;
        float f_xRot_Prev = f_xRot;

        f_yRot += Input.GetAxis("Mouse Y");
        f_xRot += Input.GetAxis("Mouse X");

        if ((f_xRot_Prev != f_xRot) || (f_yRot_Prev != f_yRot)) return true;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) return true;
        #endregion

        return false;
    }
    void KeyboardInput()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        // If the player has used the keyboard or mouse this frame, switch to Keyboard input. Otherwise, Controller.
        if (KeyboardCheck()) KeyboardInput(); else ControllerInput();
	}
}
