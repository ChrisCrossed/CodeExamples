using UnityEngine;
using System.Collections;

public class Cs_PlayerController : MonoBehaviour
{
    public float MaxWalkSpeed;
    public float MaxRunSpeed;
    public float MAX_ACCELERATION = 0.5f;

    public float f_MouseScalar = 1.0f;

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
        // Update mouse input
        UpdateInput();

        #region Directional Move

        // Move forward
        MoveDirection moveDir = MoveDirection.Stop;

        // Cardinal directions (If one button but not the other)
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) moveDir = MoveDirection.Forward;
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveDir = MoveDirection.Backward;
        else if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) moveDir = MoveDirection.StrafeLeft;
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveDir = MoveDirection.StrafeRight;

        // Diagonal directions (If forward/backward with one strafe but not the other)
        if (moveDir == MoveDirection.Forward && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) moveDir = MoveDirection.ForwardLeft;
        else if (moveDir == MoveDirection.Forward && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveDir = MoveDirection.ForwardRight;
        else if (moveDir == MoveDirection.Backward && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) moveDir = MoveDirection.BackwardLeft;
        else if (moveDir == MoveDirection.Backward && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveDir = MoveDirection.BackwardRight;
        
        MovePlayer(moveDir);

        #endregion

        #region Mouse Look
        UpdateMouseLook();
        #endregion
    }

    void UpdateMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the character with mouseX
        Vector3 playerRot = gameObject.transform.eulerAngles;
        playerRot.y += mouseX * f_MouseScalar;
        playerRot.y = Mathf.LerpAngle(gameObject.transform.eulerAngles.y, playerRot.y, 0.5f);
        gameObject.transform.eulerAngles = playerRot;

        // Rotate the camera with mouseY
        var camera = gameObject.GetComponentInChildren<Camera>().gameObject;
        Vector3 cameraRot = camera.transform.eulerAngles;
        cameraRot.x += -mouseY * f_MouseScalar;
        cameraRot.x = Mathf.LerpAngle(camera.transform.eulerAngles.x, cameraRot.x, 0.5f);
        camera.transform.eulerAngles = cameraRot;
    }

    void MovePlayer(MoveDirection moveDir_ = MoveDirection.Forward)
    {
        // Get the current movespeed to compare against
        float currVelocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        if (currVelocity <= 0.01) currVelocity = 0f;

        // Reset drag to a low amount so we can move
        gameObject.GetComponent<Rigidbody>().drag = 1;

        // If our total velocity is less than our max walkspeed, continue
        if (currVelocity < MaxWalkSpeed)
        {
            // Forward (Default)
            Vector3 newVelocity = new Vector3();

            if(moveDir_ == MoveDirection.Forward || moveDir_ == MoveDirection.ForwardLeft || moveDir_ == MoveDirection.ForwardRight)
            {
                newVelocity = gameObject.transform.forward * MAX_ACCELERATION;
            }

            // Backward
            if (moveDir_ == MoveDirection.Backward || moveDir_ == MoveDirection.BackwardLeft || moveDir_ ==  MoveDirection.BackwardRight)
            {
                newVelocity = -gameObject.transform.forward * MAX_ACCELERATION;
            }

            // Left
            if(moveDir_ == MoveDirection.StrafeLeft || moveDir_ == MoveDirection.ForwardLeft || moveDir_ == MoveDirection.BackwardLeft)
            {
                newVelocity += -gameObject.transform.right * MAX_ACCELERATION;
            }

            // Right
            if(moveDir_ == MoveDirection.StrafeRight || moveDir_ == MoveDirection.ForwardRight || moveDir_ == MoveDirection.BackwardRight)
            {
                newVelocity += gameObject.transform.right * MAX_ACCELERATION;
            }

            // Stop instead
            if (moveDir_ == MoveDirection.Stop)
            {
                newVelocity = new Vector3();
                gameObject.GetComponent<Rigidbody>().drag = 15;
            }

            newVelocity.y = gameObject.GetComponent<Rigidbody>().velocity.y;
            gameObject.GetComponent<Rigidbody>().velocity += newVelocity;
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

    ForwardLeft,
    ForwardRight,
    BackwardLeft,
    BackwardRight,

    Stop
}


// Doesn't work. Saving to ask about later.
public class InputManager : MonoBehaviour
{
    public enum KeyboardInput
    {
        Forward,
        Backward,
        StrafeLeft,
        StrafeRight,
        Jump
    }

    public enum ButtonState
    {
        Pressed,
        Released
    }

    class InputInformation
    {
        public KeyCode keyCode;
        public KeyboardInput keyboardInput;
        public ButtonState buttonState;

        public InputInformation(KeyCode keyCode_ = KeyCode.W, KeyboardInput keyboardInput = KeyboardInput.Forward)
        {
            keyCode = keyCode_;
            keyboardInput = KeyboardInput.Forward;
            buttonState = ButtonState.Released;
        }
    }

    InputInformation input_Forward = new InputInformation(KeyCode.W, KeyboardInput.Forward);
    InputInformation input_Backward = new InputInformation(KeyCode.S, KeyboardInput.Backward);

    public ButtonState GetButtonState(KeyboardInput keyboardInput_)
    {
        if (keyboardInput_ == KeyboardInput.Forward) return input_Forward.buttonState;
        else if (keyboardInput_ == KeyboardInput.Backward) return input_Backward.buttonState;

        return ButtonState.Released;
    }

    public void Update()
    {
        if (Input.GetKey(input_Forward.keyCode)) input_Forward.buttonState = ButtonState.Pressed; else input_Forward.buttonState = ButtonState.Released;
        if (Input.GetKey(input_Backward.keyCode)) input_Backward.buttonState = ButtonState.Pressed; else input_Backward.buttonState = ButtonState.Released;
    }
}
