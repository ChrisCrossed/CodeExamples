using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Enum_MapList
{
    Assault_Hanamura = 0,
    Assault_TempleOfAnubis = 1,
    Assault_Volskaya = 2,

    Escort_Dorado = 3,
    Escort_Gibraltar = 4,
    Escort_Route66 = 5,

    Hybrid_Hollywood = 6,
    Hybrid_KingsRow = 7,
    Hybrid_Numbani = 8,
    Hybrid_Eichenwalde = 9,

    Control_Ilios = 10,
    Control_Lijang = 11,
    Control_Nepal = 12,
    Control_Oasis = 13
}

public enum Enum_MapState
{
    Default = 0,
    Banned = 1,
    Selected = 2
}

public class Cs_OverlaySystem : MonoBehaviour
{
    // Variables
    int i_NumMaps;
    bool[] b_MapActive;
    [SerializeField] bool b_BestOf3 = true;

    // Game Object Connections
    RectTransform go_BanPos_Team1_1;
    RectTransform go_BanPos_Team1_2;
    RectTransform go_BanPos_Team1_3;
    RectTransform go_BanPos_Team2_1;
    RectTransform go_BanPos_Team2_2;
    RectTransform go_BanPos_Team2_3;
    RectTransform go_BO5_Left;
    RectTransform go_BO3_Left;
    RectTransform go_BO3_Center;
    RectTransform go_BO3_Right;
    RectTransform go_BO5_Right;

    // Use this for initialization
    void Start ()
    {
        // Sets number of maps in use
        i_NumMaps = 14;

        // Activates the number of maps to use
        b_MapActive = new bool[ i_NumMaps ];

        // Sets all maps to be considered active
        for(int i_ = 0; i_ < i_NumMaps; ++i_ )
        {
            b_MapActive[i_] = true;
        }

        // Set RectTransform positions
        go_BanPos_Team1_1 = GameObject.Find("BanPos_Team1_1").GetComponent<RectTransform>();
        go_BanPos_Team1_2 = GameObject.Find("BanPos_Team1_2").GetComponent<RectTransform>();
        go_BanPos_Team1_3 = GameObject.Find("BanPos_Team1_3").GetComponent<RectTransform>();
        go_BanPos_Team2_1 = GameObject.Find("BanPos_Team2_1").GetComponent<RectTransform>();
        go_BanPos_Team2_2 = GameObject.Find("BanPos_Team2_2").GetComponent<RectTransform>();
        go_BanPos_Team2_3 = GameObject.Find("BanPos_Team2_3").GetComponent<RectTransform>();
        go_BO5_Left     = GameObject.Find("BO5_Left").GetComponent<RectTransform>();
        go_BO3_Left     = GameObject.Find("BO3_Left").GetComponent<RectTransform>();
        go_BO3_Center   = GameObject.Find("BO3_Mid").GetComponent<RectTransform>();
        go_BO3_Right    = GameObject.Find("BO3_Right").GetComponent<RectTransform>();
        go_BO5_Right    = GameObject.Find("BO5_Right").GetComponent<RectTransform>();

        if(b_BestOf3)
        {
            GameObject.Find("BO5_Left").SetActive(false);
            GameObject.Find("BO5_Right").SetActive(false);
            GameObject.Find("Bar_BO5_Left").SetActive(false);
            GameObject.Find("Bar_BO5_Right").SetActive(false);
        }
    }
    
    public void MapClicked( GameObject go_Button_ )
    {
        int i_MapType = (int)go_Button_.GetComponent<Cs_Button_Map>().MapType;

        // Disable map from list
        b_MapActive[ i_MapType ] = false;

        // Tell button it cannot be clicked anymore
        go_Button_.GetComponent<Button>().interactable = false;

        // Tell map to move to proper position
        PositionButton( go_Button_ );

        // Update PickBan
    }

    int i_TurnCounter = -1;
    static bool b_BANNED = true;
    static bool b_PICKED = false;
    void PositionButton( GameObject go_Button_ )
    {
        // Increment turn counter
        ++i_TurnCounter;

        Cs_Button_Map this_Button = go_Button_.GetComponent<Cs_Button_Map>();

        if( b_BestOf3 )
        {
            // Ban (A), Ban (B), Ban (A), Ban (B), PICK (A), PICK (B), Ban (A), Ban (B), Random 1
            #region Best of 3 Format
            switch (i_TurnCounter)
            {
                case 0:
                    // Ban map, Team A, Position 1
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team1_1 );
                    break;
                case 1:
                    // Ban map, Team B, Position 1
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team2_1 );
                    break;
                case 2:
                    // Ban map, Team A, Position 2
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team1_2 );
                    break;
                case 3:
                    // Ban map, Team B, Position 2
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team2_2 );
                    break;
                case 4:
                    // Pick map, Position 1
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition( go_BO3_Left );
                    break;
                case 5:
                    // Pick map, Position 2
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition( go_BO3_Center );
                    break;
                case 6:
                    // Ban map, Team A, Position 3
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team1_3 );
                    break;
                case 7:
                    // Ban map, Team B, Position 3
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team2_3 );
                    break;
                default:
                    break;
            }
            #endregion
        }
        else
        {
            // Ban (A), Ban (B), Ban (A), Ban (B), PICK (A), PICK (B), PICK (B), PICK (A), Ban (A), Ban (B), Random 1
            #region Best of 5 Format

            #endregion
        }
    }


	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
