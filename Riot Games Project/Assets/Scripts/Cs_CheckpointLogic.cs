using UnityEngine;
using System.Collections;

public class Cs_CheckpointLogic : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        // Makes the checkpoints invisible
        gameObject.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}