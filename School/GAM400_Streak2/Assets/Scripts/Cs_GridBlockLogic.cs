using UnityEngine;
using System.Collections;

public enum Enum_FadeState
{
    FadeIn,
    Stay,
    FadeOut
}

public class Cs_GridBlockLogic : MonoBehaviour
{
    float f_FadeTimer = 0.0f;
    float f_FadeTimer_Max = 0.5f;
    float f_FadePercent = 0.0f;
    
    Enum_FadeState e_FadeState;

	// Use this for initialization
	void Start ()
    {
        Color clr_CurrMat = gameObject.GetComponent<MeshRenderer>().material.color;
        clr_CurrMat.a = 0;
        gameObject.GetComponent<MeshRenderer>().material.color = clr_CurrMat;

        Set_FadeState(Enum_FadeState.Stay);
    }

    public void Set_FadeState( Enum_FadeState e_FadeState_ )
    {
        e_FadeState = e_FadeState_;
    }

    void UpdateFadeTimers()
    {
        if (e_FadeState == Enum_FadeState.FadeIn)
        {
            // Increment Fade Timer
            if (f_FadeTimer < f_FadeTimer_Max)
            {
                f_FadeTimer += Time.deltaTime;

                if (f_FadeTimer > f_FadeTimer_Max) f_FadeTimer = f_FadeTimer_Max;
            }

            // Set Fade Percent
            f_FadePercent = f_FadeTimer / f_FadeTimer_Max;

            if (f_FadePercent == 1.0f)
            {
                e_FadeState = Enum_FadeState.Stay;
            }
        }
        else if (e_FadeState == Enum_FadeState.FadeOut)
        {
            // Decrement Fade Timer
            if (f_FadeTimer > 0)
            {
                f_FadeTimer -= Time.deltaTime;

                if (f_FadeTimer < 0) f_FadeTimer = 0;
            }

            // Set Fade Percent
            f_FadePercent = f_FadeTimer / f_FadeTimer_Max;

            if (f_FadePercent == 0f) e_FadeState = Enum_FadeState.Stay;
        }
    }

    void UpdateBlockAlpha()
    {
        if(e_FadeState == Enum_FadeState.FadeIn || e_FadeState == Enum_FadeState.FadeOut)
        {
            Color clr_CurrMat = gameObject.GetComponent<MeshRenderer>().material.color;
            clr_CurrMat.a = f_FadePercent;
            gameObject.GetComponent<MeshRenderer>().material.color = clr_CurrMat;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateBlockAlpha();

        UpdateFadeTimers();
	}
}
