using UnityEngine;
using System.Collections;

public class Cs_EmployeeLogic : MonoBehaviour
{
    Vector3 go_Pos1;
    Vector3 go_Pos2;
    NavMeshAgent navAgent;
    bool b_TestPosition;

	// Use this for initialization
	void Start ()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();

        go_Pos1 = GameObject.Find("EmployeeSpot_1").transform.position;
        go_Pos2 = GameObject.Find("EmployeeSpot_2").transform.position;

        navAgent.SetDestination(go_Pos1);

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(navAgent.remainingDistance < .15f)
        {
            b_TestPosition = !b_TestPosition;

            if (b_TestPosition) navAgent.SetDestination(go_Pos1);
            else navAgent.SetDestination(go_Pos2);
        }
	}
}
