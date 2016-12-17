using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_PlayerController_Infantry : Cs_InputManager
{
    float f_GroundAcceleration;
    float f_GroundAcceleration_Max = 5.0f;
    Cs_InputManager inputManager;

	// Use this for initialization
	void Start ()
    {
        playerInput = new PlayerInput();
        Init_ResetControls();
    }

    void Movement()
    {
        print(playerInput.zDir);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has used the keyboard or mouse this frame, switch to Keyboard input. Otherwise, Controller.
        if (KeyboardCheck()) KeyboardInput(); else ControllerInput();

        Movement();
    }
}
