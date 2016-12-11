using UnityEngine;
using System.Collections;

public class Cs_LevelManager : MonoBehaviour
{
    bool b_Time_AM;
    int i_Time_Hours;
    int i_Time_Minutes;
    float f_Time_Seconds;

	// Use this for initialization
	void Start ()
    {
        // 8 AM, start of work day
        i_Time_Hours = 8;
        i_Time_Minutes = 0;
        b_Time_AM = true;
	}
	
    void ManageClock()
    {
        f_Time_Seconds += Time.deltaTime;

        if(f_Time_Seconds > 1f)
        {
            ++i_Time_Minutes;

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
