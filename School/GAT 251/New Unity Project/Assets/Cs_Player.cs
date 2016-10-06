using UnityEngine;
using System.Collections;

public class Cs_Player : MonoBehaviour
{
    Vector3 v3_PlayerLocation;

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        v3_PlayerLocation = gameObject.transform.position;

        v3_PlayerLocation.x += Time.deltaTime;

        // gameObject.transform.position = v3_PlayerLocation;
        gameObject.GetComponent<Rigidbody>().MovePosition(v3_PlayerLocation);
	}
}
