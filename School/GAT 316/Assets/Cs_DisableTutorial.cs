using UnityEngine;
using System.Collections;

public class Cs_DisableTutorial : MonoBehaviour
{
    Cs_RoadLogic cs_RoadLogic;

    void Start()
    {
        cs_RoadLogic = GameObject.Find("RoadLogic").GetComponent<Cs_RoadLogic>();
    }

	void OnTriggerEnter( Collider collider_ )
    {
        if( collider_.transform.root.gameObject.name == "Player" )
        {
            if(GameObject.Find("Canvas").GetComponent<Cs_Tutorial>())
            {
                GameObject.Find("Canvas").GetComponent<Cs_Tutorial>().Set_DeactivateTutorial();
            }
        }

        bool b_BriefcaseCollected = cs_RoadLogic.ObjectiveCollected;

        if(b_BriefcaseCollected)
        {
            // Tell the Road Logic system to run the limo
            cs_RoadLogic.Set_SwitchToLimo();
        }
    }
}
