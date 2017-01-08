﻿using System.Collections;
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

public enum Enum_TeamList
{
    DigiPen,
    UW,
    CWU,
    WWU
}

enum Enum_TeamTurn
{
    Neither = 0,
    Team_A = 1,
    Team_B = 2
}

public class Cs_OverlaySystem : MonoBehaviour
{
    // Variables
    int i_NumMaps;
    bool[] b_MapActive;
    [SerializeField] bool b_BestOf3 = true;
    Enum_TeamTurn e_TeamTurn;

    // Team Logos
    [SerializeField] Enum_TeamList e_TeamOne;
    [SerializeField] Enum_TeamList e_TeamTwo;
    [SerializeField] Sprite[] TeamLogos;
    Image img_Backdrop_Left;
    Image img_Backdrop_Right;
    [SerializeField] Sprite img_TeamBackdrop_Ban;
    [SerializeField] Sprite img_TeamBackdrop_Pick;

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

    // Trophy Positions Based on Team Turn
    GameObject go_Trophy;
    Vector3 v3_TrophyPos_Left;
    Vector3 v3_TrophyPos_Center;
    Vector3 v3_TrophyPos_Right;

    // Use this for initialization
    void Start ()
    {
        #if !UNITY_EDITOR
        // Cursor.visible = false;
        #endif

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

            f_PickBanOver_Threshold = 5.0f;
        }
        else
        {
            f_PickBanOver_Threshold = 6.0f;
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
        ui_Text = GameObject.Find("Text_Timer").GetComponent<Text>();
        ui_Text_PickBan = GameObject.Find("Text_PickBan").GetComponent<Text>();

        GameClock( true );

        go_Trophy = GameObject.Find("trophy");
        v3_TrophyPos_Left = GameObject.Find("TrophyPos_Left").transform.position;
        v3_TrophyPos_Center = GameObject.Find("TrophyPos_Center").transform.position;
        v3_TrophyPos_Right = GameObject.Find("TrophyPos_Right").transform.position;

        tr_Ban = GameObject.Find("Ban Positions").transform;
        tr_Pick = GameObject.Find("Selected Maps").transform;
        img_GameClock = GameObject.Find("Img_GameClock").GetComponent<Image>();
        img_GameClock_Backdrop = GameObject.Find("Img_GameClock_Backdrop").GetComponent<Image>();

        v3_OffScreenPos = GameObject.Find("OffScreenPosition").transform.position;

        v3_FinalPos_BO3_1 = GameObject.Find("EndButtonPos_BO3_1").transform.position;
        v3_FinalPos_BO3_2 = GameObject.Find("EndButtonPos_BO3_2").transform.position;
        v3_FinalPos_BO3_3 = GameObject.Find("EndButtonPos_BO3_3").transform.position;
        v3_FinalPos_BO5_1 = GameObject.Find("EndButtonPos_BO5_1").transform.position;
        v3_FinalPos_BO5_3 = GameObject.Find("EndButtonPos_BO5_3").transform.position;

        if( b_BestOf3)
        {
            v3_FinalPos_BO3_1 = new Vector3(-480, 300, 0);
            v3_FinalPos_BO3_2 = new Vector3(0, 0, 0);
            v3_FinalPos_BO3_3 = new Vector3(480, -300, 0);
        }
        else
        {
            v3_FinalPos_BO3_1 = new Vector3(-600, 400, 0);
            v3_FinalPos_BO3_2 = new Vector3(0, 0, 0);
            v3_FinalPos_BO3_3 = new Vector3(600, -400, 0);
            v3_FinalPos_BO5_1 = new Vector3(300, -200, 0);
            v3_FinalPos_BO5_3 = new Vector3(-300, 200, 0);
        }
    }
    
