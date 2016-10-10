using UnityEngine;
using System.Collections;

public class Cs_EnemyLogic_Grunt : MonoBehaviour
{
    public GameObject go_EndPoint;

	// Use this for initialization
	void Start ()
    {
        if(gameObject.GetComponent<NavMeshAgent>().enabled)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = go_EndPoint.transform.position;
        }
    }

    // Updates every 0.1 seconds as to not overload/stutter gameplay elements
    void UpdateTick()
    {
        if(Vector3.Distance(gameObject.transform.position, go_EndPoint.transform.position) > 2.0f)
        {
            if(gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                gameObject.GetComponent<NavMeshAgent>().destination = go_EndPoint.transform.position;
            }
        }
    }

    // Update is called once per frame
    float f_UpdateTimer;
	void Update ()
    {
        f_UpdateTimer += Time.deltaTime;

        if(f_UpdateTimer >= 0.1f)
        {
            f_UpdateTimer = 0.0f;

            UpdateTick();
        }
    }
}
