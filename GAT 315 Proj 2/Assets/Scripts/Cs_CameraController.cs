using UnityEngine;
using System.Collections;

public class Cs_CameraController : MonoBehaviour
{
    public GameObject go_CamReference;
    Vector3 newPos;
    Quaternion newRot;

    bool b_CameraLockedToPlayer = false;
    float f_CamMoveTimer = 0.5f;

	// Use this for initialization
	void Start ()
    {
        newPos = go_CamReference.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(b_CameraLockedToPlayer)
        {
            if(go_CamReference)
            {
                if (f_CamMoveTimer < 1.0f) f_CamMoveTimer += Time.deltaTime; else f_CamMoveTimer = 1.0f;

                // Lerp to the cam reference's position
                newPos = Vector3.Lerp(gameObject.transform.position, go_CamReference.transform.position, f_CamMoveTimer);

                // Slerp to the cam reference's rotation
                newRot = Quaternion.Slerp(gameObject.transform.rotation, go_CamReference.transform.rotation, f_CamMoveTimer / 2);

                // Set new information
                gameObject.transform.position = newPos;
                gameObject.transform.rotation = newRot;
            }
        }
	}

    public void SetCameraLock(bool b_IsLockedToPlayer_)
    {
        b_CameraLockedToPlayer = b_IsLockedToPlayer_;
        f_CamMoveTimer = 0.0f;
    }
}