using UnityEngine;
using System.Collections;

public class Cs_DisableTutorial : MonoBehaviour
{

	void OnTriggerEnter( Collider collider_ )
    {
        if( collider_.transform.root.gameObject.name == "Player" )
        {
            if(GameObject.Find("Canvas").GetComponent<Cs_Tutorial>())
            {
                GameObject.Find("Canvas").GetComponent<Cs_Tutorial>().Set_DeactivateTutorial();
            }
        }
    }
}
