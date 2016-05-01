using UnityEngine;
using System.Collections;

public class Cs_PlayerController : MonoBehaviour
{
    public float MaxWalkSpeed;
    public float MaxRunSpeed;
    public float MAX_ACCELERATION = 0.5f;

    #region Mouse input
    MouseState mouseState_Left;
    float f_MouseTimer_Left;

    MouseState mouseState_Right;
    float f_MouseTimer_Right;

    MouseState mouseState_Wheel;

    const float MAX_MOUSE_HELD = 1.0f;
    #endregion
    #region Keyboard input
    
    #endregion

    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        if (Input.GetKey(KeyCode.W)) WalkForward();
        else if (Input.GetKey(KeyCode.S)) WalkForward(MoveDirection.Backward);
        else WalkForward(MoveDirection.Stop);
    }

    void WalkForward(MoveDirection moveDir_ = MoveDirection.Forward)
    {
        float currVelocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        print(currVelocity);

        if(moveDir_ == MoveDirection.Forward)
        {
            if(currVelocity < MaxWalkSpeed)
            {
                gameObject.GetComponent<Rigidbody>().drag = 1;
                gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * gameObject.GetComponent<Rigidbody>().mass * MAX_ACCELERATION * 100);
            }
        }
        else if(moveDir_ == MoveDirection.Backward)
        {
            if(currVelocity < MaxWalkSpeed)
            {
                gameObject.GetComponent<Rigidbody>().drag = 1;
                gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * gameObject.GetComponent<Rigidbody>().mass * MAX_ACCELERATION * 100);
            }
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().drag = 10;
        }
    }

    #region UpdateInput
    void UpdateInput()
    {
        mouseState_Left = SetMouseState(MouseButtons.LeftMouse, mouseState_Left, f_MouseTimer_Left);
        mouseState_Right = SetMouseState(MouseButtons.RightMouse, mouseState_Right, f_MouseTimer_Right);
        mouseState_Wheel = SetMouseWheelState();

        // Update Keyboard Input
    }
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
    #endregion
}

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

public enum MoveDirection
{
    Forward,
    Backward,
    StrafeLeft,
    StrafeRight,
    Stop
}
