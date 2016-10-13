using UnityEngine;
using System.Collections;

public class Cs_SkiingPlayerController : MonoBehaviour
{
    float f_Velocity;
    GameObject go_RaycastPoint;
    Vector3 v3_CurrentVector;
    bool b_GravityApplied;

    // Physics Materials
    PhysicMaterial physMat_Ski;
    PhysicMaterial physMat_Walk;

	// Use this for initialization
	void Start ()
    {
        go_RaycastPoint = transform.Find("RaycastPoint").gameObject;

        physMat_Ski  = (PhysicMaterial)Resources.Load("PhysMat_Ski");
        physMat_Walk = (PhysicMaterial)Resources.Load("PhysMat_Walk");
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region PlayerSliding
        Vector3 v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity;

        // If player is skiing
        if(Input.GetKey(KeyCode.Space))
        {
            // Set PhysicsMaterial
            gameObject.GetComponent<Collider>().material = physMat_Ski;

            // Raycast down and grab the angle of the terrain
            RaycastHit hit;
            if(Physics.Raycast(go_RaycastPoint.transform.position, -transform.up, out hit, 1.5f))
            {
                Vector3 v3_GroundVector = Vector3.ProjectOnPlane(gameObject.transform.forward, hit.normal);

                gameObject.GetComponent<Rigidbody>().AddForce(v3_GroundVector + v3_Velocity);
            }
        }
        // If player is not skiing
        else
        {
            // Set PhysicsMaterial
            gameObject.GetComponent<Collider>().material = physMat_Walk;
        }

        if(Input.GetKey(KeyCode.W))
        {
            if(gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 10)
            {
                f_Velocity += Time.deltaTime;

                v3_Velocity = gameObject.GetComponent<Rigidbody>().velocity + (gameObject.transform.forward * f_Velocity);

                gameObject.GetComponent<Rigidbody>().velocity = v3_Velocity;

                print((gameObject.transform.forward * Time.deltaTime * 5));
            }
        }

        // Vector3 v3_CurrPos = gameObject.transform.position;
        //gameObject.transform.position = hit.point + (gameObject.transform.up * 1);

        // Apply a forward vector and push the player in that direction
        // gameObject.GetComponent<Rigidbody>().velocity = v3_GroundVector * 10;
        #endregion
    }

    /*
    void OnCollisionEnter(Collision collision_)
    {
        if(collision_.gameObject.tag == "Ground")
        {
            b_GravityApplied = false;
        }
    }

    void OnCollisionExit(Collision collision_)
    {
        if (collision_.gameObject.tag == "Ground")
        {
            b_GravityApplied = true;
        }
    }
    */
}
