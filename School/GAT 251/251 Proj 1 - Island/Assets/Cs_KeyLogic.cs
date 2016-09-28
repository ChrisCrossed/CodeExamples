using UnityEngine;
using System.Collections;

public class Cs_KeyLogic : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    // Force Rotation
        
	}

    void OnTriggerEnter(Collider collision_)
    {
        print("Touching " + collision_.gameObject.name);
        if (collision_.gameObject.name == "Capsule")
        {
            Destroy(gameObject);
        }
    }
}
