using UnityEngine;
using System.Collections;

public class Cs_CameraController : MonoBehaviour
{
    public GameObject go_CamReference;
    Vector3 newPos;
    Quaternion newRot;

	// Use this for initialization
	void Start ()
    {
        newPos = go_CamReference.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(go_CamReference)
        {
            // Lerp to the cam reference's position
            newPos = Vector3.Lerp(gameObject.transform.position, go_CamReference.transform.position, 5f);

            // Slerp to the cam reference's rotation
            newRot = Quaternion.Slerp(gameObject.transform.rotation, go_CamReference.transform.rotation, 0.5f);

            // Set new information
            gameObject.transform.position = newPos;
            gameObject.transform.rotation = newRot;
        }
	}
}