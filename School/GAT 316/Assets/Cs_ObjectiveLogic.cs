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

            // 'Destroy' the wall guarding the briefcase
            GameObject go_Door = GameObject.Find("Gate_Exit");
            go_Door.GetComponent<BoxCollider>().isTrigger = true;
            go_Door.GetComponent<Cs_GateScript>().Set_DoorOpen(true);
            go_Door.GetComponent<Cs_GateScript>().Set_ObjectiveActive(false);
        }
    }
}
