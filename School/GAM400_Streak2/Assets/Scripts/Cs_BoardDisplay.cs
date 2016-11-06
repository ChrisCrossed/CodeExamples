using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cs_BoardDisplay : MonoBehaviour
{
    GameObject go_GridBlock;
    GameObject go_GridWall;

    int i_Height;
    int i_Width;

    bool b_IsDone;

	// Use this for initialization
	void Start ()
    {
        go_GridBlock = Resources.Load("GridBlock")      as GameObject;
        go_GridWall  = Resources.Load("GridBlock_Edge") as GameObject;

        Init_Board(20, 10);
	}

    // Create a row of Grid objects
    List<List<GameObject>>  Grid_Columns = new List<List<GameObject>>();
    List<GameObject>        Grid_Row = new List<GameObject>();
    // When called from BoardLogic, create the grid background
    void Init_Board( int i_Width_, int i_Height_, int i_MidWall_ = -1 )
    {
        i_Height = i_Height_;
        i_Width = i_Width_;
        
        if(i_MidWall_ > 1 && i_MidWall_ < i_Width_ - 2)
        {

        }
        else // No midwall
        {
            for(int y_ = 0; y_ < i_Height; ++y_)
            {
                GameObject go_WallTemp;

                Grid_Row = new List<GameObject>();

                for(int x_ = 0; x_ < i_Width_; ++x_)
                {
                    // Add a GridWall
                    if( x_ == 0 || x_ == i_Width_ - 1)
                    {
                        // Instantiate the object
                        go_WallTemp = Instantiate(go_GridWall);
                    }
                    else
                    {
                        // Instantiate the object
                        go_WallTemp = Instantiate(go_GridBlock);
                    }
                    Grid_Row.Add(go_WallTemp);
                
                    // Set position
                    go_WallTemp.transform.position = new Vector3(x_ * go_GridBlock.transform.lossyScale.x, y_ * go_GridBlock.transform.lossyScale.y, 0);
                    go_WallTemp.transform.parent = GameObject.Find("GridBlockList").transform;
                }
                // Add the current Row to the Column List
                Grid_Columns.Add(Grid_Row);
            }

            // Reset the Row
            Grid_Row = new List<GameObject>();
            // Set the position
        }
    }

    // Update is called once per frame
    int i_X;
    int i_Y;
    int i_CurrMax;
    float f_InitBoardTimer;
    float f_InitBoardTimer_Max = 0.1f;
	void Update ()
    {
        if(!b_IsDone)
        {
            CascadeTiles();
        }
        else
        {
            // CollapseTiles();
        }
    }

    void CascadeTiles()
    {
        f_InitBoardTimer += Time.deltaTime;

        if (f_InitBoardTimer >= f_InitBoardTimer_Max)
        {
            // Reset Timer
            f_InitBoardTimer = 0.0f;

            // Set new amounts
            // Cap X
            i_X = i_CurrMax;
            if (i_X > i_Width - 1) i_X = i_Width - 1;

            // Cap Y
            i_Y = i_CurrMax - i_X;
            if (i_Y > i_Height - 1) i_Y = i_Height - 1;

            Grid_Columns[i_Y][i_X].GetComponent<Cs_GridBlockLogic>().Set_FadeState(Enum_FadeState.FadeIn);

            // Run through incremental loop
            for (int i_ = 0; i_ < i_CurrMax; ++i_)
            {
                // As X decreases, Y increases.
                --i_X;
                i_Y = i_CurrMax - i_X;

                if (i_X < 0) i_X = 0;
                if (i_Y > i_Height - 1) i_Y = i_Height - 1;

                if (i_Y < i_Height && i_X < i_Width)
                {
                    Grid_Columns[i_Y][i_X].GetComponent<Cs_GridBlockLogic>().Set_FadeState(Enum_FadeState.FadeIn);
                }
            }

            if (i_CurrMax < i_Height + i_Width)
            {
                ++i_CurrMax;
                print(i_CurrMax);
            }
            else
            {
                b_IsDone = true;

                // i_X = 0;
                i_Y = 0;
                i_CurrMax = 0;
            }
        }
    }

    void CollapseTiles()
    {
        f_InitBoardTimer += Time.deltaTime;

        if (f_InitBoardTimer >= f_InitBoardTimer_Max)
        {
            // Reset Timer
            f_InitBoardTimer = 0.0f;

            // Set new amounts
            // Cap X
            i_X = i_CurrMax;
            if (i_X > i_Width - 1) i_X = i_Width - 1;

            // Cap Y
            i_Y = i_CurrMax - i_X;
            if (i_Y > i_Height - 1) i_Y = i_Height - 1;

            Grid_Columns[i_Y][i_X].GetComponent<Cs_GridBlockLogic>().Set_FadeState(Enum_FadeState.FadeOut);

            // Run through incremental loop
            for (int i_ = 0; i_ < i_CurrMax; ++i_)
            {
                // As X decreases, Y increases.
                --i_X;
                i_Y = i_CurrMax - i_X;

                if (i_X < 0) i_X = 0;
                if (i_Y > i_Height - 1) i_Y = i_Height - 1;

                if (i_Y < i_Height && i_X < i_Width)
                {
                    Grid_Columns[i_Y][i_X].GetComponent<Cs_GridBlockLogic>().Set_FadeState(Enum_FadeState.FadeIn);
                }
            }

            if (i_CurrMax < i_Height + i_Width)
            {
                ++i_CurrMax;
                print(i_CurrMax);
            }
            else
            {
                b_IsDone = false;

                // i_X = 0;
                i_Y = 0;
                i_CurrMax = 0;
            }
        }
    }
}
