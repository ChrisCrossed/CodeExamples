using UnityEngine;
using System.Collections;

public class Cs_ControlPanel : MonoBehaviour
{
    public bool b_IsControlPanel;

    GameObject[] go_Buttons;

	// Use this for initialization
	void Start ()
    {
        InitializeButtons();
	}

    void InitializeButtons()
    {
        go_Buttons = new GameObject[16];

        for(int i_ = 0; i_ < 16; ++i_)
        {
            go_Buttons[i_] = gameObject.transform.Find(i_.ToString()).gameObject;
        }
    }

    public void SetLight(int i_Pos_, bool b_IsOn)
    {
        if (i_Pos_ >= 16) i_Pos_ = 15;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
