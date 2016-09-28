using UnityEngine;
using System.Collections;

public class Cs_ObservatoryLogic : MonoBehaviour
{
    float f_Rotation;
    int i_NumberToLookAt;
    Vector3 v3_CurrentRotation = new Vector3();

    // Use this for initialization
    void Start ()
    {
	
	}

    public void SetPositionToLookAt(int i_Number_ = 0)
    {
        i_NumberToLookAt = i_Number_;
    }
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(new Vector3(0, i_NumberToLookAt * 45, 0)), Time.deltaTime / 2));
	}
}
