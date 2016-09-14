﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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

    bool b_Keyboard;
    public bool b_MouseSmoothing = true;
    float f_LookSensitivity = 5f;
    float f_lookSmoothDamp = 0.1f;
    float f_yRot;
    float f_yRot_Curr;
    float f_yRot_Vel;
    float f_xRot;
    float f_xRot_Curr;
    float f_xRot_Vel;

    bool b_CanJump;
    GameObject go_RaycastObj;
    float f_JumpTimer;
    bool b_Sprinting;
    float f_RayCast_DownwardDistance;

    Camera[] playerCam;
    float f_FOV;
    public float f_NORMAL_FOV;
    public float F_SPRINTING_FOV;
    public bool Xbox_Camera_Inverted = false;
    float INVERTED_CAMERA_MULTIPLIER;

    float f_UITimer = 3.0f;
    string s_Text;

    // Use this for initialization
    void Start()
    {
        // Disable mouse cursor
        Cursor.visible = false;

        // Set the Camera on the controller to be 'Standard' viewing (Default: Up is Up)
        if (Xbox_Camera_Inverted) INVERTED_CAMERA_MULTIPLIER = -1; else INVERTED_CAMERA_MULTIPLIER = 1;

        SetMouseSmoothing(b_MouseSmoothing, 0.1f);

        b_Keyboard = false;
        b_CanJump = true;
        go_RaycastObj = gameObject.transform.Find("JumpRaycast").gameObject;
        f_RayCast_DownwardDistance = 0.25f;
        f_MoveSpeedMultiplier = 1;

        playerCam = gameObject.GetComponentsInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        b_Keyboard = KeyboardCheck(b_Keyboard);

        // These need to run every frame due to Lerping
        Look_Controller();
        Look_Mouse();

        // However, we want to restrict movement conflictions as much as possible.
        if (b_Keyboard) { Input_Keyboard(); } else { Input_Controller(); }

        // Check if the player's allowed to jump again
        UpdateJump();

        f_UITimer += Time.deltaTime;

        TEMPORARY_UI_SYSTEM();

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    bool KeyboardCheck(bool b_KeyboardPressed)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            b_MouseSmoothing = !b_MouseSmoothing;
            SetMouseSmoothing(b_MouseSmoothing);

            if (b_MouseSmoothing) TEMPORARY_UI_SYSTEM("Smooth Look: Enabled", true); else TEMPORARY_UI_SYSTEM("Smooth Look: Disabled", true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            INVERTED_CAMERA_MULTIPLIER *= -1;

            if (INVERTED_CAMERA_MULTIPLIER == -1)
            {
                TEMPORARY_UI_SYSTEM("Controller: Inverted", true);
            }
            else
            {
                TEMPORARY_UI_SYSTEM("Controller: Standard", true);
            }
        }

        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.Space))
        {
            return true;
        }

        return false;
    }

    void PlayerMovement(Vector3 v3_Direction_, bool b_Jump_, float f_Magnitude_ = 1)
    {
        // Old velocity
        Vector3 v3_oldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        // Combine (not multiply) the player's current rotation (Quat) into the input vector (Vec3)
        Vector3 v3_FinalRotation = gameObject.transform.rotation * v3_Direction_;

        // Lerp prior velocity into new velocity
        Vector3 v3_newVelocity = Vector3.Lerp(v3_oldVelocity, v3_FinalRotation * MAX_MOVESPEED_FORWARD * f_MoveSpeedMultiplier * f_Magnitude_, 1 / ACCELERATION);

        // Return gravity
        if (!b_Jump_)
        {
            // Synthetic terminal velocity
            RaycastHit hit;

            // Raycast straight down 
            Physics.Raycast(go_RaycastObj.transform.position, -transform.up, out hit);

            // Apply fake gravity (synthetic Terminal Velocity) - Note: RigidBody gravity is OFF
            if (hit.distance > f_RayCast_DownwardDistance) v3_newVelocity.y = v3_oldVelocity.y - (Time.deltaTime * 50);
        }
        else
        {
            // Apply a jump
            v3_newVelocity.y = JUMP_HEIGHT;

            ResetJump();
        }

        // Determine direction to push against ramp
        Vector3 v3_PushDirection = RampDirection();

        // Apply velocity to player
        v3_newVelocity = Vector3.ProjectOnPlane(v3_newVelocity, v3_PushDirection);

        gameObject.GetComponent<Rigidbody>().velocity = v3_newVelocity;
        gameObject.GetComponent<Rigidbody>().AddForce(v3_PushDirection);
    }

    Vector3 RampDirection()
    {
        // Stop player from sliding
        RaycastHit hit;

        // Raycast straight down 
        Physics.Raycast(gameObject.transform.position, -transform.up, out hit, f_RayCast_DownwardDistance);

        // Return the opposite direction against the ramp
        return -hit.normal;
    }

    void Input_Keyboard()
    {
        // Create a new Vector3
        Vector3 v3_PlayerInput = new Vector3();

        #region Input
        // Determine movement vector based on player input
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            v3_PlayerInput.z = 1;
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            v3_PlayerInput.z = -1;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            v3_PlayerInput.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            v3_PlayerInput.x = 1;
        }
        #endregion

        #region Jump
        bool b_Jump = false;
        if (b_CanJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                b_Jump = true;
                b_CanJump = false;
            }
        }
        #endregion

        #region Sprint
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (b_CanJump)
            {
                f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 1.25f, 0.5f);

                b_Sprinting = true;
            }
        }
        else
        {
            f_MoveSpeedMultiplier = Mathf.Lerp(f_MoveSpeedMultiplier, 1, 0.5f);

            b_Sprinting = false;
        }

        TEMPORARY_CAMERA_SYSTEM();
        #endregion

        // Normalize vector
        v3_PlayerInput.Normalize();

        // Pass into PlayerMovement
        PlayerMovement(v3_PlayerInput, b_Jump);
    }

    void Input_Controller()
    {
        // Update controller information
        prevState = state;
        state = GamePad.GetState(playerIndex);
        
        // Quit if 'Start' is pressed
        if(state.Buttons.Start == ButtonState.Pressed)
        {
            Application.Quit();
        }

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
        if (b_CanJump)
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
            b_Sprinting = true;
        }

        Vector2 MagnitudeTest = new Vector2();
        MagnitudeTest.x = state.ThumbSticks.Left.X;
        MagnitudeTest.y = state.ThumbSticks.Left.Y;

        if (MagnitudeTest.magnitude < 0.5f)
        {
            b_Sprinting = false;
        }

        if (b_Sprinting)
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

        TEMPORARY_CAMERA_SYSTEM();

        #endregion

        // Normalize vector
        v3_PlayerInput.Normalize();

        // Pass into PlayerMovement
        PlayerMovement(v3_PlayerInput, b_Jump, f_Magnitude);
    }

    void UpdateJump()
    {
        // Raycast Out object
        RaycastHit hit;

        b_CanJump = true;

        if (f_JumpTimer < 0.2f)
        {
            f_JumpTimer += Time.deltaTime;

            if (f_JumpTimer > 0.2f) f_JumpTimer = 0.2f;

            // Disable the ability to jump during this initial window
            b_CanJump = false;
        }
        else
        {
            // If the player isn't touching the ground, disable the ability to jump.
            if (!Physics.Raycast(go_RaycastObj.transform.position, -transform.up, out hit, 0.3f))
            {
                b_CanJump = false;
            }
        }

        // Check if the player's in the air.
        // Physics.Raycast( go_RaycastObj.transform.position, -transform.up, out hit
    }

    void ResetJump()
    {
        f_JumpTimer = 0.0f;

        b_CanJump = false;
    }

    void Look_Mouse()
    {
        #region Mouse Vertical

        // Update Mouse State
        f_yRot += Input.GetAxis("Mouse Y") * f_LookSensitivity;

        // Clamp the angles (Vertical only)
        f_yRot = Mathf.Clamp(f_yRot, -90, 90);

        // Smooth it out
        f_yRot_Curr = Mathf.SmoothDamp(f_yRot_Curr, f_yRot, ref f_yRot_Vel, f_lookSmoothDamp);
        #endregion

        #region Mouse Horizontal

        // Update Mouse State
        f_xRot += Input.GetAxis("Mouse X") * f_LookSensitivity;

        // Smooth it out
        f_xRot_Curr = Mathf.SmoothDamp(f_xRot_Curr, f_xRot, ref f_xRot_Vel, f_lookSmoothDamp);

        #endregion

        // The camera, although a child, is treated separately by Unity. Give it the X and Y.
        playerCam[0].transform.rotation = Quaternion.Euler(-f_yRot_Curr, f_xRot_Curr, 0);

        // However, the player object does not look up/down, but *does* rotate around 360 degrees.
        gameObject.transform.rotation = Quaternion.Euler(0, f_xRot_Curr, 0);
    }

    void Look_Controller()
    {
        #region Vertical (Right Analog Stick)

        // 
        f_yRot += state.ThumbSticks.Right.Y * f_LookSensitivity * INVERTED_CAMERA_MULTIPLIER / 2.75f;

        f_yRot = Mathf.Clamp(f_yRot, -90, 90);

        f_yRot_Curr = Mathf.SmoothDamp(f_yRot_Curr, f_yRot, ref f_yRot_Vel, f_lookSmoothDamp);

        #endregion

        #region Horizontal (Right Analog Stick)

        f_xRot += state.ThumbSticks.Right.X * f_LookSensitivity / 2.75f;

        f_xRot_Curr = Mathf.SmoothDamp(f_xRot_Curr, f_xRot, ref f_xRot_Vel, f_lookSmoothDamp);
        #endregion

        playerCam[0].transform.rotation = Quaternion.Euler(f_yRot_Curr, f_xRot_Curr, 0);
        gameObject.transform.rotation = Quaternion.Euler(0, f_xRot_Curr, 0);
    }

    void TEMPORARY_CAMERA_SYSTEM()
    {
        float f_LerpTime = 0.1f;

        if (b_Sprinting)
        {
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                f_FOV = Mathf.Lerp(playerCam[0].fieldOfView, F_SPRINTING_FOV, f_LerpTime);
            }
        }
        else
        {
            f_FOV = Mathf.Lerp(playerCam[0].fieldOfView, f_NORMAL_FOV, f_LerpTime);
        }

        playerCam[0].fieldOfView = f_FOV;
    }

    void TEMPORARY_UI_SYSTEM(string s_Info_ = null, bool b_ResetTimer = false)
    {
        if (b_ResetTimer) f_UITimer = 0.0f;

        if (s_Info_ != null) { s_Text = s_Info_; }

        f_UITimer += Time.deltaTime;
        print(f_UITimer);

        if( f_UITimer > 5.0f) 
        {
            s_Text = " ";
        }
        if( f_UITimer >= 30.0f)
        {
            s_Text = "Press (Letter) 'O' to make the camera glide. I'd love feedback on that!\nPressing 'P' inverts your controller's camera.";
        }
        if (f_UITimer > 60f) s_Text = " ";

        GameObject.Find("Text").gameObject.GetComponent<Text>().text = s_Text;
    }

    void SetMouseSmoothing(bool b_IsMouseSmooth_, float f_lookSmoothDamp_ = 0.1f)
    {
        // If Mouse isn't smooth, turn off the smoothing. Otherwise, set to default.
        if (!b_IsMouseSmooth_) f_lookSmoothDamp = 0.05f; else f_lookSmoothDamp = f_lookSmoothDamp_;
    }
}
