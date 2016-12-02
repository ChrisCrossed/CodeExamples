using UnityEngine;
using System.Collections;

public class Cs_LimoLogic : MonoBehaviour
{
    GameObject go_LimoStop;
    GameObject go_LimoGoal;
    NavMeshAgent navMeshAgent;

	// Use this for initialization
	void Start ()
    {
        go_LimoGoal = GameObject.Find("CarGoal_2");
        go_LimoStop = GameObject.Find("LimoStop");

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 15f;

        gameObject.transform.position = GameObject.Find("CarStart_2").transform.position;

        Set_LimoStop();
	}

    void Set_LimoStop()
    {
        navMeshAgent.SetDestination(go_LimoStop.transform.position);
    }

    public void Set_LimoGoal()
    {
        navMeshAgent.SetDestination(go_LimoGoal.transform.position);
    }
}
