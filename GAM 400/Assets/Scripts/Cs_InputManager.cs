using UnityEngine;
using System.Collections;

public class Cs_InputManager : MonoBehaviour
{
    // Mouse Look Input
    float f_LookSensitivity = 5f;
    float f_yRot;
    float f_xRot;
    float f_xRot_Curr;
    float f_yRot_Curr;
    float f_xRot_Vel;
    float f_yRot_Vel;
    float f_lookSmoothDamp = 0.1f;

    Quaternion quat_CurrRot;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	public void InputUpdate ()
    {
        f_yRot += Input.GetAxis("Mouse X") * f_LookSensitivity;
        f_xRot += Input.GetAxis("Mouse Y") * f_LookSensitivity * -1;
        
        f_xRot = Mathf.Clamp(f_xRot, -90, 90);

        f_xRot_Curr = Mathf.SmoothDamp(f_xRot_Curr, f_xRot, ref f_xRot_Vel, f_lookSmoothDamp);
        f_yRot_Curr = Mathf.SmoothDamp(f_yRot_Curr, f_yRot, ref f_yRot_Vel, f_lookSmoothDamp);

        quat_CurrRot = Quaternion.Euler(f_xRot_Curr, f_yRot_Curr, 0);

        print("X: " + Input.GetAxis("Mouse X"));
        print("Y: " + Input.GetAxis("Mouse Y"));
    }

    public void SetLookSensitivity(float f_LookSensitivity_)
    {
        f_LookSensitivity = f_LookSensitivity_;
    }

    public Quaternion GetRotation(Vector3 currRot_)
    {
        return quat_CurrRot;
    }
}
