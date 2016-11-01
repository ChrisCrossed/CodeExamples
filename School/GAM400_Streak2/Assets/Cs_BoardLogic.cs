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
    size_2w_2h,
    size_2w_3h,
    size_3w_2h,
    size_3w_3h
}

public class Cs_BoardLogic : MonoBehaviour
{
    public int i_ArrayWidth;
    public int i_ArrayHeight;

    Enum_BlockType[,] BlockArray;

    // Current Active Block Information
    Vector2 v2_ActiveBlockLocation;
    Enum_BlockSize e_BlockSize;
    [SerializeField] bool b_2w_2h_Allowed = true;
    [SerializeField] bool b_2w_3h_Allowed = false;
    [SerializeField] bool b_3w_2h_Allowed = false;
    [SerializeField] bool b_3w_3h_Allowed = false;
    [SerializeField] bool b_ThreeBlockColors = false;

    Enum_BlockType[] e_NextBlockList = new Enum_BlockType[16];

	// Use this for initialization
	void Start ()
    {
        #region Set First Block to Random Size
        // Determine the size of the next block to use
        bool b_FoundNextBlock = false;
        // While we haven't found the next block, loop
        while (!b_FoundNextBlock)
        {
            int i_RandBlock = Random.Range(0, 4);

            if (i_RandBlock == 0 && b_2w_2h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_2w_2h;
                b_FoundNextBlock = true;
            }
            else if (i_RandBlock == 1 && b_2w_3h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_2w_3h;
                b_FoundNextBlock = true;
            }
            else if (i_RandBlock == 2 && b_3w_2h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_3w_2h;
                b_FoundNextBlock = true;
            }
            else if (i_RandBlock == 3 && b_3w_3h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_3w_3h;
                b_FoundNextBlock = true;
            }
        }
        #endregion

        // Initialize NextBlockList
        for(int i_ = 0; i_ < e_NextBlockList.Length; ++i_)
        {
            e_NextBlockList[i_] = Enum_BlockType.Empty;
        }
        PopulateNextBlockList();

        // Initialize block array
        BlockArray = new Enum_BlockType[i_ArrayHeight, i_ArrayWidth];
        Initialize_BlockArray();

        CreateNewBlock();

        PrintArrayToConsole();
    }
    
    Enum_BlockSize e_NextBlockSize; // Choose the size of the next random block to put on the screen
    // Fills e_NextBlockList with random blocks to push into CreateNewBlock
    void PopulateNextBlockList()
    {
        // Find the first unpopulated position
        int i_FirstOpenPosition = 0;
        for(int j_ = 0; j_ < e_NextBlockList.Length; ++j_)
        {
            if (e_NextBlockList[j_] == Enum_BlockType.Empty) continue;
            else i_FirstOpenPosition = j_;
        }

        // Numbers to randomize from. 0, 1, 2
        int i_NumBlockTypes = 2;
        if (b_ThreeBlockColors) i_NumBlockTypes = 3;

        // Start from the first open position & populate all remaining positions
        for(int i_ = i_FirstOpenPosition; i_ < e_NextBlockList.Length; ++i_)
        {
            // Find a random block
            int i_RandBlock = Random.Range(0, i_NumBlockTypes);

            // Block 'One'
            if(i_RandBlock == 0)
            {
                e_NextBlockList[i_] = Enum_BlockType.Block_1_Active;
            }
            // Block 'Two'
            else if(i_RandBlock == 1)
            {
                e_NextBlockList[i_] = Enum_BlockType.Block_2_Active;
            }
            // Block 'Three'
            else if (i_RandBlock == 2)
            {
                e_NextBlockList[i_] = Enum_BlockType.Block_3_Active;
            }
        }

        // Determine the size of the next block to use
        bool b_FoundNextBlock = false;
        // While we haven't found the next block, loop
        while(!b_FoundNextBlock)
        {
            int i_RandBlock = Random.Range(0, 4);

            if(i_RandBlock == 0 && b_2w_2h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_2w_2h;
                b_FoundNextBlock = true;
            }
            else if (i_RandBlock == 1 && b_2w_3h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_2w_3h;
                b_FoundNextBlock = true;
            }
            else if (i_RandBlock == 2 && b_3w_2h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_3w_2h;
                b_FoundNextBlock = true;
            }
            else if (i_RandBlock == 3 && b_3w_3h_Allowed)
            {
                e_NextBlockSize = Enum_BlockSize.size_3w_3h;
                b_FoundNextBlock = true;
            }
        }
    }

