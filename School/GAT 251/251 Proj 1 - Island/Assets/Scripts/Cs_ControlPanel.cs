﻿using UnityEngine;
using System.Collections;

public class Cs_ControlPanel : MonoBehaviour
{
    public bool b_IsControlPanel;

    bool[] b_Buttons;

	// Use this for initialization
	void Start ()
    {
        InitializeButtons();

        if (!b_IsControlPanel) DisableButtonRaycast();

        b_Buttons = new bool[16];
	}

    void InitializeButtons()
    {
        for(int i_ = 0; i_ < 16; ++i_)
        {
            // Find each child button and set them to 'off'
            gameObject.transform.Find(i_.ToString()).GetComponent<Cs_ButtonLogic>().SetActive(true);
        }
    }

    public void SetLight(int i_Pos_, bool b_IsOn_)
    {
        if (i_Pos_ >= 16) i_Pos_ = 15;
        if (i_Pos_ <= -1) i_Pos_ = 0;

        SetLight(i_Pos_.ToString(), b_IsOn_);
    }

    public void SetLight(string s_Pos_, bool b_IsOn_)
    {
        gameObject.transform.Find(s_Pos_).GetComponent<Cs_ButtonLogic>().SetActive(b_IsOn_);
    }

    void DisableButtonRaycast()
    {
        for (int i_ = 0; i_ < 16; ++i_)
        {
            // Find each child button and set them to 'off'
            // gameObject.transform.Find(i_.ToString()).tag = "Untagged";
            gameObject.transform.Find(i_.ToString()).gameObject.layer = 2;
        }
    }

	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
