using UnityEngine;
using System.Collections;

public class Cs_BlockOnBoardLogic : MonoBehaviour
{
    float f_LerpTimer_Horiz;
    float f_LerpTimer_Vert;
    float f_LerpTimer_Max = 1f;

    float f_yPos;
    float f_xPos;

    float f_BlockScale;
    int i_BoardWidth;

	// Use this for initialization
	void Start ()
    {
        Init_BlockModel(5, 5, 3, 20);
	}

    public void Init_BlockModel( int i_xPos_, int i_yPos_, float f_BlockScale_, int i_BoardWidth_ )
    {
        f_xPos = i_xPos_;
        f_yPos = i_yPos_;

        f_BlockScale = f_BlockScale_;
        i_BoardWidth = i_BoardWidth_;

        gameObject.transform.position = new Vector3(f_xPos * f_BlockScale, f_yPos * f_BlockScale, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.transform.position != new Vector3(f_xPos * f_BlockScale, f_yPos * f_BlockScale, 0))
        {
            float f_xPos_Temp;
            float f_yPos_Temp;

            // Lerp to current X
            if(f_LerpTimer_Horiz < f_LerpTimer_Max)
            {
                f_LerpTimer_Horiz += Time.deltaTime;

                if ( f_LerpTimer_Horiz > f_LerpTimer_Max )
                {
                    f_LerpTimer_Horiz = f_LerpTimer_Max;
                }
            }
            
            f_xPos_Temp = Mathf.Lerp(gameObject.transform.position.x, f_xPos * f_BlockScale, f_LerpTimer_Horiz / f_LerpTimer_Max);

            // Lerp to current Y
            if(f_LerpTimer_Vert < f_LerpTimer_Max)
            {
                f_LerpTimer_Vert += Time.deltaTime;

                if(f_LerpTimer_Vert > f_LerpTimer_Max)
                {
                    f_LerpTimer_Vert = f_LerpTimer_Max;
                }
            }

            f_yPos_Temp = Mathf.Lerp(gameObject.transform.position.y, f_yPos * f_BlockScale, f_LerpTimer_Vert / f_LerpTimer_Max);

            gameObject.transform.position = new Vector3(f_xPos_Temp, f_yPos_Temp, 0);
        }
	}

    public void Set_MoveLeft()
    {
        // Decrement X position by 1
        if(f_xPos - 1 >= 0)
        {
            f_xPos -= 1;
        }

        // Reset LerpTimer_Horiz
        f_LerpTimer_Horiz = 0f;
    }

    public void Set_MoveRight()
    {
        // Increment X position by 1
        if( f_xPos + 1< i_BoardWidth)
        {
            f_xPos += 1;
        }

        // Reset LerpTimer_Horiz
        f_LerpTimer_Horiz = 0.0f;
    }

    public void Set_MoveDown()
    {
        // Decrement Y position by 1
        if(f_yPos - 1 >= 0)
        {
            f_yPos -= 1;
        }

        // Reset LerpTimer_Vert
        f_LerpTimer_Vert = 0.0f;
    }

    public void Set_MoveDownToPos( int f_yPos_ )
    {
        // Get current Y position
        int i_Temp = (int)(f_yPos - f_yPos_);

        // Loop
        for(int i_ = 0; i_ < i_Temp; ++i_)
        {
            Set_MoveDown();
        }
    }
}
