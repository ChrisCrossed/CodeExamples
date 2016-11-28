using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cs_Intro_TextLogic : MonoBehaviour
{
    float f_FadeTimer;
    [SerializeField] float f_FadeTimer_Start;
    [SerializeField] float f_FadeTimer_End;
    float f_TotalFadeTime;

    float f_MaxSceneTime;

    // Use this for initialization
    void Start ()
    {
        // Set Alpha to 0
        Color clr_CurrColor = gameObject.GetComponent<Text>().color;
        clr_CurrColor.a = 0f;
        gameObject.GetComponent<Text>().color = clr_CurrColor;

        f_TotalFadeTime = f_FadeTimer_End - f_FadeTimer_Start;

        f_MaxSceneTime = GameObject.Find("Canvas").GetComponent<Cs_IntroScreenLogic>().Get_SceneMaxTime();
    }
	
	// Update is called once per frame
	void Update ()
    {
        f_FadeTimer += Time.deltaTime;

        if(f_FadeTimer >= f_FadeTimer_Start && f_FadeTimer < f_FadeTimer_End)
        {
            Color clr_CurrColor = gameObject.GetComponent<Text>().color;

            float f_Min = f_FadeTimer_Start;
            float f_Max = f_Min + f_TotalFadeTime;

            // Lerp from 0 to 1
            float f_Perc = Mathf.Lerp(f_Min, f_Max, f_Max - f_FadeTimer);

            print(f_Perc);

            clr_CurrColor.a = f_Perc;

            gameObject.GetComponent<Text>().color = clr_CurrColor;
        }

        //if(f_FadeTimer >= f_FadeTimer_Start && f_FadeTimer <= f_FadeTimer_End)
        //{
        //    float f_Perc = (f_FadeTimer_End - f_FadeTimer) / (f_FadeTimer_End - f_FadeTimer_Start);

        //    if (f_Perc > 1.0f) f_Perc = 1.0f;
        //    else if (f_Perc < 0.0f) f_Perc = 0.0f;


        //}
	}
}
