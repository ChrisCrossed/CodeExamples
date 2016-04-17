using UnityEngine;
using System.Collections;

public class Cs_GridLogic : MonoBehaviour
{
    int sizeOfGrid = 5;
    Object[] gridList;

	// Use this for initialization
	void Start ()
    {
        gridList = new GameObject[sizeOfGrid * sizeOfGrid];

        PreloadGridObjects();
	}

    void PreloadGridObjects()
    {
        for(int y = 0; y < sizeOfGrid; ++y)
        {
            for(int x = 0; x < sizeOfGrid; ++x)
            {
                Vector3 pos = new Vector3(x - (sizeOfGrid / 2), 0, y - (sizeOfGrid / 2));
                Quaternion quat = new Quaternion();

                gridList[(y * sizeOfGrid) + x] = Instantiate(Resources.Load("GridObject", typeof(GameObject)), pos, quat) as GameObject;

                if(y < sizeOfGrid / 2)
                {
                    // gridList[(y * sizeOfGrid) + y]
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
