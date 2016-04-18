using UnityEngine;
using System.Collections;

public class Cs_GridLogic : MonoBehaviour
{
    int sizeOfGrid = 5;
    Object[] gridList;

    bool b_Test = true;

    int i_CurrNumTowers;
    int i_CurrSideLength = 1;

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

    public void IncrementNumberOfTowers(bool b_Incremented = true)
    {
        if (b_Incremented) ++i_CurrNumTowers; else --i_CurrNumTowers;

        print("Num Towers: " + i_CurrNumTowers + ", length: " + i_CurrSideLength);

        if((float)i_CurrNumTowers / (float)(i_CurrSideLength * i_CurrSideLength) > 0.75f)
        {
            IncrementWidthOfTowerArray();
        }
    }

    void IncrementWidthOfTowerArray()
    {
        // i_CurrSideLength += 2;
        ++i_CurrSideLength;

        // Find the X/Y pos of the center
        int i_Center = sizeOfGrid / 2;

        // yPos = center - i_CurrSideLength
        int y_ = i_Center - i_CurrSideLength;

        // x runs from center - currsidelength through center + currsidelength.
        for(int x = y_ - i_CurrSideLength; x < y_ + i_CurrSideLength; ++x)
        {
            var tower = gridList[(y_ * sizeOfGrid) + x] as GameObject;

            tower.GetComponent<Cs_GridObjectLogic>().SetGridObjectState(true);
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
