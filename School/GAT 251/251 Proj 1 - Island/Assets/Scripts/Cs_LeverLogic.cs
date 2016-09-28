using UnityEngine;
using System.Collections;

public class Cs_LeverLogic : MonoBehaviour
{
    float f_ButtonTimer;
    float f_MAX_BUTTON_TIMER = 4.0f;

    float f_ButtonModelTimer;
    GameObject go_ButtonModel;

    public GameObject go_ControlPanel;
    bool[] b_LightArray = new bool[16];

    bool[] b_CorrectAnswer = new bool[16];

    // Use this for initialization
    void Start ()
    {
        f_ButtonTimer = f_MAX_BUTTON_TIMER;

        for(int i_ = 0; i_ < 16; ++i_)
        {
            b_CorrectAnswer[i_] = false;
        }

        SetCorrectAnswer();
    }

    void SetCorrectAnswer()
    {
        b_CorrectAnswer[0] = true;
        b_CorrectAnswer[1] = true;
        b_CorrectAnswer[2] = true;

        b_CorrectAnswer[4] = true;
        b_CorrectAnswer[6] = true;
        b_CorrectAnswer[7] = true;

        b_CorrectAnswer[12] = true;
        b_CorrectAnswer[14] = true;
    }

    public void UseButton()
    {
        if (f_ButtonTimer == f_MAX_BUTTON_TIMER)
        {
            f_ButtonModelTimer = 2.0f;

            f_ButtonTimer = 0.0f;

            b_LightArray = go_ControlPanel.GetComponent<Cs_ControlPanel>().GetBoolArray();

            if (CheckCorrectAnswer())
            {
                print("Success!");
            }
            else print("Fail...");
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
            Vector3 v3_newPos = gameObject.transform.eulerAngles;

            gameObject.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(v3_newPos.z, -30f, 0.05f));
        }
        else if (f_ButtonModelTimer >= 0.5f)
        {
            Vector3 v3_newPos = gameObject.transform.eulerAngles;

            gameObject.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(v3_newPos.z, 0f, 0.05f));
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
