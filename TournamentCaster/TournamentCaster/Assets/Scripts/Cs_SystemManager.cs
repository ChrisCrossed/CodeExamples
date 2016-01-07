using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum Enum_IconTypes
{
    FirstBlood,
    Dragon,
    Tower,
    Inhib,
    Baron
}

public class Icon
{
    public GameObject go_Icon;
    // public string s_Name;
    public bool b_IsActive;
    public int i_UIPos;
    public bool b_IsTeamLeft;
}

public class Cs_SystemManager : MonoBehaviour
{
    public string TournamentName;
    public bool SmallText;
    public string TeamName_Left;
    public string TeamName_Right;
    public Sprite TournamentLogo;
    public bool NoIcon;

    GameObject Nametag_Left;
    GameObject Nametag_Right;

    Icon Icon_FirstBlood = new Icon();
    Icon Icon_Dragon = new Icon();
    Icon Icon_Tower = new Icon();
    Icon Icon_Baron = new Icon();
    Icon Icon_Inhib = new Icon();

    int i_TeamIcons_Left = 0;
    int i_TeamIcons_Right = 0;

    // Use this for initialization
    void Start ()
    {
        InitializeIcons();
        SetTournamentLogo(TournamentLogo, TournamentName);

        // Nametag_Left = GameObject.Find("Nametag_Left");
        // Nametag_Right = GameObject.Find("Nametag_Right");

        // Nametag_Left.GetComponent<Text>().text = "HH";
    }

    void InitializeIcons()
    {
        // Set Icons
        Icon_FirstBlood.go_Icon = GameObject.Find("Icon_FirstBlood");
        Icon_Dragon.go_Icon = GameObject.Find("Icon_Dragon");
        Icon_Tower.go_Icon = GameObject.Find("Icon_Tower");
        Icon_Baron.go_Icon = GameObject.Find("Icon_Baron");
        Icon_Inhib.go_Icon = GameObject.Find("Icon_Inhib");


        // Turn off Icons
        Icon_FirstBlood.go_Icon.GetComponent<SpriteRenderer>().enabled = false;
        Icon_FirstBlood.b_IsActive = false;

        Icon_Dragon.go_Icon.GetComponent<SpriteRenderer>().enabled = false;
        Icon_Dragon.b_IsActive = false;

        Icon_Tower.go_Icon.GetComponent<SpriteRenderer>().enabled = false;
        Icon_Dragon.b_IsActive = false;

        Icon_Baron.go_Icon.GetComponent<SpriteRenderer>().enabled = false;
        Icon_Dragon.b_IsActive = false;

        Icon_Inhib.go_Icon.GetComponent<SpriteRenderer>().enabled = false;
        Icon_Dragon.b_IsActive = false;
    }

    void SetTournamentLogo(Sprite TournamentLogo_, string TournamentText_)
    {
        GameObject tournamentLogo = GameObject.Find("Overlay_Logo");
        GameObject tournamentText = GameObject.Find("Overlay_Text");

        // Reposition text based on bools
        if (NoIcon)
        {
            tournamentText.transform.localPosition = new Vector3(0, 0, 0);
            tournamentLogo.SetActive(false);
        }

        if(SmallText)
        {
            tournamentText.transform.localScale = new Vector3(0.001f, 0.001f, 1);
        }

        tournamentLogo.GetComponent<SpriteRenderer>().sprite = TournamentLogo_;
        
        if(TournamentText_.Contains("/"))
        {
            TournamentText_ = TournamentText_.Replace("/", "\n");
        }

        tournamentText.GetComponent<TextMesh>().text = TournamentText_;
    }

    // Used by Keyboard Input to apply Icon's to the screen
    void ActivateIcon(Enum_IconTypes iconType_, bool b_IsLeftTeam)
    {
        // Pre-loading generic Icon for manipulation
        Icon currentIcon = Icon_FirstBlood;

        // Load icon to check against
        if (iconType_ == Enum_IconTypes.Baron) currentIcon = Icon_Baron;
        if (iconType_ == Enum_IconTypes.Dragon) currentIcon = Icon_Dragon;
        if (iconType_ == Enum_IconTypes.Tower) currentIcon = Icon_Tower;
        if (iconType_ == Enum_IconTypes.Inhib) currentIcon = Icon_Inhib;

        // If the icon is still disabled...
        if (!currentIcon.b_IsActive)
        {
            // Figure out which position the icon will go in
            if(b_IsLeftTeam)
            {
                // Reposition the icon based on the current i_TeamIcons_Left/Right number
                // 520 - 90 * i_TeamIcons. Right Team *= -1;
                var currPos = currentIcon.go_Icon.gameObject.transform.position;
                currPos.x = -520 + (90 * i_TeamIcons_Left);
                currentIcon.go_Icon.gameObject.transform.position = currPos;

                // Increment the i_TeamIcons
                ++i_TeamIcons_Left;
            }
            else
            {
                var currPos = currentIcon.go_Icon.gameObject.transform.position;
                currPos.x = 520 - (90 * i_TeamIcons_Right);
                currentIcon.go_Icon.gameObject.transform.position = currPos;

                // Increment the i_TeamIcons
                ++i_TeamIcons_Right;
            }
            
            // Enable & place the icon
            currentIcon.go_Icon.GetComponent<SpriteRenderer>().enabled = true;
            currentIcon.b_IsActive = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    // Quit Application (For now)
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Left Team Input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateIcon(Enum_IconTypes.FirstBlood, true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateIcon(Enum_IconTypes.Dragon, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateIcon(Enum_IconTypes.Tower, true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ActivateIcon(Enum_IconTypes.Inhib, true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ActivateIcon(Enum_IconTypes.Baron, true);
        }

        // Right Team Input
        if (Input.GetKeyDown(KeyCode.P))
        {
            ActivateIcon(Enum_IconTypes.FirstBlood, false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ActivateIcon(Enum_IconTypes.Dragon, false);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ActivateIcon(Enum_IconTypes.Tower, false);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ActivateIcon(Enum_IconTypes.Inhib, false);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ActivateIcon(Enum_IconTypes.Baron, false);
        }
    }
}
