using UnityEngine;
using System.Collections;

public class Cs_EnemyVisionLogic : MonoBehaviour
{
    GameObject go_Root;
    GameObject go_RaycastPoint;

    bool b_PRESENTATION_TEST;

    GameObject go_Player;

	// Use this for initialization
	void Start ()
    {
        go_Root = gameObject.transform.root.gameObject;
        go_RaycastPoint = go_Root.transform.Find("VisionRaycast").gameObject;

        #region PRESENTATION STUFF
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        b_PRESENTATION_TEST = false;
        #endregion
    }

    float f_SeePlayerTimer;
    bool b_PlayerInCollider;
    Vector3 v3_LastKnownLocation;
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_Patrol();

            b_PRESENTATION_TEST = false;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            b_PRESENTATION_TEST = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            b_PRESENTATION_TEST = true;
        }

        #region Run a test every .1 seconds to see if we see the player in the collider
        if(f_SeePlayerTimer <= 0.1f)
        {
            f_SeePlayerTimer += Time.deltaTime;

            if (f_SeePlayerTimer > 0.1f) f_SeePlayerTimer = 0.1f;
        }

        if(f_SeePlayerTimer == 0.1f && b_PlayerInCollider)
        {
            Vector3 v3_Vector = new Vector3(go_Player.transform.position.x - go_RaycastPoint.transform.position.x,
                                            go_Player.transform.position.y - go_RaycastPoint.transform.position.y,
                                            go_Player.transform.position.z - go_RaycastPoint.transform.position.z);

            v3_Vector.Normalize();

            RaycastHit hit;

            // Find the line between the raycast point & where the player currently is
            Physics.Raycast(go_RaycastPoint.transform.position, v3_Vector, out hit);

            Debug.DrawRay(go_RaycastPoint.transform.position, v3_Vector, Color.red, 50.0f);

            if (hit.collider.transform.root.tag == "Player")
            {
                v3_LastKnownLocation = hit.collider.transform.root.gameObject.transform.position;

                print(go_Root.name + " sees the player");

                go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_ChasePlayer(v3_LastKnownLocation, true);
            }
            else
            {
                b_PlayerInCollider = false;

                if (v3_LastKnownLocation != null)
                {
                    go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_InvestigateLocation(v3_LastKnownLocation);
                }
            }

            f_SeePlayerTimer = 0.0f;
            // go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_ChasePlayer(go_Player, false);
        }
        // go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_ChasePlayer(go_Player, false);
        #endregion
    }

    void OnTriggerEnter( Collider collider_ )
    {
        if(b_PRESENTATION_TEST)
        {
            #region Working Vision Code
            if (collider_.transform.root.gameObject.tag == "Player")
            {
                Vector3 v3_Vector = new Vector3(collider_.transform.root.gameObject.transform.position.x - go_RaycastPoint.transform.position.x,
                                                collider_.transform.root.gameObject.transform.position.y - go_RaycastPoint.transform.position.y,
                                                collider_.transform.root.gameObject.transform.position.z - go_RaycastPoint.transform.position.z);

                v3_Vector.Normalize();

                RaycastHit hit;

                // Find the line between the raycast point & where the player currently is
                Physics.Raycast(go_RaycastPoint.transform.position, v3_Vector, out hit);

                Debug.DrawRay(go_RaycastPoint.transform.position, v3_Vector, Color.red, 5.0f);

                if (hit.collider.transform.root.tag == "Player")
                {
                    go_Player = hit.collider.transform.root.gameObject;

                    b_PlayerInCollider = true;
                }
            }
            // else b_PlayerInCollider = false;
            #endregion
        }
    }
}
