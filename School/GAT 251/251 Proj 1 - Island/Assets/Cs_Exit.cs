using UnityEngine;
using System.Collections;

public class Cs_Exit : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnTriggerEnter(Collider collision_)
    {
        print("Touching " + collision_.gameObject.name);
        if(collision_.gameObject.name == "Capsule")
        {
            GameObject.Find("Player").GetComponent<Cs_FPSController>().FadeToBlack();
        }
    }
}
