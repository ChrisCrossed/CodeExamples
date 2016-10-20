using UnityEngine;
using System.Collections;

public class Cs_EnemyVisionLogic : MonoBehaviour
{
    GameObject go_Root;
    GameObject go_RaycastPoint;

    GameObject go_Player;

    int i_LayerMask;
    int i_LayerMask_NotPlayer;

    // Use this for initialization
    void Start ()
    {
        go_Player = GameObject.Find("Player");
        go_Root = gameObject.transform.root.gameObject;
        go_RaycastPoint = go_Root.transform.Find("VisionRaycast").gameObject;

        #region PRESENTATION STUFF
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        #endregion

        // LayerMask info
        i_LayerMask = LayerMask.GetMask("Player", "Wall");
        i_LayerMask_NotPlayer = 9;  // Kinda have to hardcode the ground here. The player is '8', so anything greater than that is NOT the player. You can't use LayerMask.GetMask to return an int.
    }

    float f_SeePlayerTimer;
    bool b_PlayerInCollider;
    Vector3 v3_LastKnownLocation;
    // Update is called once per frame
    void Update ()
    {
        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_Patrol();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        */
    }

    void CheckToSeePlayer( Collider collider_ )
    {
        if (collider_.transform.root.gameObject.tag == "Player")
        {
            Vector3 v3_Vector = new Vector3(collider_.transform.root.gameObject.transform.position.x - go_RaycastPoint.transform.position.x,
                                            collider_.transform.root.gameObject.transform.position.y - go_RaycastPoint.transform.position.y,
                                            collider_.transform.root.gameObject.transform.position.z - go_RaycastPoint.transform.position.z);

            v3_Vector.Normalize();

            RaycastHit hit;

            // Find the line between the raycast point & where the player currently is
            if (Physics.Raycast(go_RaycastPoint.transform.position, v3_Vector, out hit, 10.0f, i_LayerMask))
            {
                if (hit.collider.gameObject.layer >= i_LayerMask_NotPlayer) return;

                Debug.DrawRay(go_RaycastPoint.transform.position, v3_Vector, Color.red, 5.0f);

                go_Player = hit.collider.gameObject;

                b_PlayerInCollider = true;

                v3_LastKnownLocation = go_Player.transform.position;

                go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_ChasePlayer(v3_LastKnownLocation, true);
            }
        }
    }

    void OnTriggerEnter( Collider collider_ )
    {
        #region Working Vision Code
        // If this is the VisionCone, we just see the player.
        if(gameObject.name == "VisionCone")
        {
            CheckToSeePlayer( collider_ );
        }
        #endregion

        if (b_PlayerInCollider) print("Collider touched, we see the player"); else print("Collider touched, we DO NOT see the player");
    }

    void OnTriggerStay( Collider collider_ )
    {
        // Check to see if this is the Radius Trigger (and not the Vision Trigger)
        if(gameObject.name == "RadiusTrigger")
        {
            // If this is the player...
            if (collider_.transform.root.gameObject.tag == "Player")
            {
                // Make sure the player is 'making noise' before trying to see the player
                if(go_Player.GetComponent<Rigidbody>().velocity.magnitude > float.Epsilon)
                {
                    // The player is 'making noise', so check to find the player
                    CheckToSeePlayer( collider_ );
                }
            }
        }
    }

    void OnTriggerExit( Collider collider_ )
    {
        // Only tries to go to 'InvestigateLocation' if we saw the player in the first place. Otherwise, we just continue.
        if(b_PlayerInCollider)
        {
            if (collider_.transform.root.gameObject.tag == "Player")
            {
                b_PlayerInCollider = false;

                if (v3_LastKnownLocation != new Vector3())
                {
                    go_Root.GetComponent<Cs_EnemyLogic_Grunt>().GoToState_InvestigateLocation(v3_LastKnownLocation);
                }
            }
        }
    }
}
