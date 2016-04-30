using UnityEngine;
using System.Collections;

/*******************************************************************************
filename    Cs_InputManager
author      Chris Christensen
DP email    C.Christensen@DigiPen.edu
course      GAM 400

Brief Description:
  This InputManager returns all keyboard & mouse input while being editable
    by the player in-game
  
*******************************************************************************/

public enum MouseState
{
    Off, // Default position. Set to Off the 1st frame after 'Released'
    Pressed, // Mouse is pressed
    Released, // Mouse is Released
    Held, // If Mouse is still held after 0.3f, becomes Held
    ScrollUp, // Scroll Wheel Up was performed
    ScrollDown // Scroll Wheel Down was performed
}

public enum MouseButtons
{
    LeftMouse,
    RightMouse,
    MouseWheel
}

public enum KeyboardButtonState { Pressed, Released }
public enum PlayerAction
{
    Forward,
    Backward,
    StrafeLeft,
    StrafeRight,

    Ability1,
    Ability2,

    Reload,
    Use,

    Jump,
    Crouch,
    ToggleWalk,
    ToggleRun,
}
public class KeyboardInput : MonoBehaviour
{
    KeyCode keyCode;
    PlayerAction playerAction;
    KeyboardButtonState buttonState;

    public void InitializeKeyboardInput(PlayerAction playerAction_, KeyCode keyCode_)
    {
        playerAction = playerAction_;

        if (playerAction_ == PlayerAction.Forward) keyCode = KeyCode.W;
        if (playerAction_ == PlayerAction.Backward) keyCode = KeyCode.S;
        if (playerAction_ == PlayerAction.StrafeLeft) keyCode = KeyCode.A;
        if (playerAction_ == PlayerAction.StrafeRight) keyCode = KeyCode.D;

        if (playerAction_ == PlayerAction.Ability1) keyCode = KeyCode.Q;
        if (playerAction_ == PlayerAction.Ability2) keyCode = KeyCode.E;

        if (playerAction_ == PlayerAction.Reload) keyCode = KeyCode.R;
        if (playerAction_ == PlayerAction.Use) keyCode = KeyCode.F;

        if (playerAction_ == PlayerAction.Jump) keyCode = KeyCode.Space;
        if (playerAction_ == PlayerAction.Crouch) keyCode = KeyCode.LeftControl;
        if (playerAction_ == PlayerAction.ToggleWalk) keyCode = KeyCode.Z;
        if (playerAction_ == PlayerAction.ToggleRun) keyCode = KeyCode.LeftShift;
    }

    public void SetKeyboardInput(KeyCode keyCode_)
    {
        keyCode = keyCode_;
    }

    public KeyboardButtonState GetButtonState()
    {
        return buttonState;
    }

    void Update()
    {
        if(buttonState == KeyboardButtonState.Released && !Input.GetKey(keyCode)) return;
        else
        {
            if (Input.GetKeyDown(keyCode)) buttonState = KeyboardButtonState.Pressed;
            else if (Input.GetKeyUp(keyCode)) buttonState = KeyboardButtonState.Released;
        }
    }
}

class KeyboardObject
{
    KeyboardInput forward = new KeyboardInput().InitializeKeyboardInput();
    KeyboardInput backward;
    KeyboardInput strafeLeft;
    KeyboardInput strafeRight;

    KeyboardInput reload;
    KeyboardInput use;

    
}

public class Cs_InputManager : MonoBehaviour
{
    MouseState mouseState_Left;
    float f_MouseTimer_Left;

    MouseState mouseState_Right;
    float f_MouseTimer_Right;

    MouseState mouseState_Wheel;

    const float MAX_MOUSE_HELD = 1.0f;

    void Start()
    {
        InitializeKeyboardInput();
    }

    void InitializeKeyboardInput()
    {

    }

