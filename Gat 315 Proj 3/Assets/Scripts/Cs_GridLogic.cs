using UnityEngine;
using System.Collections;

public class Cs_GridLogic : MonoBehaviour
{
    int sizeOfGrid = 5;
    Object[] gridList;

    bool b_Test = true;

    // Use this for initialization
    void Start()
    {
        gridList = new GameObject[sizeOfGrid * sizeOfGrid];

        PreloadGridObjects();
    }

    void PreloadGridObjects()
    {
        int midPoint = sizeOfGrid / 2;

        for (int y = 0; y < sizeOfGrid; ++y)
        {
            for (int x = 0; x < sizeOfGrid; ++x)
            {
                Vector3 pos = new Vector3(x - (sizeOfGrid / 2), 0, y - (sizeOfGrid / 2));
                Quaternion quat = new Quaternion();

                GameObject temp = Instantiate(Resources.Load("GridObject", typeof(GameObject)), pos, quat) as GameObject;

                // Only activate the middle points
                if (y >= (midPoint - 1) && y <= (midPoint + 1) && x >= (midPoint - 1) && x <= (midPoint + 1))
                {
                    temp.GetComponent<Cs_GridObjectLogic>().SetGridObjectState(true);
                }
                else
                {
                    temp.GetComponent<Cs_GridObjectLogic>().SetGridObjectState(false);
                }

                gridList[(y * sizeOfGrid) + x] = temp;
            }
        }
    }

    public void CheckGridForNewArray()
    {
        // Store the center X & Y position
        int centerXY = sizeOfGrid / 2; // Yes, 5/2 = 2. But 0 through 4 = 2, so it works for now.
        bool b_LeftWallChecked = false;
        bool b_RighttWallChecked = false;
        bool b_TopWallChecked = false;
        bool b_BottomWallChecked = false;

        // Check left wall for appropriate circumstances.
        for (int x = 0; x < sizeOfGrid / 2; ++x)
        {
            // Run from left toward the center, searching for the first active
            GameObject tempObj = gridList[(centerXY * sizeOfGrid) + x] as GameObject;
            if (tempObj.GetComponent<Cs_GridObjectLogic>().GetGridObjectState() != GridObjectState.Off)
            {
                // Run through the center X coordinate from left to 'grid center' for the first non-'Off' GridObject.
                int centerY = ((centerXY * sizeOfGrid) + x) % sizeOfGrid;

                for(int y = 0; y < sizeOfGrid; ++y)
                {
                    GameObject newTempObj = gridList[(y * sizeOfGrid) + x] as GameObject;

                    // Run through that new Y position in the array. If we find any 'On' GridObject, continue. (Means it is empty)
                    if(newTempObj.GetComponent<Cs_GridObjectLogic>().GetGridObjectState() == GridObjectState.On)
                    {
                        b_LeftWallChecked = true;

                        // Kick out because this column wont get anything new
                        continue;
                    }


                }
                // Continues kicking out of the for-loop (We don't want to continue moving inward, since those columns are already checked)
                if (b_LeftWallChecked) continue;

                // We've determined all spots in this column either are 'off' or 'Active'. Set the left column to active (if possible)
                if(centerY > 0)
                {
                    /*
                    for(int y = 0; )
                    GameObject newTempObj = gridList[]
                    if()
                    */
                }

                // Loop again. If a gridobject spot is 'Active', then make it's y-1 position On.
                // We've finished this column. Since we made a change to the array, return (kick out)

            }
            // Otherwise, all of the GridObjects are either Off or Active, then we can expand outward (y - 1)
        }
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            b_Test = !b_Test;

            for (int i = 0; i < gridList.Length; ++i)
            {
                var tower = gridList[i] as GameObject;

                tower.GetComponent<Cs_GridObjectLogic>().SetGridObjectState(b_Test);
            }
        }
#endif

    }
}
