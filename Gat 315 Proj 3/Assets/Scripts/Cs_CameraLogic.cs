using UnityEngine;
using System.Collections;

enum MouseState
{
    Off, // Default position. Set to Off the 1st frame after 'Released'
    Pressed, // Mouse is pressed
    Released, // Mouse is Released
    Held // If Mouse is still held after 0.3f, becomes Held
}

public class Cs_CameraLogic : MonoBehaviour
{
    MouseState mouseState;
    MouseState prevState;
    float f_MouseTimer;
    const float MAX_MOUSE_HELD = 1.0f;
       
	// Use this for initialization
	void Start ()
    {
        mouseState = MouseState.Off;
        f_MouseTimer = 0f;
	}

    void SetMouseState()
    {
        // Search for an excuse for the mouse to be considered off
        if(Input.GetMouseButtonUp(0) || mouseState == MouseState.Released)
        {
            // If the mouse was pressed last frame, release it now. Otherwise, shut it off.
            if (mouseState == MouseState.Held || mouseState == MouseState.Pressed) { mouseState = MouseState.Released; f_MouseTimer = 0f; return; }
            else { mouseState = MouseState.Off; return; }
        }

        if(mouseState == MouseState.Pressed || mouseState == MouseState.Held)
        {
            f_MouseTimer += Time.deltaTime;

            // If the button has been held less than 0.3f, then it's pressed. Otherwise, it's held down.
            if (f_MouseTimer < MAX_MOUSE_HELD) { mouseState = MouseState.Pressed; return; }
            else { mouseState = MouseState.Held; return; }
        }

        // Mouse pressed
        if (Input.GetMouseButtonDown(0))
        {
            mouseState = MouseState.Pressed;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update mouse information
        SetMouseState();
        
        if(mouseState == MouseState.Pressed && prevState != MouseState.Pressed)
        {
            RaycastHit hit;
            Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                // Do something with the object that was hit by the raycast.
                // print(objectHit.name);

                // Check to see if the object, when clicked, has an appropriate collider. If it does, attempt to create a wall.
                if(objectHit.GetComponent<Cs_GridObjectLogic>())
                {
                    objectHit.GetComponent<Cs_GridObjectLogic>().ToggleGameObjects();
                }
            }
        }
        
        // Store the previous state
        prevState = mouseState;
    }
}
