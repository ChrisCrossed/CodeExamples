using UnityEngine;
using System.Collections;

public class Cs_Siren : MonoBehaviour
{
    bool b_IsEnabled;

    GameObject go_LeftSirenModel;
    GameObject go_RightSirenModel;

    float f_RotSpeed = 135f;

    // Use this for initialization
    void Start ()
    {
        go_LeftSirenModel = transform.Find("Mdl_SoundVisual_Left").gameObject;
        go_RightSirenModel = transform.Find("Mdl_SoundVisual_Right").gameObject;

        Set_Enabled = true;
    }

    public bool Set_Enabled
    {
        set
        {
            // Set the visual siren object
            go_LeftSirenModel.GetComponent<MeshRenderer>().enabled = value;
            go_RightSirenModel.GetComponent<MeshRenderer>().enabled = value;

            b_IsEnabled = value;
        }
        get
        {
           return b_IsEnabled;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if( b_IsEnabled )
        {
            // Rotate the model
            Vector3 v3_Rotation = gameObject.transform.eulerAngles;
            v3_Rotation.y += Time.deltaTime * f_RotSpeed;
            v3_Rotation.y = Mathf.Clamp(v3_Rotation.y, 0f, 360f);
            gameObject.transform.eulerAngles = v3_Rotation;
        }
	}
}
