using UnityEngine;
using System.Collections;

public struct TournamentInfo
{
    public string s_BluTeam;
    public string s_RedTeam;
    public string s_EventName;
    public bool b_EventTwoLines;
    public bool b_ShowLogo;
}

public class Cs_TournamentInfo : MonoBehaviour
{
    TournamentInfo tourneyInfo = new TournamentInfo();

	// Use this for initialization
	public void SetTournamentInfo(string s_BluTeam_, string s_RedTeam_, string s_EventName_, bool b_EventTwoLines_, bool b_ShowLogo_)
    {
        tourneyInfo.s_BluTeam = s_BluTeam_;
        tourneyInfo.s_RedTeam = s_RedTeam_;
        tourneyInfo.s_EventName = s_EventName_;
        tourneyInfo.b_EventTwoLines = b_EventTwoLines_;
        tourneyInfo.b_ShowLogo = b_ShowLogo_;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        if(GameObject.Find("SystemManager"))
        {
            GameObject.Find("SystemManager").GetComponent<Cs_SystemManager>().InitializeGame(tourneyInfo);
        }
    }
}