    void LoadTeamGraphics()
    {
        #region Set icons based on team
        if (b_BestOf3)
        {
            // Remove left & right side icons
            GameObject.Find("Logo_BO5_Left").SetActive(false);
            GameObject.Find("Logo_BO5_Right").SetActive(false);

            Image Logo_One = GameObject.Find("Logo_BO3_Left").GetComponent<Image>();
            Image Logo_Two = GameObject.Find("Logo_BO3_Mid").GetComponent<Image>();
            Image Logo_Three = GameObject.Find("Logo_BO3_Right").GetComponent<Image>();

            img_Backdrop_Left = GameObject.Find("TeamLogo_Left_Back").GetComponent<Image>();
            img_Backdrop_Right = GameObject.Find("TeamLogo_Right_Back").GetComponent<Image>();

            // Set icons based on team
            if (e_TeamOne == Enum_TeamList.CWU) Logo_One.sprite = TeamLogos[0];
            else if (e_TeamOne == Enum_TeamList.DigiPen) Logo_One.sprite = TeamLogos[1];
            else if (e_TeamOne == Enum_TeamList.UW) Logo_One.sprite = TeamLogos[2];
            else Logo_One.sprite = TeamLogos[3];

            if (e_TeamTwo == Enum_TeamList.CWU) Logo_Two.sprite = TeamLogos[0];
            else if (e_TeamTwo == Enum_TeamList.DigiPen) Logo_Two.sprite = TeamLogos[1];
            else if (e_TeamTwo == Enum_TeamList.UW) Logo_Two.sprite = TeamLogos[2];
            else Logo_Two.sprite = TeamLogos[3];

            Logo_Three.sprite = spr_Dice[5];
        }
        else
        {
            Image Logo_One = GameObject.Find("Logo_BO5_Left").GetComponent<Image>();
            Image Logo_Two = GameObject.Find("Logo_BO3_Left").GetComponent<Image>();
            Image Logo_Three = GameObject.Find("Logo_BO3_Mid").GetComponent<Image>();
            Image Logo_Four = GameObject.Find("Logo_BO3_Right").GetComponent<Image>();
            Image Logo_Five = GameObject.Find("Logo_BO5_Right").GetComponent<Image>();

            img_Backdrop_Left = GameObject.Find("TeamLogo_Left_Back").GetComponent<Image>();
            img_Backdrop_Right = GameObject.Find("TeamLogo_Right_Back").GetComponent<Image>();

            // Set icons based on team
            if (e_TeamOne == Enum_TeamList.CWU)
            {
                Logo_One.sprite = TeamLogos[0];
                Logo_Three.sprite = TeamLogos[0];
            }
            else if (e_TeamOne == Enum_TeamList.DigiPen)
            {
                Logo_One.sprite = TeamLogos[1];
                Logo_Three.sprite = TeamLogos[1];
            }
            else if (e_TeamOne == Enum_TeamList.UW)
            {
                Logo_One.sprite = TeamLogos[2];
                Logo_Three.sprite = TeamLogos[2];
            }
            else
            {
                Logo_One.sprite = TeamLogos[3];
                Logo_Three.sprite = TeamLogos[3];
            }

            if (e_TeamTwo == Enum_TeamList.CWU)
            {
                Logo_Two.sprite = TeamLogos[0];
                Logo_Four.sprite = TeamLogos[0];
            }
            else if (e_TeamTwo == Enum_TeamList.DigiPen)
            {
                Logo_Two.sprite = TeamLogos[1];
                Logo_Four.sprite = TeamLogos[1];
            }
            else if (e_TeamTwo == Enum_TeamList.UW)
            {
                Logo_Two.sprite = TeamLogos[2];
                Logo_Four.sprite = TeamLogos[2];
            }
            else
            {
                Logo_Two.sprite = TeamLogos[3];
                Logo_Four.sprite = TeamLogos[3];
            }

            Logo_Five.sprite = spr_Dice[5];
        }
        #endregion

        if (e_TeamOne == Enum_TeamList.CWU) GameObject.Find("TeamLogo_Left").GetComponent<Image>().sprite = TeamLogos[0];
        else if (e_TeamOne == Enum_TeamList.DigiPen) GameObject.Find("TeamLogo_Left").GetComponent<Image>().sprite = TeamLogos[1];
        else if (e_TeamOne == Enum_TeamList.UW) GameObject.Find("TeamLogo_Left").GetComponent<Image>().sprite = TeamLogos[2];
        else GameObject.Find("TeamLogo_Left").GetComponent<Image>().sprite = TeamLogos[3];

        if (e_TeamTwo == Enum_TeamList.CWU) GameObject.Find("TeamLogo_Right").GetComponent<Image>().sprite = TeamLogos[0];
        else if (e_TeamTwo == Enum_TeamList.DigiPen) GameObject.Find("TeamLogo_Right").GetComponent<Image>().sprite = TeamLogos[1];
        else if (e_TeamTwo == Enum_TeamList.UW) GameObject.Find("TeamLogo_Right").GetComponent<Image>().sprite = TeamLogos[2];
        else GameObject.Find("TeamLogo_Right").GetComponent<Image>().sprite = TeamLogos[3];

        img_Backdrop_Left.sprite = img_TeamBackdrop_Ban;
        img_Backdrop_Right.sprite = img_TeamBackdrop_Ban;
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

        ui_Text_PickBan.enabled = false;
    }

