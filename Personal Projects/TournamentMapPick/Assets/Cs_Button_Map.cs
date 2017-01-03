using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Button_Map : MonoBehaviour
{
    [SerializeField] Enum_MapList e_MapType;
    Cs_OverlaySystem overlaySystem;

	// Use this for initialization
	void Start ()
    {
        overlaySystem = GameObject.Find("Canvas").GetComponent<Cs_OverlaySystem>();
	}

    public void ClickButton()
    {
        overlaySystem.MapClicked( gameObject );
    }

    public Enum_MapList MapType
    {
        get { return e_MapType; }
    }

    bool b_IsActivated;
    float f_MoveTimer;
    RectTransform FinalPosition;
    public void GoToPosition( RectTransform pos_ )
    {
        FinalPosition = pos_;
        b_IsActivated = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		// If button was pressed, then begin lerping to the final position
        if( b_IsActivated )
        {

        }
	}
}
