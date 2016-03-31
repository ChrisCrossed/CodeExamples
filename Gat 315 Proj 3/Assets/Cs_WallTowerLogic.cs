using UnityEngine;
using System.Collections;

public class Cs_WallTowerLogic : Cs_DefaultBase
{

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P)) { SetNewMaterialColor(Colors.Blue); print("Pressed"); }
	}
}
