using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_PlayerController_Infantry : Cs_InputManager
{
    // Object Connection
    Cs_InputManager inputManager;
    GameObject go_Camera;

    // Speed Variables
    float f_GroundAcceleration;
    static float f_GroundAcceleration_Max = 5.0f;

    // Mouse Look Variables
    static float f_VertRotation_Max = 60f;

    internal void Initialize()
    {
        go_Camera = transform.root.FindChild("Main Camera").gameObject;
    }

    void ReceiveExternalVelocities()
    {
        // Raycast downward to find the velocity of whatever we're standing on. Apply that velocity to the player.
    }

    void Movement()
    {
        Vector3 v3_OldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        Vector3 v3_RampDirection = new Vector3();

        // Find directional vector
        Vector3 v3_Vector = new Vector3(playerInput.xDir, 0, playerInput.zDir);
        v3_Vector.Normalize();

        // Combine (not multiply) the player's current rotation (Quat) into the input vector (Vec3)
        Vector3 v3_FinalRotation = gameObject.transform.rotation * v3_Vector;

        Vector3 v3_NewVelocity = Vector3.Lerp(v3_OldVelocity, v3_FinalRotation * 10, 1f / 10f);

        gameObject.GetComponent<Rigidbody>().velocity = v3_NewVelocity;
    }

    float f_yRot;
    float f_yRot_Vel;
    float f_lookSmoothDamp = 0.1f;
    float f_yRot_Curr;
    void MouseRotations()
    {
        #region Horizontal rotation (y axis)
        Vector3 v3_CurrRot = gameObject.transform.eulerAngles;
        v3_CurrRot.y += playerInput.mouseHoriz;
        gameObject.transform.eulerAngles = v3_CurrRot;
        #endregion

        #region Vertical (x axis)
        f_yRot -= playerInput.mouseVert; // Inverted
        print(f_yRot);

        // Clamp
        f_yRot = Mathf.Clamp(f_yRot, -89, 89);
        f_yRot_Curr = Mathf.SmoothDamp(f_yRot_Curr, f_yRot, ref f_yRot_Vel, f_lookSmoothDamp);

        // go_Camera.transform.rotation = Quaternion.Euler(-f_yRot_Curr, 0, 0);
        go_Camera.transform.eulerAngles = new Vector3(f_yRot_Curr, v3_CurrRot.y, 0);
        #endregion
    }

    // Update is called once per frame
    internal void Update()
    {
        // Call's the input manager's Update
        base.InputUpdate();

        ReceiveExternalVelocities();
        MouseRotations();
        Movement();
    }
}