    int i_TEST;
    int i_TurnCounter = -1;
    static bool b_BANNED = true;
    static bool b_PICKED = false;
    int i_RandomMap;
    GameObject go_CurrMap;
    Transform tr_Ban;
    Transform tr_Pick;
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
                    this_Button.GoToPosition( go_BanPos_Team1_1, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    break;
                case 1:
                    // Ban map, Team B, Position 1
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team2_1, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    break;
                case 2:
                    // Ban map, Team A, Position 2
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team1_2, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_B;

                    b_SetToSwitch_Left = true;
                    break;
                case 3:
                    // Ban map, Team B, Position 2
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team2_2, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    b_SetToSwitch_Right = true;
                    ui_Text_PickBan.text = "PICK";
                    ui_Text_PickBan.color = new Color(0, 0.5f, 0, 1.0f);
                    ui_Text.color = new Color(0, 0.5f, 0, 1.0f);
                    break;
                case 4:
                    // Pick map, Position 1
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition( go_BO3_Left, tr_Pick);
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    b_SetToSwitch_Left = true;

                    // Set button for ending manipulation
                    map_Button1 = go_Button_;

                    break;
                case 5:
                    // Pick map, Position 2
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition( go_BO3_Center, tr_Pick);
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    b_SetToSwitch_Right = true;
                    ui_Text_PickBan.text = "BAN";
                    ui_Text_PickBan.color = new Color(0.5f, 0f, 0, 1.0f);
                    ui_Text.color = new Color(0.5f, 0f, 0, 1.0f);

                    // Set button for ending manipulation
                    map_Button2 = go_Button_;

                    break;
                case 6:
                    // Ban map, Team A, Position 3
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team1_3, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    break;
                case 7:
                    // Ban map, Team B, Position 3
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team2_3, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Neither;

                    // Begin rolling the die for the last random map
                    Run_RollForRandomMap();

                    // Disable the mouse cursor input
                    GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = false;

                    // Turn off the Game Clock object
                    GameClockVisible( false );

                    ui_Text_PickBan.text = "";

                    break;
                case 8:
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition( go_BO3_Right, tr_Pick );
                    
                    // Set button for ending manipulation
                    map_Button3 = go_Button_;
                    
                    b_PickBanActive = false;

                    // Run through remaining maps and disable them
                    for (int i_ = 0; i_ < i_NumMaps; ++i_)
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
            // Ban (A), Ban (B), Ban (A), Ban (B), PICK (A), PICK (B), PICK (A), PICK (B), Ban (A), Ban (B), Random 1
            #region Best of 5 Format
            switch (i_TurnCounter)
            {
                case 0:
                    // Ban map, Team A, Position 1
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition( go_BanPos_Team1_1, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    break;
                case 1:
                    // Ban map, Team B, Position 1
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition(go_BanPos_Team2_1, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    break;
                case 2:
                    // Ban map, Team A, Position 2
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition(go_BanPos_Team1_2, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    b_SetToSwitch_Left = true;
                    break;
                case 3:
                    // Ban map, Team B, Position 2
                    this_Button.GoToPosition(go_BanPos_Team2_2, tr_Ban );
                    this_Button.Set_MapState = b_BANNED;
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    b_SetToSwitch_Right = true;
                    ui_Text_PickBan.text = "PICK";
                    ui_Text_PickBan.color = new Color(0, 0.5f, 0, 1.0f);
                    ui_Text.color = new Color(0, 0.5f, 0, 1.0f);
                    break;
                case 4:
                    // Pick map, Position 1
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition(go_BO5_Left, tr_Pick );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    map_Button1 = go_Button_;
                    break;
                case 5:
                    // Pick map, Position 2
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition(go_BO3_Left, tr_Pick );
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    map_Button2 = go_Button_;
                    break;
                case 6:
                    // Pick map, Position 2
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition(go_BO3_Center, tr_Pick );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    b_SetToSwitch_Left = true;
                    map_Button3 = go_Button_;
                    break;
                case 7:
                    // Pick map, Position 1
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition(go_BO3_Right, tr_Pick );
                    e_TeamTurn = Enum_TeamTurn.Team_A;
                    b_SetToSwitch_Right = true;
                    ui_Text_PickBan.text = "BAN";
                    ui_Text_PickBan.color = new Color(0.5f, 0f, 0, 1.0f);
                    ui_Text.color = new Color(0.5f, 0f, 0, 1.0f);
                    map_Button4 = go_Button_;
                    break;
                case 8:
                    // Ban map, Team A, Position 3
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition(go_BanPos_Team1_3, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Team_B;
                    break;
                case 9:
                    // Ban map, Team B, Position 3
                    this_Button.Set_MapState = b_BANNED;
                    this_Button.GoToPosition(go_BanPos_Team2_3, tr_Ban );
                    e_TeamTurn = Enum_TeamTurn.Neither;
                    ui_Text_PickBan.text = "";

                    // Begin rolling the die for the last random map
                    Run_RollForRandomMap();

                    // Disable the mouse cursor input
                    GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = false;
                    
                    // Turn off the Game Clock object
                    GameClockVisible(false);

                    break;
                case 10:
                    this_Button.Set_MapState = b_PICKED;
                    this_Button.GoToPosition(go_BO5_Right, tr_Pick );
                    map_Button5 = go_Button_;

                    b_PickBanActive = false;

                    // Run through remaining maps and disable them
                    for (int i_ = 0; i_ < i_NumMaps; ++i_)
                    {
                        if (b_MapActive[i_])
                        {
                            if (i_ == 0) button_Hanamura.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 1) button_Anubis.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 2) button_Volskaya.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 3) button_Ilios.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 4) button_Lijang.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 5) button_Nepal.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 6) button_Hollywood.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 7) button_KingsRow.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 8) button_Numbani.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 9) button_Eichenwalde.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 10) button_Dorado.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 11) button_Gibraltar.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                            else
                            if (i_ == 12) button_Route66.GetComponent<Cs_Button_Map>().Set_MapState = b_BANNED;
                        }
                    }
                    break;

                default:
                    break;
            }
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
            GameClock( true );

            // Disable the mouse cursor input
            GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = false;

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

        b_MapActive[i_RandomMap] = false;

        go_CurrMap.GetComponent<Cs_Button_Map>().ClickButton();
    }
    public void Run_RollForRandomMap()
    {
        f_AnticipationTimer = 0f;
        f_DieTimer = 1.0f;
        f_DieTimer_Max = 0.5f;
        f_DieTimer_Min = 0.1f;
        i_DieSide = 0;

        b_DieActive = true;
    }

    float f_Timer;
    int i_GameClock;
    [SerializeField] int i_GameClock_Max = 31;
    Text ui_Text;
    Text ui_Text_PickBan;
    public void GameClock( bool b_Reset_ = false )
    {
        // If we aren't resetting, then continue the count
        if( !b_Reset_ )
        {
            // Increment timer
            f_Timer += Time.deltaTime;

            if ( i_GameClock > 1 )
            {
                // Enable Pick/Ban text
                ui_Text_PickBan.enabled = true;

                if (f_Timer >= 1.0f)
                {
                    i_GameClock -= 1;

                    f_Timer = 0f;

                    ui_Text.text = string.Format("{0:00}", i_GameClock);
                }
            }
            else
            {
                ui_Text.text = string.Format("{0:0.0}", (1.0f - f_Timer));

                if( 1.0f - f_Timer <= 0f )
                {
                    i_GameClock = i_GameClock_Max;
                    f_Timer = 0f;
                    ui_Text.text = string.Format("0.0", i_GameClock);

                    Run_RollForRandomMap();
                }
            }
        }
        else
        {
            i_GameClock = i_GameClock_Max;
            f_Timer = 0f;
            ui_Text.text = "";
            ui_Text_PickBan.enabled = false;
        }
    }

    bool b_GameClockVisible = true;
    Image img_GameClock;
    Image img_GameClock_Backdrop;
    void GameClockVisible( bool b_IsVisible_ = true )
    {
        if ( !b_IsVisible_ ) b_GameClockVisible = b_IsVisible_;

        if( !b_GameClockVisible )
        {
            Color clr_GameClock_Alpha = img_GameClock.color;
            Color clr_Backdrop_Alpha = img_GameClock_Backdrop.color;

            if (clr_Backdrop_Alpha.a > 0f || clr_GameClock_Alpha.a > 0f)
            {
                clr_Backdrop_Alpha.a -= Time.deltaTime;
                clr_GameClock_Alpha.a -= Time.deltaTime;

                if (clr_Backdrop_Alpha.a < 0f) clr_Backdrop_Alpha.a = 0f;
                if (clr_GameClock_Alpha.a < 0f) clr_GameClock_Alpha.a = 0f;
            }

            img_GameClock.color = clr_GameClock_Alpha;
            img_GameClock_Backdrop.color = clr_Backdrop_Alpha;
        }
    }

    GameObject map_Button1;
    GameObject map_Button2;
    GameObject map_Button3;
    GameObject map_Button4;
    GameObject map_Button5;
    Vector3 v3_FinalPos_BO5_1;
    Vector3 v3_FinalPos_BO3_1;
    Vector3 v3_FinalPos_BO3_2;
    Vector3 v3_FinalPos_BO3_3;
    Vector3 v3_FinalPos_BO5_3;
    void Set_ConfirmedMapFinalPositions( bool b_IsBO3_ )
    {
        if( b_IsBO3_ )
        {
            // Release transforms
            map_Button1.transform.SetParent( GameObject.Find("Canvas").transform );
            map_Button2.transform.SetParent( GameObject.Find("Canvas").transform );
            map_Button3.transform.SetParent( GameObject.Find("Canvas").transform );

            #region Set new positions
            Vector3 v3_Map1 = v3_FinalPos_BO3_1;
            v3_Map1.x += 1750f;
            map_Button1.transform.position = v3_Map1;

            Vector3 v3_Map2 = v3_FinalPos_BO3_2;
            v3_Map2.x += 1750f;
            map_Button2.transform.position = v3_Map2;

            Vector3 v3_Map3 = v3_FinalPos_BO3_3;
            v3_Map3.x += 1750f;
            map_Button3.transform.position = v3_Map3;
            #endregion

            #region Set new sizes
            GameObject go_Temp;
            for(int i_ = 0; i_ < 3; ++i_)
            {
                if (i_ == 0) go_Temp = map_Button1;
                else if (i_ == 1) go_Temp = map_Button2;
                else go_Temp = map_Button3;
                
                go_Temp.GetComponent<RectTransform>().sizeDelta = new Vector2(775, 200);
                go_Temp.transform.FindChild("Img_Picked").GetComponent<RectTransform>().sizeDelta = new Vector2(775, 200);
                go_Temp.transform.FindChild("Text").GetComponent<Text>().fontSize = 135;
                go_Temp.transform.FindChild("Trail").GetComponent<Image>().enabled = true;
            }
            #endregion
        }
        else
        {
            // Release transforms
            map_Button1.transform.SetParent(GameObject.Find("Canvas").transform);
            map_Button2.transform.SetParent(GameObject.Find("Canvas").transform);
            map_Button3.transform.SetParent(GameObject.Find("Canvas").transform);
            map_Button4.transform.SetParent(GameObject.Find("Canvas").transform);
            map_Button5.transform.SetParent(GameObject.Find("Canvas").transform);

            #region Set new positions
            Vector3 v3_Map1 = v3_FinalPos_BO3_1;
            v3_Map1.x += 1750f;
            map_Button1.transform.position = v3_Map1;

            Vector3 v3_Map2 = v3_FinalPos_BO5_1;
            v3_Map2.x += 1750f;
            map_Button2.transform.position = v3_Map2;

            Vector3 v3_Map3 = v3_FinalPos_BO3_2;
            v3_Map3.x += 1750f;
            map_Button3.transform.position = v3_Map3;

            Vector3 v3_Map4 = v3_FinalPos_BO5_3;
            v3_Map4.x += 1750f;
            map_Button4.transform.position = v3_Map4;

            Vector3 v3_Map5 = v3_FinalPos_BO3_3;
            v3_Map3.x += 1750f;
            map_Button5.transform.position = v3_Map3;
            #endregion

            #region Set new sizes
            GameObject go_Temp;
            for (int i_ = 0; i_ < 5; ++i_)
            {
                if (i_ == 0) go_Temp = map_Button1;
                else if (i_ == 1) go_Temp = map_Button2;
                else if (i_ == 2) go_Temp = map_Button3;
                else if (i_ == 3) go_Temp = map_Button4;
                else go_Temp = map_Button5;

                go_Temp.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 200);
                go_Temp.transform.FindChild("Img_Picked").GetComponent<RectTransform>().sizeDelta = new Vector2(600, 200);
                go_Temp.transform.FindChild("Text").GetComponent<Text>().fontSize = 135;
                go_Temp.transform.FindChild("Trail").GetComponent<Image>().enabled = true;
            }
            #endregion
        }
    }

    // Update is called once per frame
    bool b_BeginFinalMapAnimations;
    bool b_PickBanActive = true;
    bool b_SetToSwitch_Left;
    bool b_SetToSwitch_Right;
    bool b_WaitOneFrame;
    bool b_DieActive;
    float f_QuitTimer;
    float f_ProcessBeginTimer = 5.0f;
    float f_LerpSpeed = 0.06f;
    Vector3 v3_OffScreenPos;
    float f_PickBanOver_Timer;
    float f_PickBanOver_Threshold;
    float f_LerpTimer_Picked;
    float f_LerpTimer_Banned;
    [SerializeField] AnimationCurve ac_;
    void Update ()
    {
        RollDie( b_DieActive );
        GameClockVisible();

        // If the PickBan phase is complete
        if( !b_PickBanActive )
        {
            f_PickBanOver_Timer += Time.deltaTime;

            if( f_PickBanOver_Timer >= 3.0f)
            {
                f_LerpTimer_Picked += Time.deltaTime;

                // Begin Lerping the Selected Maps objects
                Vector3 v3_PickedMapsLoc = GameObject.Find("Selected Maps").transform.position;
                v3_PickedMapsLoc = Vector3.Lerp(v3_PickedMapsLoc, v3_OffScreenPos, ac_.Evaluate( f_LerpTimer_Picked / 15.0f ) );
                GameObject.Find("Selected Maps").transform.position = v3_PickedMapsLoc;
            }
            
            if( f_PickBanOver_Timer >= 4.0f)
            {
                f_LerpTimer_Banned += Time.deltaTime;

                // Begin Lerping the Selected Maps objects
                Vector3 v3_BannedMapsLoc = GameObject.Find("Ban Positions").transform.position;
                v3_BannedMapsLoc = Vector3.Lerp(v3_BannedMapsLoc, v3_OffScreenPos, ac_.Evaluate( f_LerpTimer_Banned / 15.0f ) );
                GameObject.Find("Ban Positions").transform.position = v3_BannedMapsLoc;
            }

            if( f_PickBanOver_Timer >= f_PickBanOver_Threshold && !b_BeginFinalMapAnimations )
            {
                Set_ConfirmedMapFinalPositions( b_BestOf3 );
                b_BeginFinalMapAnimations = true;
            }

            #region Lerp in buttons to show final map picks
            if ( f_PickBanOver_Timer >= f_PickBanOver_Threshold + 1 && b_BeginFinalMapAnimations )
            {
                if(b_BestOf3)
                {
                    // Begin lerping to final positions
                    Vector3 v3_LerpPos = map_Button1.transform.position;
                    v3_LerpPos = Vector3.Lerp(v3_LerpPos, v3_FinalPos_BO3_1, 0.05f);
                    map_Button1.transform.position = v3_LerpPos;
                }
            }

            if (f_PickBanOver_Timer >= f_PickBanOver_Threshold + 1.5f && b_BeginFinalMapAnimations)
            {
                if (b_BestOf3)
                {
                    // Begin lerping to final positions
                    Vector3 v3_LerpPos = map_Button2.transform.position;
                    v3_LerpPos = Vector3.Lerp(v3_LerpPos, v3_FinalPos_BO3_2, 0.05f);
                    map_Button2.transform.position = v3_LerpPos;
                }
                else
                {
                    // Begin lerping to final positions
                    Vector3 v3_LerpPos = map_Button2.transform.position;
                    v3_LerpPos = Vector3.Lerp(v3_LerpPos, v3_FinalPos_BO5_1, 0.05f);
                    map_Button2.transform.position = v3_LerpPos;
                }
            }

            if (f_PickBanOver_Timer >= f_PickBanOver_Threshold + 2f && b_BeginFinalMapAnimations)
            {
                if (b_BestOf3)
                {
                    // Begin lerping to final positions
                    Vector3 v3_LerpPos = map_Button3.transform.position;
                    v3_LerpPos = Vector3.Lerp(v3_LerpPos, v3_FinalPos_BO3_3, 0.05f);
                    map_Button3.transform.position = v3_LerpPos;
                }
            }

            if( f_PickBanOver_Timer >= f_PickBanOver_Threshold + 2.5f && b_BeginFinalMapAnimations )
            {
                if (!b_BestOf3)
                {
                    // Begin lerping to final positions
                    Vector3 v3_LerpPos = map_Button4.transform.position;
                    v3_LerpPos = Vector3.Lerp(v3_LerpPos, v3_FinalPos_BO5_3, 0.05f);
                    map_Button4.transform.position = v3_LerpPos;
                }
            }

            if (f_PickBanOver_Timer >= f_PickBanOver_Threshold + 3f && b_BeginFinalMapAnimations)
            {
                if (!b_BestOf3)
                {
                    // Begin lerping to final positions
                    Vector3 v3_LerpPos = map_Button5.transform.position;
                    v3_LerpPos = Vector3.Lerp(v3_LerpPos, v3_FinalPos_BO3_3, 0.05f);
                    map_Button5.transform.position = v3_LerpPos;
                }
            }
            #endregion
        }
        // If the first five seconds have passed
        else if( f_ProcessBeginTimer < 0f && b_PickBanActive )
        {
            #region Lerp the trophy models position
            Vector3 v3_Lerp = go_Trophy.transform.position;
            if(e_TeamTurn == Enum_TeamTurn.Team_A)
            {
                v3_Lerp = Vector3.Lerp(v3_Lerp, v3_TrophyPos_Left, f_LerpSpeed);

                #region Lerp backdrops
                Color clr_Alpha = img_Backdrop_Left.color;
                if(clr_Alpha.a < 1.0f)
                {
                    clr_Alpha.a += Time.deltaTime;

                    if (clr_Alpha.a >= 1.0f) clr_Alpha.a = 1.0f;
                }
                img_Backdrop_Left.color = clr_Alpha;

                clr_Alpha = img_Backdrop_Right.color;
                if(clr_Alpha.a > 0f)
                {
                    clr_Alpha.a -= Time.deltaTime;

                    if(clr_Alpha.a < 0f)
                    {
                        clr_Alpha.a = 0f;

                        if(b_SetToSwitch_Right)
                        {
                            if( img_Backdrop_Right.sprite == img_TeamBackdrop_Ban )
                            {
                                img_Backdrop_Right.sprite = img_TeamBackdrop_Pick;
                            }
                            else
                            {
                                img_Backdrop_Right.sprite = img_TeamBackdrop_Ban;
                            }

                            b_SetToSwitch_Right = false;
                        }
                    }
                }
                img_Backdrop_Right.color = clr_Alpha;
                #endregion
            }
            else if( e_TeamTurn == Enum_TeamTurn.Team_B)
            {
                v3_Lerp = Vector3.Lerp(v3_Lerp, v3_TrophyPos_Right, f_LerpSpeed);

                #region Lerp backdrops
                Color clr_Alpha = img_Backdrop_Right.color;
                if (clr_Alpha.a < 1.0f)
                {
                    clr_Alpha.a += Time.deltaTime;

                    if (clr_Alpha.a >= 1.0f) clr_Alpha.a = 1.0f;
                }
                img_Backdrop_Right.color = clr_Alpha;

                clr_Alpha = img_Backdrop_Left.color;
                if (clr_Alpha.a > 0f)
                {
                    clr_Alpha.a -= Time.deltaTime;

                    if (clr_Alpha.a < 0f)
                    {
                        clr_Alpha.a = 0f;

                        if (b_SetToSwitch_Left)
                        {
                            if (img_Backdrop_Left.sprite == img_TeamBackdrop_Ban)
                            {
                                img_Backdrop_Left.sprite = img_TeamBackdrop_Pick;
                            }
                            else
                            {
                                img_Backdrop_Left.sprite = img_TeamBackdrop_Ban;
                            }

                            b_SetToSwitch_Left = false;
                        }
                    }
                }
                img_Backdrop_Left.color = clr_Alpha;
                #endregion
            }
            else
            {
                v3_Lerp = Vector3.Lerp(v3_Lerp, v3_TrophyPos_Center, f_LerpSpeed);

                #region Lerp backdrops
                Color clr_Alpha = img_Backdrop_Right.color;
                if (clr_Alpha.a > 0f)
                {
                    clr_Alpha.a -= Time.deltaTime;

                    if (clr_Alpha.a < 0f) clr_Alpha.a = 0f;
                }
                img_Backdrop_Right.color = clr_Alpha;

                clr_Alpha = img_Backdrop_Left.color;
                if (clr_Alpha.a > 0f)
                {
                    clr_Alpha.a -= Time.deltaTime;

                    if (clr_Alpha.a < 0f)
                    {
                        clr_Alpha.a = 0f;
                    }
                }
                img_Backdrop_Left.color = clr_Alpha;
                #endregion
            }
            go_Trophy.transform.position = v3_Lerp;
            #endregion

            GameClock();
            
            #region Quit if Escape is double-tapped
            if(f_QuitTimer > 0f)
            {
                if (f_QuitTimer >= 0.5f) f_QuitTimer = -Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); print("WE QUIT"); }

                f_QuitTimer += Time.deltaTime;
            }

            if(Input.GetKeyDown(KeyCode.Escape) && f_QuitTimer == 0f)
            {
                f_QuitTimer += Time.deltaTime;
            }
            #endregion
        }
        else
        {
            f_ProcessBeginTimer -= Time.deltaTime;

            // Disable the mouse cursor input
            GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = false;
            ui_Text_PickBan.enabled = false;

            // Begin the Team Visual process
            if (f_ProcessBeginTimer <= 0f)
            {
                e_TeamTurn = Enum_TeamTurn.Team_A;

                f_LerpSpeed /= 2f;
                
                if(b_PickBanActive)
                {
                    // Disable the mouse cursor input
                    GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = true;
                    ui_Text_PickBan.enabled = true;
                }
            }
        }

        if (!b_WaitOneFrame)
        {
            LoadTeamGraphics();

            b_WaitOneFrame = true;
        }
    }
}