    /*
    public void SetKeyboardInput(KeyboardAction keyboardButton_, KeyCode keyCode_)
    {
        if (keyboardButton_ == KeyboardAction.Forward) keyboardInput.key_Forward = keyCode_;
        if (keyboardButton_ == KeyboardAction.Backward) keyboardInput.key_Backward = keyCode_;
        if (keyboardButton_ == KeyboardAction.StrafeLeft) keyboardInput.key_StrafeLeft = keyCode_;
        if (keyboardButton_ == KeyboardAction.StrafeRight) keyboardInput.key_StrafeRight = keyCode_;

        if (keyboardButton_ == KeyboardAction.Ability1) keyboardInput.key_Ability1 = keyCode_;
        if (keyboardButton_ == KeyboardAction.Ability2) keyboardInput.key_Ability2 = keyCode_;

        if (keyboardButton_ == KeyboardAction.Reload) keyboardInput.key_Reload = keyCode_;
        if (keyboardButton_ == KeyboardAction.Use) keyboardInput.key_Use = keyCode_;

        if (keyboardButton_ == KeyboardAction.Jump) keyboardInput.key_Jump = keyCode_;
        if (keyboardButton_ == KeyboardAction.Crouch) keyboardInput.key_Crouch = keyCode_;
        if (keyboardButton_ == KeyboardAction.ToggleRun) keyboardInput.key_ToggleRun = keyCode_;
        if (keyboardButton_ == KeyboardAction.ToggleWalk) keyboardInput.key_ToggleWalk = keyCode_;
    }
    */
    
    public MouseState GetMouseState(MouseButtons mouseButton_)
    {
        if (mouseButton_ == MouseButtons.LeftMouse) return mouseState_Left;
        else if (mouseButton_ == MouseButtons.RightMouse) return mouseState_Right;

        // Return the mousewheel by default
        return mouseState_Wheel;
    }
    
    MouseState SetMouseState(MouseButtons mouseButtons_, MouseState mouseState_, float f_MouseTimer_)
    {
        // 0 is the default axis for Left Mouse Click
        int mouseAxis = 0;
        if (mouseButtons_ == MouseButtons.RightMouse) mouseAxis = 1;

        // Search for an excuse for the mouse to be considered off
        if (Input.GetMouseButtonUp(mouseAxis) || mouseState_ == MouseState.Released)
        {
            // If the mouse was pressed last frame, release it now. Otherwise, shut it off.
            if (mouseState_ == MouseState.Held || mouseState_ == MouseState.Pressed) { mouseState_ = MouseState.Released; f_MouseTimer_ = 0f; return mouseState_; }
            else { mouseState_ = MouseState.Off; return mouseState_; }
        }

        if (mouseState_ == MouseState.Pressed || mouseState_ == MouseState.Held)
        {
            f_MouseTimer_ += Time.deltaTime;

            // If the button has been held less than MAX_MOUSE_HELD, then it's pressed. Otherwise, it's held down.
            if (f_MouseTimer_ < MAX_MOUSE_HELD) { mouseState_ = MouseState.Pressed; return mouseState_; }
            else { mouseState_ = MouseState.Held; return mouseState_; }
        }

        // Mouse pressed
        if (Input.GetMouseButtonDown(mouseAxis))
        {
            mouseState_ = MouseState.Pressed;
            return mouseState_;
        }

        // Default
        mouseState_ = MouseState.Off;
        return mouseState_;
    }

    MouseState SetMouseWheelState()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            // If the Mouse scroll axis is > 0, the mouse scroll wheel is 'Up', otherwise it is down.
            if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) mouseState_Wheel = MouseState.ScrollUp; else mouseState_Wheel = MouseState.ScrollDown;
            return mouseState_Wheel;
        }

        mouseState_Wheel = MouseState.Off;
        return mouseState_Wheel;
    }

    // Update is called once per frame
    void Update()
    {
        mouseState_Left = SetMouseState(MouseButtons.LeftMouse, mouseState_Left, f_MouseTimer_Left);
        mouseState_Right = SetMouseState(MouseButtons.RightMouse, mouseState_Right, f_MouseTimer_Right);
        mouseState_Wheel = SetMouseWheelState();

        // Update Keyboard Input
    }
}
