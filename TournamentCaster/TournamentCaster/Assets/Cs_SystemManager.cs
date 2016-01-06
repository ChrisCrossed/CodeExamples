using UnityEngine;
using System.Collections;

public class Cs_SystemManager : MonoBehaviour
{
    GameObject Nametag_Left;
    GameObject Nametag_Right;

	// Use this for initialization
	void Start ()
    {
        Nametag_Left = GameObject.Find("Nametag_Left");
        Nametag_Right = GameObject.Find("Nametag_Right");
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
