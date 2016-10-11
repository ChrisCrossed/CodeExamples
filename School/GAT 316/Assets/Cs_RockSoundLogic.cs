using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cs_RockSoundLogic : MonoBehaviour
{
    List<GameObject> go_EnemyList = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
	    
	}

    public void MakeSound()
    {
        print("Making a sound...");

        for (int i = 0; i < go_EnemyList.Count; ++i)
        {
            if(go_EnemyList[i].GetComponent<Cs_EnemyLogic_Grunt>())
            {
                print("Telling " + go_EnemyList[i].name + " to go to: " + gameObject.transform.position);

                go_EnemyList[i].GetComponent<Cs_EnemyLogic_Grunt>().GoToState_InvestigateLocation(gameObject);
            }
        }
    }

    void OnTriggerEnter( Collider collider_ )
    {
        if(!(collider_.gameObject.tag == "Enemy"))
        {
            return;
        }

        for(int i = 0; i < go_EnemyList.Count; ++i)
        {
            if(go_EnemyList[i] == collider_.gameObject)
            {
                return;
            }
        }

        go_EnemyList.Add(collider_.gameObject);
    }

    void OnTriggerExit( Collider collider_ )
    {
        for (int i = 0; i < go_EnemyList.Count; ++i)
        {
            if (go_EnemyList[i] == collider_.gameObject)
            {
                go_EnemyList.RemoveAt(i);
            }
        }
    }
}
