using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cs_ObjectiveManager : MonoBehaviour
{
    bool b_ClockedIn;

    int i_NumTasks = 2;

    Text txt_JobList_1_Text;
    Text txt_JobList_2_Text;
    Text txt_JobList_3_Text;
    Text txt_JobList_4_Text;
    Text txt_JobList_5_Text;

    string[] s_JobList = new string[15];
    string s_TurnInJob = "[TURN IN]";

    #region Task - 'Boss Kick Me'
    bool b_Job_BossKickMe;          // 0
    GameObject go_BossSign;
    #endregion

    #region Task - 'Change Radio Station'
    bool b_Job_ChangeRadioStation;  // 1
    [SerializeField] GameObject[] go_RadioList;
    #endregion

    #region Task - 'Punch In'
    bool b_Job_PunchIn;
    GameObject go_PunchInClock;
    #endregion

    // Use this for initialization
    void Start ()
    {
        #region Initialize HUD Text
        txt_JobList_1_Text = GameObject.Find("JobList_1_Text").GetComponent<Text>();
        txt_JobList_2_Text = GameObject.Find("JobList_2_Text").GetComponent<Text>();
        txt_JobList_3_Text = GameObject.Find("JobList_3_Text").GetComponent<Text>();
        txt_JobList_4_Text = GameObject.Find("JobList_4_Text").GetComponent<Text>();
        txt_JobList_5_Text = GameObject.Find("JobList_5_Text").GetComponent<Text>();
        #endregion

        #region Task - 'Boss Kick Me'
        go_BossSign = GameObject.Find("BackMessage");
        #endregion

        #region Task - 'Punch In'
        go_PunchInClock = GameObject.Find("PunchInClock");
        #endregion

        // CreateNewJob();
        Init_PunchIn();

        Set_TaskText();
    }

    public void CreateNewJob()
    {
        bool b_JobFound = false;

        while(!b_JobFound)
        {
            int i_JobNumber = Random.Range(0, i_NumTasks);

            switch(i_JobNumber)
            {
                // Boss Kick Me Sign
                case 0:
                    if(!b_Job_BossKickMe)
                    {
                        Init_BossKickMe();
                        b_JobFound = true;
                    }
                    break;

                case 1:
                    if (!b_Job_ChangeRadioStation)
                    {
                        Init_ChangeRadioStation();
                        b_JobFound = true;
                    }
                    break;
            }
        }
    }

    public bool ClockIn
    {
        set { b_ClockedIn = value; }
        get { return b_ClockedIn; }
    }

    #region Punch In
    int i_PunchIn_Number = -1;
    string s_PunchIn_Text = "Punch In Comrade!";
    void Init_PunchIn()
    {
        if(!b_Job_PunchIn)
        {
            b_Job_PunchIn = true;

            go_PunchInClock.GetComponent<Cs_Objective>().Set_State = Enum_ObjectiveState.InProgress;

            Set_TaskText(s_PunchIn_Text);
        }
    }
    public void Complete_PunchIn()
    {
        if (b_Job_PunchIn)
        {
            b_Job_PunchIn = false;
            if (i_PunchIn_Number >= 0) s_JobList[ i_PunchIn_Number ] = s_TurnInJob;
            Set_TaskText();
            i_PunchIn_Number = -1;

            ClockIn = true;
        }
    }
    #endregion

    #region 'Kick Me' On Boss
    int i_BossKickMe_Number = -1;
    string s_BossKickMe_Text = "Put Sign on Boss's Back";
    void Init_BossKickMe()
    {
        b_Job_BossKickMe = true;
        go_BossSign.GetComponent<Cs_Objective>().Set_State = Enum_ObjectiveState.InProgress;
        
        Set_TaskText(s_BossKickMe_Text);
    }
    public void Complete_BossKickMe()
    {
        if(b_Job_BossKickMe)
        {
            b_Job_BossKickMe = false;
            if(i_BossKickMe_Number >= 0) s_JobList[ i_BossKickMe_Number ] = s_TurnInJob;
            Set_TaskText();
            i_BossKickMe_Number = -1;
        }
    }
    #endregion

    #region Change Radio Station
    int i_ChangeRadioStation_Number = -1;
    string s_ChangeRadioStation_Text = "Play Union\nAppropriate Music";
    void Init_ChangeRadioStation()
    {
        b_Job_ChangeRadioStation = true;
        
        // Loop through all radios and reset them
        for(int i_ = 0; i_ < go_RadioList.Length; ++i_)
        {
            go_RadioList[i_].GetComponent<Cs_RadioLogic>().Set_ResetRadio( true );
        }
        
        Set_TaskText(s_ChangeRadioStation_Text);
    }
    public void Complete_ChangeRadioStation()
    {
        if(b_Job_ChangeRadioStation)
        {
            b_Job_ChangeRadioStation = false;
            if (i_ChangeRadioStation_Number >= 0) s_JobList[ i_ChangeRadioStation_Number ] = s_TurnInJob;
            Set_TaskText();
            i_ChangeRadioStation_Number = -1;
        }
    }
    #endregion

    public void Set_TurnInTasks()
    {
        for(int i_ = 0; i_ < s_JobList.Length; ++i_)
        {
            if(s_JobList[i_] == s_TurnInJob)
            {
                s_JobList[i_] = "";
            }
        }

        Set_TaskText();
    }

    void Set_TaskText( string s_Text_ = "")
    {
        // Bubble sort text
        for(int i_ = 0; i_ < s_JobList.Length; ++i_)
        {
            if (s_JobList[i_] == null) s_JobList[i_] = "";
            else if(s_JobList[i_] == "")
            {
                for(int j_ = i_ + 1; j_ < s_JobList.Length; ++j_)
                {
                    if(s_JobList[j_] != "" || s_JobList[j_] == null)
                    {
                        s_JobList[i_] = s_JobList[j_];
                        s_JobList[j_] = "";
                        break;
                    }
                }
            }
        }
        
        // Run through the text list to find the first open position. Assign text there.
        for(int i_ = 0; i_ < s_JobList.Length; ++i_)
        {
            if (s_JobList[i_] == "")
            {
                s_JobList[i_] = s_Text_;
                break;
            }
        }

        // Store new text positions for reference
        for(int i_ = 0; i_ < s_JobList.Length; ++i_)
        {
            if (s_JobList[i_] == s_BossKickMe_Text) i_BossKickMe_Number = i_;
            else if (s_JobList[i_] == s_ChangeRadioStation_Text) i_ChangeRadioStation_Number = i_;
            else if (s_JobList[i_] == s_PunchIn_Text) i_PunchIn_Number = i_;
        }

        // print("Kick Me: " + i_BossKickMe_Number + ", Radio: " + i_ChangeRadioStation_Number);

        // Set text on screen
        txt_JobList_1_Text.text = s_JobList[0];
        txt_JobList_2_Text.text = s_JobList[1];
        txt_JobList_3_Text.text = s_JobList[2];
        txt_JobList_4_Text.text = s_JobList[3];
        txt_JobList_5_Text.text = s_JobList[4];
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.U))
        {
            go_BossSign.GetComponent<Cs_Objective>().Set_State = Enum_ObjectiveState.Disabled;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Init_ChangeRadioStation();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Init_BossKickMe();
        }
	}
}