    void CreateNewBlock()
    {
        // Manually create a set of new blocks in the proper location
        int i_NumToShift = 0;

        // Find the 'X' location to set the block location (2 high)
        if(e_BlockSize == Enum_BlockSize.size_2w_2h || e_BlockSize == Enum_BlockSize.size_3w_2h)
        {
            // Finds the center of the List width
            v2_ActiveBlockLocation.x = (int)((i_ArrayWidth - 1) / 2);
        }
        else // (3 high)
        {
            // Finds the center of the List width, and shifts to the left one space
            v2_ActiveBlockLocation.x = (int)((i_ArrayWidth - 1) / 2) - 1;
        }

        // Find the 'Y' location to set the block location
        if (e_BlockSize == Enum_BlockSize.size_2w_3h || e_BlockSize == Enum_BlockSize.size_3w_3h)
        {
            v2_ActiveBlockLocation.y = i_ArrayHeight - 3;
        }
        else v2_ActiveBlockLocation.y = i_ArrayHeight - 2;

        // Set the number of blocks to shift afterward
        if (e_BlockSize == Enum_BlockSize.size_2w_2h)                                                 i_NumToShift = 4;
        else if (e_BlockSize == Enum_BlockSize.size_2w_3h || e_BlockSize == Enum_BlockSize.size_3w_2h)  i_NumToShift = 6;
        else if (e_BlockSize == Enum_BlockSize.size_3w_3h)                                            i_NumToShift = 9;

        // No matter what, set the initial 2x2
        SetBlock(v2_ActiveBlockLocation,                                                    e_NextBlockList[0]);
        SetBlock(new Vector2(v2_ActiveBlockLocation.x + 1, v2_ActiveBlockLocation.y + 0),   e_NextBlockList[1]);
        SetBlock(new Vector2(v2_ActiveBlockLocation.x + 0, v2_ActiveBlockLocation.y + 1),   e_NextBlockList[2]);
        SetBlock(new Vector2(v2_ActiveBlockLocation.x + 1, v2_ActiveBlockLocation.y + 1),   e_NextBlockList[3]);

        // If we're specifically 3x2, set those positions
        if( e_BlockSize == Enum_BlockSize.size_3w_2h )
        {
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 2, v2_ActiveBlockLocation.y + 1), e_NextBlockList[4]);
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 2, v2_ActiveBlockLocation.y + 0), e_NextBlockList[5]);
        }
        // If we're specifically 2x3, set those positions
        else if( e_BlockSize == Enum_BlockSize.size_2w_3h)
        {
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 0, v2_ActiveBlockLocation.y + 2), e_NextBlockList[4]);
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 1, v2_ActiveBlockLocation.y + 2), e_NextBlockList[5]);
        }
        else if( e_BlockSize == Enum_BlockSize.size_3w_3h )
        {
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 2, v2_ActiveBlockLocation.y + 1), e_NextBlockList[4]);
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 2, v2_ActiveBlockLocation.y + 0), e_NextBlockList[5]);

            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 0, v2_ActiveBlockLocation.y + 2), e_NextBlockList[6]);
            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 1, v2_ActiveBlockLocation.y + 2), e_NextBlockList[7]);

            SetBlock(new Vector2(v2_ActiveBlockLocation.x + 2, v2_ActiveBlockLocation.y + 2), e_NextBlockList[8]);
        }

        ShiftNewBlockList( i_NumToShift );
    }

    // Used within 'CreateNewBlock' to manipulate the Next Block List
    void ShiftNewBlockList( int i_NumToShift_ )
    {
        // Shift the blocks at the i_NumToShift_ positions down
        for(int i_ = 0; i_ < (e_NextBlockList.Length - i_NumToShift_); ++i_)
        {
            e_NextBlockList[i_] = e_NextBlockList[i_ + i_NumToShift_];
        }

        // Set the final positions to empty
        for(int j_ = (e_NextBlockList.Length - i_NumToShift_); j_ < e_NextBlockList.Length; ++ j_)
        {
            e_NextBlockList[j_] = Enum_BlockType.Empty;
        }

        // Re-populate the list
        PopulateNextBlockList();
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
                if (BlockArray[j, i] == Enum_BlockType.Empty) tempString += "[__] ";
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

        if (BlockSize_ == Enum_BlockSize.size_3w_2h || BlockSize_ == Enum_BlockSize.size_3w_3h)
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

        if(BlockSize_ == Enum_BlockSize.size_2w_2h)
        {
            // (0,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);

            // (1,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
        }
        #endregion

        #region 3 wide by 2 tall
        if(BlockSize_ == Enum_BlockSize.size_3w_2h || BlockSize_ == Enum_BlockSize.size_3w_3h)
        {
            // (2, 0) -> (2, -1)
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y - 1, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y));

            // (2, 1) -> (2, 0)
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y    , GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 1));

            // If 3x2:
            if (BlockSize_ == Enum_BlockSize.size_3w_2h)
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
        if (BlockSize_ == Enum_BlockSize.size_2w_3h || BlockSize_ == Enum_BlockSize.size_3w_3h)
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
        if (BlockSize_ == Enum_BlockSize.size_3w_3h)
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
    void MoveActiveBlocks_Left(Vector2 v2_BottomLeft, Enum_BlockSize e_BlockSize_)
    {
        #region Ensure appropriate spaces below are empty. Otherwise, do not do anything.
        if (v2_BottomLeft.x - 1 < 0) { return; }

        // As for block repositioning, I do not want blocks to be pushed around if another block exists.
        if (GetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 0) != Enum_BlockType.Empty) { return; }
        if (GetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 1) != Enum_BlockType.Empty) { return; }

        // Check in case the active blocks we have are 3 high
        if(e_BlockSize_ == Enum_BlockSize.size_2w_3h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
        {
            if (GetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 2) != Enum_BlockType.Empty) { return; }
        }
        #endregion

        #region Default 2x2
        // (0,0) -> (-1, 0)
        SetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 0, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y));

        // (1, 0) -> (0, 0)
        SetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 0, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y));

        // (0, 1) -> (-1, 1)
        SetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1));

        // (1, 1) -> (0, 1)
        SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 1, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1));

        if (e_BlockSize_ == Enum_BlockSize.size_2w_2h)
        {
            // (1,0) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y, Enum_BlockType.Empty);

            // (1,1) -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 1, Enum_BlockType.Empty);
        }
        #endregion

        #region 3 wide by 2 tall
        if (e_BlockSize_ == Enum_BlockSize.size_3w_2h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
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
        if (e_BlockSize_ == Enum_BlockSize.size_2w_3h || e_BlockSize_ == Enum_BlockSize.size_3w_3h)
        {
            // (0,2) -> (-1, 2) 
            SetBlock((int)v2_BottomLeft.x - 1, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 2));

            // (1,2) -> (0, 2)
            SetBlock((int)v2_BottomLeft.x, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2));

            // If 2x3: 
            if(e_BlockSize_ == Enum_BlockSize.size_2w_3h)
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
        if (e_BlockSize_ == Enum_BlockSize.size_3w_3h)
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
        if (BlockSize_ == Enum_BlockSize.size_2w_2h || BlockSize_ == Enum_BlockSize.size_2w_3h)
        {
            // If the position to the right of the block doesn't exist, quit out.
            if (v2_BottomLeft.x + 2 >= i_ArrayWidth) { return; }
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
        if(BlockSize_ == Enum_BlockSize.size_3w_3h)
        {
            // 2, 2 -> 3, 2
            SetBlock((int)v2_BottomLeft.x + 3, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2));
        }

        if(BlockSize_ == Enum_BlockSize.size_2w_3h || BlockSize_ == Enum_BlockSize.size_3w_3h)
        {
            // 1, 2 -> 2, 2
            SetBlock((int)v2_BottomLeft.x + 2, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2));

            // 0, 2 -> 1, 2
            SetBlock((int)v2_BottomLeft.x + 1, (int)v2_BottomLeft.y + 2, GetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 2));

            // 0, 2 -> CLEAR
            SetBlock((int)v2_BottomLeft.x + 0, (int)v2_BottomLeft.y + 2, Enum_BlockType.Empty);
        }

        if (BlockSize_ == Enum_BlockSize.size_3w_2h || BlockSize_ == Enum_BlockSize.size_3w_3h)
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

        // Set the next block size to be whatever we found
        e_BlockSize = e_NextBlockSize;

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
            MoveActiveBlocks_Down(v2_ActiveBlockLocation, e_BlockSize);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveActiveBlocks_Left(v2_ActiveBlockLocation, e_BlockSize);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveActiveBlocks_Right(v2_ActiveBlockLocation, e_BlockSize);

            PrintArrayToConsole();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AllBlocksStatic();

            PrintArrayToConsole();
        }
    }
}
