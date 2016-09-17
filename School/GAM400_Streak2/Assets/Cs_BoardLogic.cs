using UnityEngine;
using System.Collections;

public enum Enum_BlockType
{
    Block_1_Static = 10,
    Block_1_Active = 15,
    Block_2_Static = 20,
    Block_2_Active = 25,
    Block_3_Static = 30,
    Block_3_Active = 35,
    Empty = 00
}
public enum Enum_BlockSize
{
    size_2x2,
    size_2x3,
    size_3x2,
    size_3x3
}

public class Cs_BoardLogic : MonoBehaviour
{
    public int i_ArrayWidth;
    public int i_ArrayHeight;

    Enum_BlockType[,] BlockArray;

	// Use this for initialization
	void Start ()
    {
        BlockArray = new Enum_BlockType[i_ArrayHeight, i_ArrayWidth];

        Initialize_BlockArray();

        SetBlock(1, 1, Enum_BlockType.Block_2_Static);
        SetBlock(2, 1, Enum_BlockType.Block_1_Static);
        SetBlock(1, 2, Enum_BlockType.Block_2_Static);
        SetBlock(2, 2, Enum_BlockType.Block_1_Static);

        PrintArrayToConsole();
    }

    void Initialize_BlockArray()
    {
        for(int y = 0; y < i_ArrayHeight; ++y)
        {
            for (int x = 0; x < i_ArrayWidth; ++x)
            {
                SetBlock(x, y, Enum_BlockType.Empty);
            }
        }
    }

    void PrintArrayToConsole()
    {
        // The 'y' is reversed (top to bottom) to compensate for printing
        for(int j = i_ArrayHeight - 1; j >= 0 ; j--)
        {
            string tempString = "";

            for(int i = 0; i < i_ArrayWidth; ++i)
            {
                if (BlockArray[j, i] == Enum_BlockType.Empty) tempString += "[00] ";
                else tempString += "[" + (int)BlockArray[j, i] + "] ";
            }
            print(tempString);
        }
        print("\n-----------------------------------------------------------------\n");
    }

    #region Block Position Manipulation
    void MoveActiveBlocks_Down(Vector2 v2_BottomLeft, Enum_BlockSize BlockSize_)
    {
        if (v2_BottomLeft.y - 1 < 0) return;

        #region Default 2x2
        // (0,0) -> (0, -1)
        SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y - 1, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y));

        // (1, 0) -> (0, -1)
        SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y - 1, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y));

        // (0, 1) -> (0, 0)
        SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1));

        // (1,1) -> (1, 0)
        SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1));

        if(BlockSize_ == Enum_BlockSize.size_2x2)
        {
            // (0,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);

            // (1,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
        }
        #endregion

        #region 3 wide by 2 tall
        if(BlockSize_ == Enum_BlockSize.size_3x2 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // (2, 0) -> (2, -1)
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y - 1, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y));

            // (2, 1) -> (2, 0)
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1));

            // If 3x2: (2,1) -> CLEAR
            if (BlockSize_ == Enum_BlockSize.size_3x2) SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
        }
        #endregion

        #region 2 wide by 3 tall
        if (BlockSize_ == Enum_BlockSize.size_2x3 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // (0,2) -> (0, 1) 
            SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 2));

            // (1,2) -> (1, 1)
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2));

            // (0,2) -> CLEAR
            SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);

            // (1,2) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);
        }
        #endregion

        #region 3 wide by 3 tall
        if (BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // (2,2) -> (2,1)
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2));

            // (2,2) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);
        }
        #endregion
    }

    void SetBlock(int x_Pos_, int y_Pos_, Enum_BlockType blockType_)
    {
        if (x_Pos_ < 0 || x_Pos_ >= i_ArrayWidth)  return;
        if (y_Pos_ < 0 || y_Pos_ >= i_ArrayHeight) return;

        BlockArray[y_Pos_, x_Pos_] = blockType_;
    }

    Enum_BlockType GetBlock(int x_Pos_, int y_Pos_)
    {
        return BlockArray[y_Pos_, x_Pos_];
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.O))
        {
            MoveActiveBlocks_Down(new Vector2( 1, 1 ), Enum_BlockSize.size_2x2);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            MoveActiveBlocks_Down(new Vector2(1, 0), Enum_BlockSize.size_2x2);

            PrintArrayToConsole();
        }
    }
}
