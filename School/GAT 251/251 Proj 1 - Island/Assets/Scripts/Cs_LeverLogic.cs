using UnityEngine;
using System.Collections;

public class Cs_LeverLogic : MonoBehaviour
{
    float f_ButtonTimer;
    float f_MAX_BUTTON_TIMER = 4.0f;

    float f_ButtonModelTimer;
    GameObject go_ButtonModel;
    
    Material mat_FailSuccess;

    [SerializeField]
    GameObject go_Door;

    public GameObject go_CorrectAnswerSource;
    public GameObject go_ControlPanel;
    bool[] b_LightArray = new bool[16];
    bool[] b_CorrectAnswer = new bool[16];

    [SerializeField]
    Material mat_SuccessFail;
    Color color_Fail;
    Color color_Success;
    bool b_ChangeColor;


    // Use this for initialization
    void Start ()
    {
        f_ButtonTimer = f_MAX_BUTTON_TIMER;

        for(int i_ = 0; i_ < 16; ++i_)
        {
            b_CorrectAnswer[i_] = false;
        }

        SetCorrectAnswer();

        #region Set Colors
        color_Fail = new Color(1, (float)(100 / 255), 0);
        color_Success = new Color((float)100 / 255, 1, 0);

        if (mat_SuccessFail != null) mat_SuccessFail.color = color_Fail;
        #endregion
    }

    void SetCorrectAnswer()
    {
        if ( go_CorrectAnswerSource.GetComponent<Cs_ControlPanel>() )
        {
            b_CorrectAnswer = go_CorrectAnswerSource.GetComponent<Cs_ControlPanel>().GetBoolArray();
        }
        else if( go_CorrectAnswerSource.GetComponent<Cs_HintButtonLogic>() )
        {
            b_CorrectAnswer = go_CorrectAnswerSource.GetComponent<Cs_HintButtonLogic>().GetBoolArray();
        }
        else
        {
            print("No Correct Answer Set");
        }

        string str_Temp = "";

        for(int i_ = 0; i_ < 16; ++i_)
        {
            if(i_ != 16) str_Temp += b_CorrectAnswer[i_].ToString() + ", ";
            else str_Temp += b_CorrectAnswer[i_].ToString() + ".";
        }

        print(gameObject.name + " got answer: " + str_Temp);
    }

    public void UseButton()
    {
        if (f_ButtonTimer == f_MAX_BUTTON_TIMER)
        {
            f_ButtonModelTimer = 2.0f;

            f_ButtonTimer = 0.0f;

            // b_LightArray = go_ControlPanel.GetComponent<Cs_ControlPanel>().GetBoolArray();

            if (CheckCorrectAnswer())
            {
                go_Door.GetComponent<Cs_Door>().MoveDoor();
            }
            else
            {
                string str_Temp = "";

                for (int i_ = 0; i_ < 16; ++i_)
                {
                    if (i_ != 16) str_Temp += b_CorrectAnswer[i_].ToString() + ", ";
                    else str_Temp += b_CorrectAnswer[i_].ToString() + ".";
                }

                print(gameObject.name + " got answer: " + str_Temp);

                str_Temp = "";

                for (int i_ = 0; i_ < 16; ++i_)
                {
                    if (i_ != 16) str_Temp += b_LightArray[i_].ToString() + ", ";
                    else str_Temp += b_LightArray[i_].ToString() + ".";
                }

                print("Control Panel had answer: " + str_Temp);
            }
        }
    }

    bool CheckCorrectAnswer()
    {
        for(int i_ = 0; i_ < 16; ++i_)
        {
            if (b_LightArray[i_] != b_CorrectAnswer[i_]) return false;
        }

        return true;
    }

    void UpdateButtonModel()
    {
        if (f_ButtonTimer < f_MAX_BUTTON_TIMER)
        {
            f_ButtonTimer += Time.deltaTime;

            if (f_ButtonTimer > f_MAX_BUTTON_TIMER) f_ButtonTimer = f_MAX_BUTTON_TIMER;
        }

        // Button Model Movement
        if (f_ButtonModelTimer >= 1.25f)
        {
            float f_LerpAngle = Mathf.LerpAngle(gameObject.transform.eulerAngles.z, gameObject.transform.eulerAngles.z - 15f, 0.05f);
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, f_LerpAngle);
        }
        else if (f_ButtonModelTimer >= 0.5f)
        {
            float f_LerpAngle = Mathf.LerpAngle(gameObject.transform.eulerAngles.z, gameObject.transform.eulerAngles.z + 15f, 0.05f);

            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, f_LerpAngle);
        }

        if (f_ButtonModelTimer != 0.0f)
        {
            f_ButtonModelTimer -= Time.deltaTime;

            if (f_ButtonModelTimer < 0.0f) f_ButtonModelTimer = 0.0f;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateButtonModel();

        if (Input.GetKeyDown(KeyCode.C)) UseButton();
	}
}
