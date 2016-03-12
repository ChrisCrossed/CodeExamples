using UnityEngine;
using System.Collections;

public class Cs_LevelManager : MonoBehaviour
{
    // Level Game Objects
    GameObject Wall_PosX;
    GameObject Wall_NegX;
    GameObject Wall_PosY;
    GameObject Wall_NegY;
    GameObject Floor;
    int i_Pos_Level1;
    int i_Pos_Level2;
    int i_Pos_Level3;
    int i_Pos_Level4;
    int i_Pos_Level5;
    int i_Width_Level1;
    int i_Width_Level2;
    int i_Width_Level3;
    int i_Width_Level4;
    int i_Width_Level5;
    GameObject hudCamera;

    // Timer Objects
    public float GameLengthMinutes;
    public float TimeIncreaseOnScore;
    float f_Timer;
    float f_Countdown;
    float f_TimerIncrement;
    int i_CurrLevel;

    // Score Game Objects
    GameObject Score_Primary;
    GameObject Score_Secondary;

	// Use this for initialization
	void Start ()
    {
        i_CurrLevel = 1;

        // Level Game Objects
        Wall_PosX = GameObject.Find("Wall_PosX");
        Wall_NegX = GameObject.Find("Wall_NegX");
        Wall_PosY = GameObject.Find("Wall_PosY");
        Wall_NegY = GameObject.Find("Wall_NegY");
        Floor = GameObject.Find("Floor");
        i_Pos_Level1 = 325;
        i_Pos_Level2 = 275;
        i_Pos_Level3 = 225;
        i_Pos_Level4 = 175;
        i_Pos_Level5 = 125;
        i_Width_Level1 = 650;
        i_Width_Level2 = 550;
        i_Width_Level3 = 450;
        i_Width_Level4 = 350;
        i_Width_Level5 = 250;
        hudCamera = GameObject.Find("HUD_Camera");

        // Timer Objects
        f_Timer = 0;
        f_Countdown = GameLengthMinutes * 60f;
        f_TimerIncrement = TimeIncreaseOnScore;

        // Score Game Objects;
        Score_Primary = GameObject.Find("");
        Score_Secondary = GameObject.Find("");
    }

    void UpdateLevelSpecs(int i_LevelWidth_)
    {
        // Update current floor width
        Vector3 floorWidth = Floor.transform.localScale;

        if (floorWidth.x > i_LevelWidth_ / 10)
        {
            floorWidth.x -= Time.deltaTime * 5;
            floorWidth.z -= Time.deltaTime * 5;
        }
        else
        {
            floorWidth.x = i_LevelWidth_ / 10;
            floorWidth.z = i_LevelWidth_ / 10;
        }

        Floor.transform.localScale = floorWidth;

        // Update current wall positions
        float wallPos = Floor.transform.localScale.x * 10 / 2;

        Wall_PosX.transform.position = new Vector3(wallPos, Wall_PosX.transform.position.y, Wall_PosX.transform.position.z);

        Wall_NegX.transform.position = new Vector3(-wallPos, Wall_NegX.transform.position.y, Wall_NegX.transform.position.z);

        Wall_PosY.transform.position = new Vector3(Wall_PosY.transform.position.x, Wall_PosY.transform.position.y, wallPos);

        Wall_NegY.transform.position = new Vector3(Wall_NegY.transform.position.x, Wall_NegY.transform.position.y, -wallPos);

        // Update current wall lengths
        float wallLength = Floor.transform.localScale.x * 10;

        Wall_PosX.transform.localScale = new Vector3(Wall_PosX.transform.localScale.x, Wall_PosX.transform.localScale.y, wallLength);

        Wall_NegX.transform.localScale = new Vector3(Wall_NegX.transform.localScale.x, Wall_NegX.transform.localScale.y, wallLength);

        Wall_PosY.transform.localScale = new Vector3(wallLength, Wall_PosY.transform.localScale.y, Wall_PosY.transform.localScale.z);

        Wall_NegY.transform.localScale = new Vector3(wallLength, Wall_NegY.transform.localScale.y, Wall_NegY.transform.localScale.z);

        // Lerp camera's height
        Vector3 newCamPos = hudCamera.transform.position;
        newCamPos.y = Mathf.Lerp(newCamPos.y, i_LevelWidth_, 0.05f / i_CurrLevel);
        hudCamera.transform.position = newCamPos;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.P) && i_CurrLevel < 5)
        {
            ++i_CurrLevel;
        }

	    if (i_CurrLevel == 1)
        {
            UpdateLevelSpecs(i_Width_Level1);
        }
        else if (i_CurrLevel == 2)
        {
            UpdateLevelSpecs(i_Width_Level2);
        }
        else if (i_CurrLevel == 3)
        {
            UpdateLevelSpecs(i_Width_Level3);
        }
        else if (i_CurrLevel == 4)
        {
            UpdateLevelSpecs(i_Width_Level4);
        }
        else // i_CurrLevel == 5
        {
            UpdateLevelSpecs(i_Width_Level5);
        }
    }
}
