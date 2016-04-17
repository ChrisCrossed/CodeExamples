using UnityEngine;
using System.Collections;

public class Cs_GridLogic : MonoBehaviour
{
    Object[] gridList = new Object[25];

	// Use this for initialization
	void Start ()
    {
        PreloadGridObjects();
	}

    void PreloadGridObjects()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion quat = new Quaternion();

        gridList[0] = Instantiate(Resources.Load("GridObject", typeof(GameObject)), pos, quat) as GameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
