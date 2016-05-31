using UnityEngine;
using System.Collections;

public class Cs_PlayerController : Cs_InputManager
{
    public float f_MouseSensitivity = 3f;
    public bool b_MouseSmoothing = true;

    GameObject go_Player;

	// Use this for initialization
	void Start ()
    {
        go_Player = gameObject.transform.parent.gameObject;

        SetLookSensitivity(f_MouseSensitivity);
        SetMouseSmoothing(b_MouseSmoothing, 0.1f);
	}
	
	// Update is called once per frame
    void Update ()
    {
        #region Input Manager
        InputUpdate();
        #endregion

        // Update the camera rotation based on mouse input (Uses InputManager)
        gameObject.transform.rotation = GetRotation(gameObject.transform.eulerAngles);

        Vector3 newRot = go_Player.transform.eulerAngles;
        newRot.y = gameObject.transform.eulerAngles.y;
        go_Player.transform.eulerAngles = newRot;
	}
}
