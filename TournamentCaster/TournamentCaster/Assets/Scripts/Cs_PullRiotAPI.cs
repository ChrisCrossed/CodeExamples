using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;

/*******************************************************************************
filename    Cs_PullRiotAPI.cpp
author      Chris Christensen
email       ChrisCrossed.86@Gmail.com

Brief Description:
  This program grabs various Riot API information when necessary.
  
*******************************************************************************/

enum RegionLocations
{
    NA, EUW, BR, EUNE, KR, LAN, LAS, OCE, RU, TR
}

public class Cs_PullRiotAPI : MonoBehaviour
{
    // Match information (Holds the info to files)
    int i_MatchID;
    RegionLocations enum_RegionLocation;
    string s_SaveAPIInfo;

    // Caster Information
    string s_CasterAPIKey;
    string s_CasterUsername;
    int i_CasterID;

    WWW www_ApiRequest;
    bool b_IsDone = false;

    // Red Team Information

    // Blue Team Information

    // Use this for initialization
    void Start()
    {
        // Forcing in my own key to test. MUST replace later.
        s_CasterAPIKey = "79ba48bc-e49a-4a64-a3ee-55ac3d012c24";
        s_CasterUsername = "ChrisCrossed";
        enum_RegionLocation = RegionLocations.NA;

        // Before each match, find my summonerID based off my username.
        // int i_SummID_By_Username = GetSummonerIDByName(s_CasterUsername);
        DownloadAPI_Summoner_1_4(RegionLocations.NA, "ChrisCrossed");
    }

    /*******************************************************************************

        Function:   DownloadAPI_Summoner_1_4
     Description:   Downloads the Summoner 1.4 API. For use when finding SummonerID

          Inputs:   enum_RegionLocation_ (RegionLocation) - World Location
                    s_SummonerName_ (String) - The Username being searched for

         Outputs:   i_SummonerID (Int) - The number Riot provides for their username

    *******************************************************************************/
    void DownloadAPI_Summoner_1_4(RegionLocations enum_RegionLocation_, string s_SummonerName_)
    {
        // Reference Link:
        // https://na.api.pvp.net/api/lol/na/v1.4/summoner/by-name/ChrisCrossed?api_key=79ba48bc-e49a-4a64-a3ee-55ac3d012c24

        // Create the proper website link
        string s_LinkURL = "https://" +
                            enum_RegionLocation_.ToString() +
                            ".api.pvp.net/api/lol/" +
                            enum_RegionLocation_.ToString() +
                            "/v1.4/summoner/by-name/" +
                            s_SummonerName_.ToString() + 
                            "?api_key=" + s_CasterAPIKey;

        APIRequest(s_LinkURL, true, TEST_GetSummonerID);
    }
    
    void TEST_GetSummonerID(string s_SaveAPIInfo_)
    {
        // Store it so I can mess with it
        string s_SaveAPIInfo = s_SaveAPIInfo_;

        if(s_SaveAPIInfo.Contains("\"ID\":"))
        {
            print("True");
        }
    }

    /*******************************************************************************

        Function:   APIRequest
     Description:   Sends out the request to the internet for information.
                    Is run by Update() every frame to check when a request completes.

          Inputs:   s_WebAPILink_ (String) - The website request in string form
                    b_IsNewRequest_ (Bool) - Resets the Request
                    OutputFunction_ (Function) - The function to call when complete

         Outputs:   None

    *******************************************************************************/
    void APIRequest(string s_WebAPILink_, bool b_IsNewRequest_, Action<string> OutputFunction_)
    {
        if(b_IsNewRequest_)
        {
            // This sends out the API Info online. www_ApiRequest.IsDone states if complete.
            www_ApiRequest = new WWW(s_WebAPILink_);

            // States that a request has been made. Turns off later so I don't keep requesting
            b_IsDone = true;
        }
        else
        {
            if (www_ApiRequest.isDone && b_IsDone)
            {
                s_SaveAPIInfo = www_ApiRequest.text;
                // Doesn't work yet
                // OutputFunction_(s_SaveAPIInfo);
                
                b_IsDone = false;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        APIRequest(null, false, null);
    }
}
