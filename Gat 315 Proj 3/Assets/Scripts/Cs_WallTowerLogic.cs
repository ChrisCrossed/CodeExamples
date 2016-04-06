using UnityEngine;
using System.Collections;

public class Cs_WallTowerLogic : Cs_DefaultBase
{
    int i_Counter;

	// Use this for initialization
	void Start ()
    {
        BoxCollider boxCollider = gameObject.transform.Find("Col_BaseCollider").GetComponent<BoxCollider>();
        CapsuleCollider radiusCollider = gameObject.transform.Find("Col_Radius").GetComponent<CapsuleCollider>();
        Initialize(10, 10, boxCollider, radiusCollider);
        SetMat
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ++i_Counter; print("Pressed");

            if (i_Counter > 5) i_Counter = 0;

            // YourEnum foo = (YourEnum)yourInt;
            Colors temp = (Colors)i_Counter;

            SetNewMaterialColor(temp);
        }
	}
}
