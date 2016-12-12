using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cs_LevelManager : MonoBehaviour
{
    bool b_Time_AM;
    int i_Time_Hours;
    int i_Time_Minutes;
    float f_Time_Seconds;

    Text txt_ClockText;
    TextMesh txt_TimeClockText;

	// Use this for initialization
	void Start ()
    {
        // Connect items
        txt_ClockText = GameObject.Find("Text_Clock").GetComponent<Text>();
        txt_TimeClockText = GameObject.Find("PunchClockText").GetComponent<TextMesh>();

        // 8 AM, start of work day
        i_Time_Hours = 7;
        i_Time_Minutes = 45;
        b_Time_AM = true;
	}
	
    void ManageClock()
    {
        f_Time_Seconds += Time.deltaTime;

        if(f_Time_Seconds > 1.5f)
        {
            ++i_Time_Minutes;
            f_Time_Seconds = 0f;

            if(i_Time_Minutes >= 60)
            {
                i_Time_Minutes = 0;

                ++i_Time_Hours;

                if(i_Time_Hours > 12)
                {
                    i_Time_Hours = 1;

                    b_Time_AM = !b_Time_AM;
                }
            }

            txt_ClockText.text = string.Format("{0:0}:{1:00}", i_Time_Hours, i_Time_Minutes);
            txt_TimeClockText.text = string.Format("{0:0}:{1:00}", i_Time_Hours, i_Time_Minutes);
        }
    }

    public int GetTime_Minutes
    {
        get { return i_Time_Minutes; }
    }

    public int GetTime_Hours
    {
        get { return i_Time_Hours; }
    }

	// Update is called once per frame
	void Update ()
    {
        ManageClock();
	}
}
