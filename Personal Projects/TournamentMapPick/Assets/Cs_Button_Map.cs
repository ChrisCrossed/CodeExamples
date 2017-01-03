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
        overlaySystem.MapClicked( e_MapType );
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
