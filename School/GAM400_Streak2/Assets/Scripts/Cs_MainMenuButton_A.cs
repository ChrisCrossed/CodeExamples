using UnityEngine;
using System.Collections;

public enum Enum_MenuButtonState
{
    FadeOut,
    FadeIn,
    Highlighted
}

public class Cs_MainMenuButton_A : MonoBehaviour
{
    Enum_MenuButtonState e_MenuButtonState;
    float f_MoveSpeed = 1.0f;

    float f_xPos_Center;
    float f_xPos_Left;

	// Use this for initialization
	void Start ()
    {
        e_MenuButtonState = Enum_MenuButtonState.FadeIn;
    }

    public void Set_FoldLeft()
    {

    }

    public void Set_FoldCenter()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
