using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Enum_MapList
{
    Assault_Hanamura = 0,
    Assault_TempleOfAnubis = 1,
    Assault_Volskaya = 2,

    Escort_Dorado = 3,
    Escort_Gibraltar = 4,
    Escort_Route66 = 5,

    Hybrid_Hollywood = 6,
    Hybrid_KingsRow = 7,
    Hybrid_Numbani = 8,
    Hybrid_Eichenwalde = 9,

    Control_Ilios = 10,
    Control_Lijang = 11,
    Control_Nepal = 12,
    Control_Oasis = 13
}

public enum Enum_MapState
{
    Default = 0,
    Banned = 1,
    Selected = 2
}

public class Cs_OverlaySystem : MonoBehaviour
{
    Camera this_Camera;

    bool[] b_MapActive = new bool[14];

	// Use this for initialization
	void Start ()
    {
        // this_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
    
    public void MapClicked( Enum_MapList e_MapList_ )
    {
        print("Connected");

        // Disable map from list

        // Tell map to move to proper position

        // Update PickBan
    }

    void SetMapState( Enum_MapList e_MapList_, bool b_IsActive_ = false )
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
