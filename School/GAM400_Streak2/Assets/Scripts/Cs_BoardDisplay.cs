using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cs_BoardDisplay : MonoBehaviour
{
    // The virtual reference of what GridBoardLogic uses. Read only.
    Enum_BlockType[,] BlockArray;
    // The visual blocks we add to, change and move to show objects on the screen.
    Enum_BlockType[,] DisplayArray;
    GameObject[,] DisplayArray_Blocks;

    GameObject go_GridBlock;
    GameObject go_GridWall;

    GameObject go_Block_A;
    GameObject go_Block_B;
    GameObject go_Empty;

    int i_Height;
    int i_Width;

    bool b_IsDone;

    // Use this for initialization
    void Start()
    {
        // Init_Board(10, 10);
    }

    void LoadResources()
    {
        go_GridBlock = Resources.Load("GridBlock") as GameObject;
        go_GridWall = Resources.Load("GridBlock_Edge") as GameObject;

        go_Block_A = Resources.Load("Block_X") as GameObject;
        go_Block_B = Resources.Load("Block_O") as GameObject;

        go_Empty = Resources.Load("Block_Empty") as GameObject;
    }

    // Create a row of Grid objects
    List<List<GameObject>> Grid_Columns = new List<List<GameObject>>();
    List<GameObject> Grid_Row = new List<GameObject>();
    // When called from BoardLogic, create the grid background
    public void Init_Board(int i_Width_, int i_Height_, int i_MidWall_ = -1)
    {
        i_Height = i_Height_;
        i_Width = i_Width_;

        LoadResources();

        #region Create Grid & GridWalls
        if (i_MidWall_ > 1 && i_MidWall_ < i_Width_ - 2)
        {

        }
        else // No midwall
        {
            for (int y_ = 0; y_ < i_Height; ++y_)
            {
                GameObject go_WallTemp;

                Grid_Row = new List<GameObject>();

                for (int x_ = 0; x_ < i_Width_; ++x_)
                {
                    // Add a GridWall
                    if (x_ == 0 || x_ == i_Width_ - 1)
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
        #endregion

        #region Populate 'Block Array' & 'Display Array' as empty

        BlockArray = new Enum_BlockType[i_Height, i_Width];
        DisplayArray = new Enum_BlockType[i_Height, i_Width];
        DisplayArray_Blocks = new GameObject[i_Height, i_Width];

        for (int y_ = 0; y_ < i_Height; ++y_)
        {
            for (int x_ = 0; x_ < i_Width; ++x_)
            {
                BlockArray[y_, x_] = Enum_BlockType.Empty;
                DisplayArray[y_, x_] = Enum_BlockType.Empty;

                DisplayArray_Blocks[y_, x_] = Instantiate(go_Empty);

                // Set parent (for Editor clutter)
                DisplayArray_Blocks[y_, x_].transform.SetParent(GameObject.Find("EmptyBlocks").transform);
            }
        }

        #endregion
    }

    // Update is called once per frame
    int i_X;
    int i_Y;
    int i_CurrMax;
    float f_InitBoardTimer;
    float f_InitBoardTimer_Max = 0.1f;
    void Update()
    {
        // print(DisplayArray[4, 8].ToString());

        if (!b_IsDone)
        {
            CascadeTiles();
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

    // WIP
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

    public void MoveBlock_Dir( Enum_Direction e_MoveDir_, IntVector2 iv2_Pos_ )
    {
        if( e_MoveDir_ == Enum_Direction.Left )
        {
            // Push indicated block to the left
            if (DisplayArray_Blocks[iv2_Pos_.y, iv2_Pos_.x].GetComponent<Cs_BlockOnBoardLogic>())
            {
                DisplayArray_Blocks[iv2_Pos_.y, iv2_Pos_.x].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveLeft();
            }

            // Store block to left of this block
            Enum_BlockType e_TempBlock = DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 1];

            // Swap Blocks in Array
            DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 1] = DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x + 0];
            DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x + 0] = e_TempBlock;

            // Swap DisplayArray_Blocks in same manner
            GameObject go_TempBlock = DisplayArray_Blocks[iv2_Pos_.y, iv2_Pos_.x - 1];

            DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 1] = DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x + 0];
            DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x + 0] = go_TempBlock;
        }
        else if( e_MoveDir_ == Enum_Direction.Right )
        {
            // Push indicated block to the right
            if (DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x + 0].GetComponent<Cs_BlockOnBoardLogic>())
            {
                DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveRight();
            }

            // Store block to the right of this block
            Enum_BlockType e_TempBlock = DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x + 1];

            // Swap Blocks in Array
            DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x + 1] = DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 0];
            DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 0] = e_TempBlock;

            // Swap DisplayArray_Blocks in same manner
            GameObject go_TempBlock = DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x + 1];

            DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x + 1] = DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0];
            DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0] = go_TempBlock;
        }
        else if( e_MoveDir_ == Enum_Direction.Down )
        {
            // Push indicated block to the left
            if(DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0].GetComponent<Cs_BlockOnBoardLogic>())
            {
                DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveDown();
            }
            
            // Store block to the right of this block
            Enum_BlockType e_TempBlock = DisplayArray[iv2_Pos_.y - 1, iv2_Pos_.x + 0];

            // Swap Blocks in Array
            DisplayArray[iv2_Pos_.y - 1, iv2_Pos_.x + 0] = DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 0];
            DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 0] = e_TempBlock;

            // Swap DisplayArray_Blocks in same manner
            GameObject go_TempBlock = DisplayArray_Blocks[iv2_Pos_.y - 1, iv2_Pos_.x + 0];

            DisplayArray_Blocks[iv2_Pos_.y - 1, iv2_Pos_.x + 0] = DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0];
            DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0] = go_TempBlock;
        }
        else if(e_MoveDir_ == Enum_Direction.Up )
        {
            // Push indicated block up
            if (DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x + 0].GetComponent<Cs_BlockOnBoardLogic>())
            {
                DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveUp();
            }

            // Store block above this block
            Enum_BlockType e_TempBlock = DisplayArray[iv2_Pos_.y + 1, iv2_Pos_.x + 0];

            // Swap Blocks in Array
            DisplayArray[iv2_Pos_.y + 1, iv2_Pos_.x + 0] = DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 0];
            DisplayArray[iv2_Pos_.y + 0, iv2_Pos_.x - 0] = e_TempBlock;

            // Swap DisplayArray_Blocks in same manner
            GameObject go_TempBlock = DisplayArray_Blocks[iv2_Pos_.y + 1, iv2_Pos_.x + 0];

            DisplayArray_Blocks[iv2_Pos_.y + 1, iv2_Pos_.x + 0] = DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0];
            DisplayArray_Blocks[iv2_Pos_.y + 0, iv2_Pos_.x - 0] = go_TempBlock;
        }
    }

    public void ShiftBlocks( Enum_Direction e_MoveDir_, Enum_BlockSize e_BlockSize_, IntVector2 iv2_BottomLeft_)
    {
        if( e_MoveDir_ == Enum_Direction.Left )
        {
            #region Shift Left
            // Shift original 2x2 left
            MoveBlock_Dir(Enum_Direction.Left, iv2_BottomLeft_);
            MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 0));
            MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 0, iv2_BottomLeft_.y + 1));
            MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 1));

            // 3 wide
            if(e_BlockSize_ == Enum_BlockSize.size_3w_2h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 0));
                MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 1));
            }

            // 3 high
            if (e_BlockSize_ == Enum_BlockSize.size_2w_3h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 0, iv2_BottomLeft_.y + 2));
                MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 2));
            }

            // 3x3
            if(e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Left, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 2));
            }
            #endregion
        }
        else if( e_MoveDir_ == Enum_Direction.Right )
        {
            #region Shift Right
            // Order is jumbled to allow blocks to not overwrite one-another

            // 3x3
            if (e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 2));
            }

            // 3 wide
            if (e_BlockSize_ == Enum_BlockSize.size_3w_2h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 0));
                MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 1));
            }

            // 3 high
            if (e_BlockSize_ == Enum_BlockSize.size_2w_3h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 2));
                MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 0, iv2_BottomLeft_.y + 2));
            }

            // Shift original 2x2 left
            MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 0));
            MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 1));
            MoveBlock_Dir(Enum_Direction.Right, iv2_BottomLeft_);
            MoveBlock_Dir(Enum_Direction.Right, new IntVector2(iv2_BottomLeft_.x + 0, iv2_BottomLeft_.y + 1));
            #endregion
        }
        else if( e_MoveDir_ == Enum_Direction.Down )
        {
            #region Shift Down
            // Shift original 2x2 left
            MoveBlock_Dir(Enum_Direction.Down, iv2_BottomLeft_);
            MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 0));
            MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 0, iv2_BottomLeft_.y + 1));
            MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 1));

            // 3 wide
            if (e_BlockSize_ == Enum_BlockSize.size_3w_2h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 0));
                MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 1));
            }

            // 3 high
            if (e_BlockSize_ == Enum_BlockSize.size_2w_3h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 0, iv2_BottomLeft_.y + 2));
                MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 1, iv2_BottomLeft_.y + 2));
            }

            // 3x3
            if (e_BlockSize_ == Enum_BlockSize.size_3w_3h)
            {
                MoveBlock_Dir(Enum_Direction.Down, new IntVector2(iv2_BottomLeft_.x + 2, iv2_BottomLeft_.y + 2));
            }
            #endregion
        }
    }
    public void ShiftBlocks( Enum_Direction e_MoveDir_, Enum_BlockSize e_BlockSize_, Vector2 v2_BottomLeft_ )
    {
        IntVector2 iv2_BottomLeft = new IntVector2( (int)v2_BottomLeft_.x, (int)v2_BottomLeft_.y );

        ShiftBlocks(e_MoveDir_, e_BlockSize_, iv2_BottomLeft);
    }

    public void RotateBlocks(Enum_Direction e_RotDir_, Enum_BlockSize e_BlockSize_, IntVector2 iv2_BottomLeft_)
    {
        // Store Bottom Left
        Enum_BlockType e_BotLeftBlock = DisplayArray[iv2_BottomLeft_.y, iv2_BottomLeft_.x];
        GameObject go_BotLeftBlock = DisplayArray_Blocks[iv2_BottomLeft_.y, iv2_BottomLeft_.x];

        if (e_RotDir_ == Enum_Direction.Left)
        {
            #region Shift Left (Counter-clockwise)
            // Move Blocks Visually
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveDown();
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveLeft();
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveUp();
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveRight();

            // Re-arrange the blocks in the DisplayArray_Blocks to match
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 0] = DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0];
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0] = DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1];
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1] = DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1];
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1] = go_BotLeftBlock;

            // Re-arrange the blocks in the Display Array to match
            DisplayArray[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 0] = DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0];
            DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0] = DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1];
            DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1] = DisplayArray[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1];
            DisplayArray[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1] = e_BotLeftBlock;
            #endregion
        }
        else if( e_RotDir_ == Enum_Direction.Right )
        {
            #region Shift Right (Clockwise)
            // Move Blocks Visually
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveUp();
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveRight();
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveDown();
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1].GetComponent<Cs_BlockOnBoardLogic>().Set_MoveLeft();

            // Re-arrange the blocks in the DisplayArray_Blocks to match
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 0] = DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1];
            DisplayArray_Blocks[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1] = DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1];
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1] = DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0];
            DisplayArray_Blocks[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0] = go_BotLeftBlock;

            // Re-arrange the blocks in the Display Array to match
            DisplayArray[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 0] = DisplayArray[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1];
            DisplayArray[iv2_BottomLeft_.y + 0, iv2_BottomLeft_.x + 1] = DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1];
            DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 1] = DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0];
            DisplayArray[iv2_BottomLeft_.y + 1, iv2_BottomLeft_.x + 0] = e_BotLeftBlock;
            #endregion
        }
    }
    public void RotateBlocks(Enum_Direction e_RotDir_, Enum_BlockSize e_BlockSize_, Vector2 v2_BottomLeft_)
    {
        IntVector2 iv2_Temp = new IntVector2((int)v2_BottomLeft_.x, (int)v2_BottomLeft_.y);
        RotateBlocks(e_RotDir_, e_BlockSize_, iv2_Temp);
    }

    void DestroyBlockAt( IntVector2 iv2_DestroyLoc_ )
    {

    }

    public void Set_BoardState(Enum_BlockType[,] BlockArray_ )
    {
        BlockArray = BlockArray_;
    }

    public void Set_NewBlocks( Enum_BlockType[,] e_NewBlockTypeArray_, Enum_BlockSize e_BlockSize_, IntVector2 iv2_BottomLeft_ )
    {
        // Find Height/Width of block array
        int i_BlockWidth = 2;
        int i_BlockHeight = 2;

        // 3 Wide
        if (e_BlockSize_ == Enum_BlockSize.size_3w_2h || e_BlockSize_ == Enum_BlockSize.size_3w_2h) i_BlockWidth = 3;
        // 3 High
        if (e_BlockSize_ == Enum_BlockSize.size_2w_3h || e_BlockSize_ == Enum_BlockSize.size_3w_3h) i_BlockHeight = 3;
        
        // Run through the list of NewBlockTypeArray
        for(int y_ = 0; y_ < i_BlockHeight; ++y_)
        {
            for (int x_ = 0; x_ < i_BlockWidth; ++x_)
            {
                GameObject go_BlockTemp;

                // Create a new block based on the position within NewBlockTypeArray
                if (e_NewBlockTypeArray_[y_, x_] == Enum_BlockType.Block_1_Active)
                {
                    // go_BlockTemp is Instantiated as above by default
                    go_BlockTemp = Instantiate(go_Block_A);
                    go_BlockTemp.transform.SetParent(GameObject.Find("DisplayBlockList").transform);
                }
                else if (e_NewBlockTypeArray_[y_, x_] == Enum_BlockType.Block_2_Active)
                {
                    go_BlockTemp = Instantiate(go_Block_B);
                    go_BlockTemp.transform.SetParent(GameObject.Find("DisplayBlockList").transform);
                }
                else if (e_NewBlockTypeArray_[y_, x_] == Enum_BlockType.Block_3_Active)
                {
                    print("TODO: ADD BLOCK 3 TO CS_BOARDDISPLAY");
                    go_BlockTemp = Instantiate(go_Empty);
                    go_BlockTemp.transform.SetParent(GameObject.Find("EmptyBlocks").transform);
                }
                else
                {
                    print("INVALID BLOCK ADDED TO CS_BOARDDISPLAY");
                    go_BlockTemp = Instantiate(go_Empty);
                    go_BlockTemp.transform.SetParent(GameObject.Find("EmptyBlocks").transform);
                }

                // Initialize this block
                int i_NewX = x_ + iv2_BottomLeft_.x;
                int i_NewY = y_ + iv2_BottomLeft_.y;
                print("Possss: " + i_NewX + ", " + i_NewY);
                if(go_BlockTemp.GetComponent<Cs_BlockOnBoardLogic>())
                {

                    go_BlockTemp.GetComponent<Cs_BlockOnBoardLogic>().Init_BlockModel(i_NewX, i_NewY, 3.0f, i_Width);
                }

                // Add this block type to the proper position within Display Array
                DisplayArray[i_NewY, i_NewX] = e_NewBlockTypeArray_[y_, x_];

                DisplayArray_Blocks[i_NewY, i_NewX] = go_BlockTemp;
            }
        }
    }

    void PrintArrayToConsole()
    {
        // The 'y' is reversed (top to bottom) to compensate for printing
        for(int j = i_Height - 1; j >= 0 ; j--)
        {
            string tempString = "";

            for(int i = 0; i < i_Width; ++i)
            {
                // Left & Right 'Walls'
                if((i == 0 || i == i_Width - 1) && (DisplayArray[j, i] == Enum_BlockType.Empty))
                {
                    tempString += "{!!} ";
                }
                // Normal empty block position
                else if (DisplayArray[j, i] == Enum_BlockType.Empty) tempString += "[__] ";
                // Print a populated block
                else tempString += "[" + (int)BlockArray[j, i] + "] ";
            }
            print(tempString);
        }
        // print("Active Block: " + v2_ActiveBlockLocation + "\n-----------------------------------------------------------------\n");
    }
}
