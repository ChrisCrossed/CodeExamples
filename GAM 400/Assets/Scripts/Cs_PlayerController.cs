using UnityEngine;
using System.Collections;

public class Cs_PlayerController : Cs_InputManager
{
    public float f_MouseSensitivity = 3f;
    public bool b_MouseSmoothing = true;

	// Use this for initialization
	void Start ()
    {
        SetLookSensitivity(f_MouseSensitivity);
        SetMouseSmoothing(b_MouseSmoothing, 0.001f);
	}
	
	// Update is called once per frame
    void Update ()
    {
        #region Input Manager
        InputUpdate();
        #endregion

        gameObject.transform.rotation = GetRotation(gameObject.transform.eulerAngles);
	}
}
