using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cs_SystemManager : MonoBehaviour
{
    GameObject Nametag_Left;
    GameObject Nametag_Right;

    public Sprite TournamentLogo;

	// Use this for initialization
	void Start ()
    {
        // Nametag_Left = GameObject.Find("Nametag_Left");
        // Nametag_Right = GameObject.Find("Nametag_Right");

        // Nametag_Left.GetComponent<Text>().text = "HH";
    }

    void SetTournamentLogo(Sprite TournamentLogo_, string TournamentText_)
    {
        GameObject tournamentLogo = GameObject.Find("Overlay_Logo");
        GameObject tournamentText = GameObject.Find("Overlay_Text");
    }
	
	// Update is called once per frame
	void Update ()
    {
	    // Quit Application (For now)
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
