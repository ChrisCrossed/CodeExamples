using UnityEngine;
using System.Collections;

public class Cs_CameraTrigger : MonoBehaviour
{
    GameObject go_CameraObj;
    GameObject go_Player;

    // Use this for initialization
    void Start()
    {
        // Initialize objects
        go_Player = GameObject.Find("Player");

        go_CameraObj = transform.Find("go_CameraPos").gameObject;
    }

    void OnTriggerEnter(Collider collision_)
    {
        GameObject go_CollisionObj = collision_.transform.root.gameObject;

        if (go_CollisionObj.tag == "Player")
        {
            if (go_CameraObj != null)
            {
                // Tell player's camera to lerp to this game object
                go_Player.GetComponent<Cs_PlayerController>().SetCameraPosition(go_CameraObj);
            }
        }
    }

    void OnTriggerExit(Collider collision_)
    {
        GameObject go_CollisionObj = collision_.transform.root.gameObject;

        if (go_CollisionObj.tag == "Player")
        {
            if (go_CameraObj != null)
            {
                // Tell player's camera to return to default
                go_Player.GetComponent<Cs_PlayerController>().SetCameraPosition();
            }
        }
    }
}
