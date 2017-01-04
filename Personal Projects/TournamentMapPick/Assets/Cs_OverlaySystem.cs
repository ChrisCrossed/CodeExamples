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

    Control_Ilios = 3,
    Control_Lijang = 4,
    Control_Nepal = 5,

    Hybrid_Hollywood = 6,
    Hybrid_KingsRow = 7,
    Hybrid_Numbani = 8,
    Hybrid_Eichenwalde = 9,

    Escort_Dorado = 10,
    Escort_Gibraltar = 11,
    Escort_Route66 = 12,
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

    // Buttons
    GameObject button_Hanamura;
    GameObject button_Anubis;
    GameObject button_Volskaya;
    GameObject button_Ilios;
    GameObject button_Lijang;
    GameObject button_Nepal;
    GameObject button_Hollywood;
    GameObject button_KingsRow;
    GameObject button_Numbani;
    GameObject button_Eichenwalde;
    GameObject button_Dorado;
    GameObject button_Gibraltar;
    GameObject button_Route66;

    // Use this for initialization
    void Start ()
    {
        // Sets number of maps in use
        i_NumMaps = 13;

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

        // Button connections
        button_Hanamura = GameObject.Find("Button_Hanamura");
        button_Anubis = GameObject.Find("Button_Anubis");
        button_Volskaya = GameObject.Find("Button_Volskaya");
        button_Ilios = GameObject.Find("Button_Ilios");
        button_Lijang = GameObject.Find("Button_Lijang");
        button_Nepal = GameObject.Find("Button_Nepal");
        button_Hollywood = GameObject.Find("Button_Hollywood");
        button_KingsRow = GameObject.Find("Button_KingsRow");
        button_Numbani = GameObject.Find("Button_Numbani");
        button_Eichenwalde = GameObject.Find("Button_Eichenwalde");
        button_Dorado = GameObject.Find("Button_Dorado");
        button_Gibraltar = GameObject.Find("Button_Gibraltar");
        button_Route66 = GameObject.Find("Button_Route66");

        dieGraphic = GameObject.Find("DieGraphic");
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

    int i_TEST;
    int i_TurnCounter = -1;
    static bool b_BANNED = true;
    static bool b_PICKED = false;
    int i_RandomMap;
    GameObject go_CurrMap;
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

                    // Begin rolling the die for the last random map
                    b_DieActive = true;
                    break;
                case 8:
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition( go_BO3_Right );

                    // Run through remaining maps and disable them
                    for(int i_ = 0; i_ < i_NumMaps; ++i_)
                    {
                        if(b_MapActive[i_])
                        {
                            if (i_ == 0) button_Hanamura.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 1) button_Anubis.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 2) button_Volskaya.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 3) button_Ilios.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 4) button_Lijang.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 5) button_Nepal.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 6) button_Hollywood.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 7) button_KingsRow.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 8) button_Numbani.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 9) button_Eichenwalde.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 10) button_Dorado.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 11) button_Gibraltar.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED; else
                            if (i_ == 12) button_Route66.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                        }
                    }

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

    float f_AnticipationTimer;
    float f_DieTimer = 1.0f;
    float f_DieTimer_Max = 0.5f;
    float f_DieTimer_Min = 0.1f;
    int i_DieSide = 0;
    GameObject dieGraphic;
    [SerializeField] Sprite[] spr_Dice;
    void RollDie( bool b_IsActive_ = true )
    {
        if( b_IsActive_ )
        {
            // Enable graphic
            dieGraphic.SetActive(true);

            // Increment alpha
            Color clr_Alpha = dieGraphic.GetComponent<Image>().color;
            clr_Alpha.a += Time.deltaTime;
            if (clr_Alpha.a > 1.0f) clr_Alpha.a = 1.0f;
            dieGraphic.GetComponent<Image>().color = clr_Alpha;

            // Decrement Die Timer
            f_DieTimer -= Time.deltaTime;

            // If Die Timer <= 0, Change Graphic, Reduce Max Die Timer, Reset Die Timer
            if (f_DieTimer <= 0f)
            {
                #region Change Graphic
                // Change graphic
                bool b_NewSideFound = false;
                while (!b_NewSideFound)
                {
                    int i_NewSide = Random.Range(1, 6);

                    if (i_NewSide != i_DieSide)
                    {
                        i_DieSide = i_NewSide;

                        switch (i_DieSide)
                        {
                            case 1:
                                dieGraphic.GetComponent<Image>().sprite = spr_Dice[0];
                                break;
                            case 2:
                                dieGraphic.GetComponent<Image>().sprite = spr_Dice[1];
                                break;
                            case 3:
                                dieGraphic.GetComponent<Image>().sprite = spr_Dice[2];
                                break;
                            case 4:
                                dieGraphic.GetComponent<Image>().sprite = spr_Dice[3];
                                break;
                            case 5:
                                dieGraphic.GetComponent<Image>().sprite = spr_Dice[4];
                                break;
                            case 6:
                                dieGraphic.GetComponent<Image>().sprite = spr_Dice[5];
                                break;
                            default:
                                break;
                        }

                        b_NewSideFound = true;
                    }
                }
                #endregion

                // Reduce Max Die Timer
                f_DieTimer_Max -= 0.05f;
                if (f_DieTimer_Max <= f_DieTimer_Min) f_DieTimer_Max = f_DieTimer_Min;

                // Reset Die Timer
                f_DieTimer = f_DieTimer_Max;

                if(f_AnticipationTimer < (5.0f * f_DieTimer_Min) && f_DieTimer_Max ==  f_DieTimer_Min)
                {
                    f_AnticipationTimer += Time.deltaTime;

                    if(f_AnticipationTimer > (5.0f * f_DieTimer_Min) )
                    {
                        b_DieActive = false;

                        StartCoroutine( PickRandomMap() );
                    }
                }
            }
        }
        else
        {
            // Reset values
            f_DieTimer = 1.0f;
            f_DieTimer_Max = 0.5f;

            // Increment alpha
            Color clr_Alpha = dieGraphic.GetComponent<Image>().color;
            clr_Alpha.a -= Time.deltaTime;
            if (clr_Alpha.a < 0f)
            {
                dieGraphic.SetActive(false);

                clr_Alpha.a = 0.0f;
            }
            dieGraphic.GetComponent<Image>().color = clr_Alpha;
        }
    }

    GameObject go_MapOut;
    IEnumerator PickRandomMap()
    {
        yield return new WaitForSeconds(0.5f);

        // While we haven't found an unpicked map
        bool b_FoundMap = false;
        while (!b_FoundMap)
        {
            i_RandomMap = Random.Range(0, i_NumMaps);

            if (b_MapActive[i_RandomMap]) b_FoundMap = true;
        }

        // i_RandomMap = 4;
        print("Picked map: " + i_RandomMap);

        #region Set map based on random number
        switch (i_RandomMap)
        {
            case 0:
                go_CurrMap = button_Hanamura;
                break;
            case 1:
                go_CurrMap = button_Anubis;
                break;
            case 2:
                go_CurrMap = button_Volskaya;
                break;
            case 3:
                go_CurrMap = button_Ilios;
                break;
            case 4:
                go_CurrMap = button_Lijang;
                break;
            case 5:
                go_CurrMap = button_Nepal;
                break;
            case 6:
                go_CurrMap = button_Hollywood;
                break;
            case 7:
                go_CurrMap = button_KingsRow;
                break;
            case 8:
                go_CurrMap = button_Numbani;
                break;
            case 9:
                go_CurrMap = button_Eichenwalde;
                break;
            case 10:
                go_CurrMap = button_Dorado;
                break;
            case 11:
                go_CurrMap = button_Gibraltar;
                break;
            case 12:
                go_CurrMap = button_Route66;
                break;
            default:
                break;
        }
        #endregion

        go_MapOut = go_CurrMap;

        b_MapActive[i_RandomMap] = false;

        go_CurrMap.GetComponent<Cs_Button_Map>().ClickButton();
    }

    // Update is called once per frame
    bool b_DieActive;
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P)) b_DieActive = !b_DieActive;

        RollDie( b_DieActive );
    }
}
