/*******************************************************************************/
/*
\file    Cs_SkiingPlayerController.cs (GitHub edit)
\author  Christopher Christensen (C.Christensen)
\date    1/25/2017

\ Table of Contents:
    \ PlayerInput (Line 17) - Process that determines the player's velocity, creates a vector based on input and passes the data.
    \ Ski (Line 85) - When the player is on the ground, raycasts are performed to determine ground/platform slopes and apply
                      velocities based on the ground normals.

\brief   Player Input logic translated into velocity manipulation.
*/
/*******************************************************************************/

void PlayerInput()
{
    Vector3 v3_OldVelocity = gameObject.GetComponent<Rigidbody>().velocity;

    // If the player is in the air, do not manipulate their movement velocity
    RaycastHit hit = CheckRaycasts();
    if(hit.distance < 2f)
    {
        float f_JumpVelocity = v3_OldVelocity.y;

        Vector3 v3_InputVelocity = new Vector3();

        if(b_LookHorizontalAllowed_Tutorial)
        {
            if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                v3_InputVelocity.z = 1;
            }
            else if(!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                v3_InputVelocity.z = -1;
            }

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                v3_InputVelocity.x = -1;
            }
            else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                v3_InputVelocity.x = 1;
            }
        }

        v3_InputVelocity.Normalize();
        
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D))
        {
            f_Speed += Time.deltaTime * f_Acceleration;

            if (f_Speed > f_MaxRunSpeed) f_Speed = f_MaxRunSpeed;
        }
        else
        {
            f_Speed -= Time.deltaTime * f_Acceleration;

            if (f_Speed < 0) f_Speed = 0;
        }

        // Aligning vector to that of the player's rotation
        Vector3 v3_FinalVelocity = Vector3.Lerp(v3_OldVelocity, gameObject.transform.rotation * v3_InputVelocity * f_Speed, 0.1f);

        // Restore y velocity
        v3_FinalVelocity.y = f_JumpVelocity;

        // Project upon a plane
        v3_NewVelocity = Vector3.ProjectOnPlane(v3_NewVelocity, -hit.normal);

        // Set final rotation
        gameObject.GetComponent<Rigidbody>().velocity = v3_FinalVelocity;
    }
    else
    {
        // We're above the ground. Disable the ability to jump.
        b_CanJump = false;
    }
}

void Ski()
{
    #region Ski (if in the air)
    // Raycast down and grab the angle of the terrain
    RaycastHit hit = CheckRaycasts();

    // This checks to be sure there is ground below us & it is within a certain distance
    if (hit.distance < 1.1f && (hit.normal != new Vector3()))
    {
        if (v3_Velocity == new Vector3())
        {
            // Set PhysicsMaterial
            gameObject.GetComponent<Collider>().material = physMat_Ski;
            
            if (f_MaxSpeed > 0)
            {
                // v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;
                v3_Velocity = new Vector3(v3_Velocity.x, 0, v3_Velocity.z);
                v3_Velocity = Vector3.ProjectOnPlane(v3_Velocity, hit.normal);
                v3_Velocity.Normalize();
                v3_Velocity *= f_MaxSpeed;
            }
        }
        else
        {
            v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;
        }

        // Increase grass skiing volume
        SetVolume( Enum_SFX.Grass, true );

        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= v3_Velocity.magnitude)
        {
            gameObject.GetComponent<Rigidbody>().velocity = v3_Velocity;
        }
    }
    else
    {
        // Checks the player's current speed while flying. Soft-caps & reduces their horizontal speed
        if( gameObject.GetComponent<Rigidbody>().velocity.magnitude > 50f)
        {
            float f_yVel = gameObject.GetComponent<Rigidbody>().velocity.y;

            Vector3 v3_CurrVelocity_ = gameObject.GetComponent<Rigidbody>().velocity;
            v3_CurrVelocity_.Normalize();
            v3_CurrVelocity_ *= 50f;
            gameObject.GetComponent<Rigidbody>().velocity = v3_CurrVelocity_;
        }
        else if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > f_MaxSpeed)
        {
            Vector3 v3_CurrVelocity_ = gameObject.GetComponent<Rigidbody>().velocity;
            v3_CurrVelocity_.x *= 0.98f;
            v3_CurrVelocity_.z *= 0.98f;
            gameObject.GetComponent<Rigidbody>().velocity = v3_CurrVelocity_;
        }

        // Decrease grass skiing volume
        SetVolume( Enum_SFX.Grass, false );
    }
    #endregion
}
