using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_PlayerController_Infantry : Cs_InputManager
{
    // Object Connection
    Cs_InputManager inputManager;
    GameObject go_Camera;
    GameObject go_Raycast_Front;
    GameObject go_Raycast_Back;
    GameObject go_Raycast_Left;
    GameObject go_Raycast_Right;

    // Jumping
    bool b_OnGround;
    bool b_JumpJetAllowed;

    // Speed Variables
    float f_GroundAcceleration;
    static float f_GroundAcceleration_Max = 5.0f;

    // Mouse Look Variables
    static float f_VertRotation_Max = 60f;

    internal void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Object Connection
        go_Camera = transform.root.FindChild("Main Camera").gameObject;
        go_Raycast_Front = transform.Find("RaycastObjects").Find("Raycast_Front").gameObject;
        go_Raycast_Back = transform.Find("RaycastObjects").Find("Raycast_Back").gameObject;
        go_Raycast_Left = transform.Find("RaycastObjects").Find("Raycast_Left").gameObject;
        go_Raycast_Right = transform.Find("RaycastObjects").Find("Raycast_Right").gameObject;
    }
    
    GameObject go_Platform;
    Vector3 v3_PlatformPreviousPos;
    // Finds the object the player is standing on, checks to see if it is moving, and applies that velocity to the player
    void ReceiveExternalVelocities()
    {
        #region Khan, here's the 'new' code: Applies the difference the object has moved. Both formats have been removed by latest verion.
        // Capture the gameobject we're standing on.
        RaycastHit hit = CheckRaycasts( LayerMask.GetMask("MovingPlatform") );

        if (hit.rigidbody)
        {
            print("Got here");

            // If it's not the same platform we were in contact with last frame
            if (go_Platform != hit.collider.gameObject)
            {
                // Store game object & position
                go_Platform = hit.collider.gameObject;
                v3_PlatformPreviousPos = hit.transform.position;
            }
            // Same platform as last frame.
            else
            {
                // Calculate differences and apply to player.
                Vector3 v3_Difference = hit.transform.position - v3_PlatformPreviousPos;
                gameObject.GetComponent<Rigidbody>().MovePosition( gameObject.transform.position + v3_Difference );

                // Store current position
                v3_PlatformPreviousPos = hit.transform.position;

                // Remove angular velocity
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        else
        {
            // Not the same game object. Clear data.
            go_Platform = null;
            v3_PlatformPreviousPos = new Vector3();
        }
        #endregion

        #region Khan, here's the old code. In various formats, the code would remain the same: Find the velocity of the touching object and add to player's velocity
        /*
        // Raycast downward to find the velocity of whatever we're standing on. Apply that velocity to the player.
        RaycastHit hit = CheckRaycasts();

        // Ground objects
        if(hit.collider != null)
        {
            // Get object's velocity if it has one
            Vector3 v3_TouchingVelocity = new Vector3();
            if(hit.rigidbody)
            {
                v3_TouchingVelocity = hit.rigidbody.velocity;
                v3_TouchingVelocity = v3_TouchingVelocity.normalized;
                return v3_TouchingVelocity;
            }
        }

        return new Vector3();
        */
        #endregion
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

    float f_CamRot;
    float f_CamRot_Vel;
    float f_lookSmoothDamp = 0.05f;
    float f_CamRot_Curr;
    void MouseRotations()
    {
        #region Horizontal rotation (y axis)
        Vector3 v3_CurrRot = gameObject.transform.eulerAngles;
        v3_CurrRot.y += playerInput.mouseHoriz;
        gameObject.transform.eulerAngles = v3_CurrRot;
        #endregion

        #region Vertical (x axis)
        f_CamRot -= playerInput.mouseVert; // Inverted

        // Clamp
        f_CamRot = Mathf.Clamp(f_CamRot, -89, 89);
        f_CamRot_Curr = Mathf.SmoothDamp(f_CamRot_Curr, f_CamRot, ref f_CamRot_Vel, f_lookSmoothDamp);
        
        go_Camera.transform.eulerAngles = new Vector3(f_CamRot_Curr, v3_CurrRot.y, 0);
        #endregion
    }
    
    // Raycasts down through four points on the player, finds the closes distance, and returns that RaycastHit.
    RaycastHit CheckRaycasts( int i_LayerMask_ = -1)
    {
        // outHit is what we'll be sending out from the function
        RaycastHit outHit;

        // tempHit is what we'll compare against
        RaycastHit tempHit;

        int i_LayerMask;
        if( i_LayerMask_ == -1)
        {
            i_LayerMask = LayerMask.GetMask("Ground");
        }
        else
        {
            i_LayerMask = i_LayerMask_;
        }

        // Set the default as outHit automatically
        Physics.Raycast(go_Raycast_Front.transform.position, -transform.up, out outHit, 0.15f, i_LayerMask);

        // Begin comparing against the other three. Find the shortest distance
        if (Physics.Raycast(go_Raycast_Back.transform.position, -transform.up, out tempHit, 0.15f, i_LayerMask))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        if (Physics.Raycast(go_Raycast_Left.transform.position, -transform.up, out tempHit, 0.15f, i_LayerMask))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        if (Physics.Raycast(go_Raycast_Right.transform.position, -transform.up, out tempHit, 0.15f, i_LayerMask))
        {
            if (tempHit.distance < outHit.distance) outHit = tempHit;
        }

        // Return the shortest hit distance
        return outHit;
    }

    // Update is called once per frame
    internal void FixedUpdate()
    {
        // Call's the input manager's Update
        base.InputUpdate();
        
        MouseRotations();
        Movement();
        ReceiveExternalVelocities();
    }

    #region Not Used
    void OnCollisionEnter(Collision collider_)
    {
        /*
        // If the object we're touching is the object directly below the player, we're touching the ground
        RaycastHit hit;
        int i_LayerMask = collider_.gameObject.layer;
        print(LayerMask.LayerToName(i_LayerMask));
        Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, Mathf.Infinity, i_LayerMask);
        
        if(hit.collider)
        {
            print("Touching: " + hit.collider.gameObject.layer);
        }

        // If the player is now touching a moving platform, parent it
        if (collider_.gameObject.layer == LayerMask.NameToLayer("MovingPlatform"))
        {
            gameObject.transform.SetParent(collider_.transform.root);
        }*/
    }

    void OnCollisionExit(Collision collider_)
    {
        /*
        // If the player's parent is the same as the parent of this moving platform...
        if (gameObject.transform.parent == collider_.transform.root)
        {
            // Release it
            gameObject.transform.parent = null;
        }
        */
    }
    #endregion
}
