using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cs_ObjectiveManager : MonoBehaviour
{
    int i_NumTasks = 1;

    Text txt_JobList_1_Text;
    Text txt_JobList_2_Text;
    Text txt_JobList_3_Text;
    Text txt_JobList_4_Text;
    Text txt_JobList_5_Text;

    string[] s_JobList = new string[15];

    #region Task - 'Boss Kick Me'
    bool b_Job_BossKickMe; // 0
    GameObject go_BossSign;
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

        CreateNewJob();

        Set_TaskText();
    }

    void CreateNewJob()
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
                    if (!b_Job_BossKickMe)
                    {
                        Init_BossKickMe();
                        b_JobFound = true;
                    }
                    break;
            }
        }
    }

    int i_BossKickMe_Number;
    string s_BossKickMe_String;
    void Init_BossKickMe()
    {
        b_Job_BossKickMe = true;
        go_BossSign.GetComponent<Cs_Objective>().Set_State = Enum_ObjectiveState.InProgress;

        s_BossKickMe_String = "Put Sign on Boss's Back";
        Set_TaskText(s_BossKickMe_String);
    }
    public void Complete_BossKickMe()
    {
        if(b_Job_BossKickMe)
        {
            b_Job_BossKickMe = false;
            s_JobList[ i_BossKickMe_Number ] = "";
            Set_TaskText();
        }
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
            if (s_JobList[i_] == s_BossKickMe_String) i_BossKickMe_Number = i_;
        }

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
	    if(Input.GetKeyDown(KeyCode.O))
        {
            go_BossSign.GetComponent<Cs_Objective>().Set_State = Enum_ObjectiveState.Disabled;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Init_BossKickMe();
        }
	}
}
