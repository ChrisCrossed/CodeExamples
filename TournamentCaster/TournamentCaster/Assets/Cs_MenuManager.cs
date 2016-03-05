using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cs_MenuManager : MonoBehaviour
{
    string s_BluTeam;
    string s_RedTeam;
    string s_EventName;
    bool b_EventTwoLines;
    bool b_ShowLogo;

	// Use this for initialization
	void Start ()
    {
        // Sets defaults
        s_BluTeam = "NAM";
        s_RedTeam = "NAM";
        s_EventName = "ONE/TWO";
        b_EventTwoLines = true;
        b_ShowLogo = true;

        // Sets event listeners to take the text input and send them to their various functions
        GameObject.Find("BlueTeamName").GetComponent<InputField>().onValueChanged.AddListener(SetBlueTeamName);
        GameObject.Find("RedTeamName").GetComponent<InputField>().onValueChanged.AddListener(SetRedTeamName);
        GameObject.Find("EventName").GetComponent<InputField>().onValueChanged.AddListener(SetEventName);
        GameObject.Find("EventButton").GetComponent<Button>().onClick.AddListener(SetNumberLines);
        GameObject.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener(SetShowLogo);
        GameObject.Find("GoButton").GetComponent<Button>().onClick.AddListener(StartTournament);
    }

    public void SetBlueTeamName(string s_BluTeam_)
    {
        s_BluTeam = s_BluTeam_;

        print("Blue Team: " + s_BluTeam);
    }

    public void SetRedTeamName(string s_RedTeam_)
    {
        s_RedTeam = s_RedTeam_;

        print("Red Team: " + s_RedTeam);
    }

    public void SetEventName(string s_EventName_)
    {
        s_EventName = s_EventName_;

        print("Event Name: " + s_EventName);
    }

    public void SetNumberLines()
    {
        b_EventTwoLines = !b_EventTwoLines;

        if (b_EventTwoLines)
        {
            print("Two Lines");

            GameObject.Find("EventButton").GetComponentInChildren<Text>().text = "Event Name is 2 Lines";
        }
        else
        {
            print("Three Lines");

            GameObject.Find("EventButton").GetComponentInChildren<Text>().text = "Event Name is 3 Lines";
        }
    }

    public void SetShowLogo(bool b_ShowLogo_)
    {
        b_ShowLogo = b_ShowLogo_;

        if (b_ShowLogo)
        {
            print("Show Logo");
        }
        else
        {
            print("No Logo");
        }
    }

    public void StartTournament()
    {
        // Find GameObject: Apply the Initialization
        GameObject.Find("TournamentOptions").GetComponent<Cs_TournamentInfo>().SetTournamentInfo(s_BluTeam, s_RedTeam, s_EventName, b_EventTwoLines, b_ShowLogo);

        // Change levels to the Tournament backdrop
        SceneManager.LoadScene("Caster");
    }
}