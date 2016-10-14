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

    // Current Active Block Information
    Vector2 v2_ActiveBlockLocation;
    public Enum_BlockSize e_BlockSize = Enum_BlockSize.size_2x2;

	// Use this for initialization
	void Start ()
    {
        BlockArray = new Enum_BlockType[i_ArrayHeight, i_ArrayWidth];

        Initialize_BlockArray();

        // Default (2x2)
        SetBlock(1, 1, Enum_BlockType.Block_2_Active);
        SetBlock(2, 1, Enum_BlockType.Block_1_Active);
        SetBlock(1, 2, Enum_BlockType.Block_2_Active);
        SetBlock(2, 2, Enum_BlockType.Block_1_Active);

        // 3 wide
        // SetBlock(3, 1, Enum_BlockType.Block_1_Active);
        // SetBlock(3, 2, Enum_BlockType.Block_2_Active);

        // 3 high
        // SetBlock(1, 3, Enum_BlockType.Block_1_Active);
        // SetBlock(2, 3, Enum_BlockType.Block_3_Static);

        // (3x3)
        // SetBlock(3, 3, Enum_BlockType.Block_3_Active);

        v2_ActiveBlockLocation = new Vector2(1, 1);

        PrintArrayToConsole();
    }

    void CreateNewBlock()
    {
        // Find the 'X' location to set the block location
        if(e_BlockSize == Enum_BlockSize.size_2x2 || e_BlockSize == Enum_BlockSize.size_2x3)
        {
            // Finds the center of the List width
            v2_ActiveBlockLocation.x = (int)((i_ArrayWidth - 1) / 2);
        }
        else
        {
            // Finds the center of the List width, and shifts to the left one space
            v2_ActiveBlockLocation.x = (int)((i_ArrayWidth - 1) / 2) - 1;
        }

        // Find the 'Y' location to set the block location
        if (e_BlockSize == Enum_BlockSize.size_2x3 || e_BlockSize == Enum_BlockSize.size_3x3)
        {
            v2_ActiveBlockLocation.y = i_ArrayHeight - 3;
        }
        else v2_ActiveBlockLocation.y = i_ArrayHeight - 2;

        // Manually create a set of new blocks in the proper location
        SetBlock(v2_ActiveBlockLocation, Enum_BlockType.Block_3_Active);
        SetBlock(new Vector2(v2_ActiveBlockLocation.x + 1, v2_ActiveBlockLocation.y), Enum_BlockType.Block_3_Active);
        SetBlock(new Vector2(v2_ActiveBlockLocation.x + 1, v2_ActiveBlockLocation.y + 1), Enum_BlockType.Block_3_Active);
        SetBlock(new Vector2(v2_ActiveBlockLocation.x, v2_ActiveBlockLocation.y + 1), Enum_BlockType.Block_3_Active);
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
        print("Active Block: " + v2_ActiveBlockLocation + "\n-----------------------------------------------------------------\n");
    }

    #region Block Position Manipulation
    // Complete
    void MoveActiveBlocks_Down(Vector2 v2_BottomLeft, Enum_BlockSize BlockSize_)
    {
        #region Ensure appropriate spaces below are empty. Otherwise, convert all blocks to static.
        if (v2_BottomLeft.y - 1 < 0) { AllBlocksStatic(); return; }

        if (GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y - 1) != Enum_BlockType.Empty) { AllBlocksStatic(); return; }
        if (GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y - 1) != Enum_BlockType.Empty) { AllBlocksStatic(); return; }

        if (BlockSize_ == Enum_BlockSize.size_3x2 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            if (GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y - 1) != Enum_BlockType.Empty) { AllBlocksStatic(); return; }
        }
        #endregion

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
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y    , GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1));

            // If 3x2:
            if (BlockSize_ == Enum_BlockSize.size_3x2)
            {
                // (0,1) -> CLEAR
                SetBlock((int)v2_BottomLeft.x    , (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);

                // (1,1) -> CLEAR
                SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);

                // (2,1) -> CLEAR
                SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
            }
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

        // Move the CurrentBlockLocation 'y'
        --v2_ActiveBlockLocation.y;
    }

    // Complete
    void MoveActiveBlocks_Left(Vector2 v2_BottomLeft, Enum_BlockSize BlockSize_)
    {
        #region Ensure appropriate spaces below are empty. Otherwise, convert all blocks to static.
        if (v2_BottomLeft.x - 1 < 0) { AllBlocksStatic(); return; }

        // As for block repositioning, I do not want blocks to be pushed around if another block exists.
        if (GetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y) != Enum_BlockType.Empty) { return; }
        if (GetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 1) != Enum_BlockType.Empty) { return; }

        // Check in case the active blocks we have are 3 high
        if(BlockSize_ == Enum_BlockSize.size_2x3 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            if (GetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 2) != Enum_BlockType.Empty) { return; }
        }
        #endregion

        #region Default 2x2
        // (0,0) -> (-1, 0)
        SetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y));

        // (1, 0) -> (0, 0)
        SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y));

        // (0, 1) -> (-1, 1)
        SetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1));

        // (1, 1) -> (0, 1)
        SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1));

        if (BlockSize_ == Enum_BlockSize.size_2x2)
        {
            // (1,0) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y, Enum_BlockType.Empty);

            // (1,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
        }
        #endregion

        #region 3 wide by 2 tall
        if (BlockSize_ == Enum_BlockSize.size_3x2 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // (2, 0) -> (1, 0)
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y));

            // (2, 1) -> (1, 1)
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1));
            
            // (2,0) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y, Enum_BlockType.Empty);

            // (2,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
        }
        #endregion

        #region 2 wide by 3 tall
        if (BlockSize_ == Enum_BlockSize.size_2x3 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // (0,2) -> (-1, 2) 
            SetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 2));

            // (1,2) -> (0, 2)
            SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2));

            // If 2x3: 
            if(BlockSize_ == Enum_BlockSize.size_2x3)
            {
                // (1,0) -> CLEAR
                SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y, Enum_BlockType.Empty);

                // (1,1) -> CLEAR
                SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);

                // (1,2) -> CLEAR
                SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);
            }
        }
        #endregion

        #region 3 wide by 3 tall
        if (BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // (2,2) -> (1,2)
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2));

            // (2,2) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);
        }
        #endregion

        --v2_ActiveBlockLocation.x;
    }

    // TODO: Fix & Complete
    void MoveActiveBlocks_Right(Vector2 v2_BottomLeft, Enum_BlockSize BlockSize_)
    {
        // Check two spaces over from the bottom left block
        // Return check. Performs no operation if we're outside the size of the grid
        if (BlockSize_ == Enum_BlockSize.size_2x2 || BlockSize_ == Enum_BlockSize.size_2x3)
        {
            // If the position to the right of the block doesn't exist, quit out.
            if (v2_BottomLeft.x + 2 > i_ArrayWidth) { return; }
            // if (v2_BottomLeft.x + 2 > ) { return; }


            // If the position to the right of the block isn't empty, quit out
            if (GetBlock( (int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y    ) != Enum_BlockType.Empty) { return; }
            if (GetBlock( (int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1) != Enum_BlockType.Empty) { return; }
        }
        else // Block is 3 wide, not 2
        {
            // If the position to the right of the block doesn't exist, quit out.
            if (v2_BottomLeft.x + 3 >= i_ArrayWidth) { return; }

            // If the position to the right of the block isn't empty, quit out
            if (GetBlock((int)v2_BottomLeft.x + 3, (int)v2_BottomLeft.y) != Enum_BlockType.Empty) { return; }
            if (GetBlock((int)v2_BottomLeft.x + 3, (int)v2_BottomLeft.y + 1) != Enum_BlockType.Empty) { return; }
        }

        #region Run the check from the top right, backward
        if(BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // 2, 2 -> 3, 2
            SetBlock((int)v2_BottomLeft.x + 3, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2));
        }

        if(BlockSize_ == Enum_BlockSize.size_2x3 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // 1, 2 -> 2, 2
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2));

            // 0, 2 -> 1, 2
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 2));

            // 0, 2 -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);
        }

        if (BlockSize_ == Enum_BlockSize.size_3x2 || BlockSize_ == Enum_BlockSize.size_3x3)
        {
            // 2, 1 -> 3, 1
            SetBlock((int)v2_BottomLeft.x + 3, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1));

            // 2, 0 -> 3, 0
            SetBlock((int)v2_BottomLeft.x + 3, (int)v2_BottomLeft.y + 0, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 0));
        }

        // Default (2x2 Block) - Remember, reverse order. Right to Left.
        // 1, 1 -> 2, 1
        SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1));

        // 0, 1 -> 1, 1
        SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 1));

        // 0, 1 -> CLEAR
        SetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);

        // 1, 0 -> 2, 0
        SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 0, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 0));

        // 0, 0 -> 1, 0
        SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 0, GetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 0));

        // 0, 0 -> CLEAR
        SetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 0, Enum_BlockType.Empty);

        #endregion

        ++v2_ActiveBlockLocation.x;
    }

    // TODO: Start
    void RotateBlocks_Clockwise(Vector2 v2_BottomLeft, Enum_BlockSize BlockSize_)
    {

    }

    // TODO: Start
    void RotateBlocks_CounterClock(Vector2 v2_BottomLeft, Enum_BlockSize BlockSize_)
    {

    }

    void SetBlock(int x_Pos_, int y_Pos_, Enum_BlockType blockType_)
    {
        if (x_Pos_ < 0 || x_Pos_ >= i_ArrayWidth)  return;
        if (y_Pos_ < 0 || y_Pos_ >= i_ArrayHeight) return;

        BlockArray[y_Pos_, x_Pos_] = blockType_;
    }
    
    void SetBlock(Vector2 blockPos_, Enum_BlockType blockType_)
    {
        SetBlock((int)blockPos_.x, (int)blockPos_.y, blockType_);
    }

    void AllBlocksStatic()
    {
        #region Convert All to Static
        // Run through the array and convert all blocks into their static counterpart. Run bottom to top, left to right.
        for (int x_ = 0; x_ < i_ArrayWidth; ++x_)
        {
            for (int y_ = 0; y_ < i_ArrayHeight; ++y_)
            {
                Enum_BlockType tempBlock = GetBlock(x_, y_);

                if (tempBlock == Enum_BlockType.Block_1_Active)
                {
                    SetBlock(x_, y_, Enum_BlockType.Block_1_Static);
                }
                else if (tempBlock == Enum_BlockType.Block_2_Active)
                {
                    SetBlock(x_, y_, Enum_BlockType.Block_2_Static);
                }
                else if (tempBlock == Enum_BlockType.Block_3_Active)
                {
                    SetBlock(x_, y_, Enum_BlockType.Block_3_Static);
                }
            }
        }
        #endregion

        #region Pull Blocks Down
        // Run through the array and pull blocks down to their lowest point
        for (int x_ = 0; x_ < i_ArrayWidth; ++x_)
        {
            // The y begins at 1 since we can't move a block down at y = 0
            for (int y_ = 1; y_ < i_ArrayHeight; ++y_)
            {
                // Get the current block type
                Enum_BlockType thisBlock = GetBlock(x_, y_);

                // Get the current block type beneath us
                Enum_BlockType lowerBlock = GetBlock(x_, y_ - 1);

                if(thisBlock != Enum_BlockType.Empty && lowerBlock == Enum_BlockType.Empty)
                {
                    // If a static block is found with an open spot beneath it, shift it down one spot
                    SetBlock(x_, y_ - 1, thisBlock);

                    // Set the previous block position to empty
                    SetBlock(x_, y_, Enum_BlockType.Empty);

                    // Reset and re-loop
                    y_ = 0;

                    continue;
                }
            }
        }
        #endregion

        CreateNewBlock();
    }

    Enum_BlockType GetBlock(int x_Pos_, int y_Pos_)
    {
        return BlockArray[y_Pos_, x_Pos_];
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.S))
        {
            MoveActiveBlocks_Down(v2_ActiveBlockLocation, Enum_BlockSize.size_3x3);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveActiveBlocks_Left(v2_ActiveBlockLocation, Enum_BlockSize.size_3x3);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveActiveBlocks_Right(v2_ActiveBlockLocation, Enum_BlockSize.size_2x2);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AllBlocksStatic();

            PrintArrayToConsole();
        }
    }
}
