using UnityEngine;
using System.Collections;

public class Cs_BossLogic : MonoBehaviour
{
    GameObject go_BossSpot_1;
    GameObject go_BossSpot_2;
    GameObject go_BossSpot_3;
    GameObject go_BossSpot_4;

    GameObject go_CurrBossSpot;
    NavMeshAgent nav_Destination;

    // Use this for initialization
    void Start ()
    {
        go_BossSpot_1 = GameObject.Find("BossSpot_1");
        go_BossSpot_2 = GameObject.Find("BossSpot_2");
        go_BossSpot_3 = GameObject.Find("BossSpot_3");
        go_BossSpot_4 = GameObject.Find("BossSpot_4");

        // print("Set: 1");
        go_CurrBossSpot = go_BossSpot_1;
        nav_Destination = gameObject.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        nav_Destination.SetDestination(go_CurrBossSpot.transform.position);

        if(nav_Destination.remainingDistance < 0.15f)
        {
            if(go_CurrBossSpot == go_BossSpot_1)
            {
                // print("Set: 2");
                go_CurrBossSpot = go_BossSpot_2;
            }
            else if (go_CurrBossSpot == go_BossSpot_2)
            {
                // print("Set: 3");
                go_CurrBossSpot = go_BossSpot_3;
            }
            else if (go_CurrBossSpot == go_BossSpot_3)
            {
                // print("Set: 4");
                go_CurrBossSpot = go_BossSpot_4;
            }
            else if (go_CurrBossSpot == go_BossSpot_4)
            {
                // print("Set: 1");
                go_CurrBossSpot = go_BossSpot_1;
            }
        }
	}
}
