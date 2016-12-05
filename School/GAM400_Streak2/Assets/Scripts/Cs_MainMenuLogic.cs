using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_MainMenuLogic : MonoBehaviour
{
    GameObject go_GameSettings;

	// Use this for initialization
	void Start ()
    {
        if(GameObject.Find("GameSettings"))
        {
            go_GameSettings = GameObject.Find("GameSettings").gameObject;
        }

        if(go_GameSettings)
        {
            go_GameSettings.GetComponent<Cs_MainMenu_GameSettings>().Set_GameSettings(true, true, false, false, true, 5, 5, 1);
            SceneManager.LoadScene(3);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
