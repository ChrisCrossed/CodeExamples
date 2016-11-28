using UnityEngine;
using System.Collections;

public class Cs_ObjectiveLogic : MonoBehaviour
{
    GameObject mdl_Briefcase;

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        mdl_Briefcase = transform.Find("mdl_Briefcase").gameObject;
	}

    void OnTriggerEnter( Collider collider_ )
    {
        if (collider_.transform.root.gameObject.name == "Player")
        {
            mdl_Briefcase.GetComponent<Cs_BriefcaseLogic>().Set_PickedUp();
        }
    }
}
