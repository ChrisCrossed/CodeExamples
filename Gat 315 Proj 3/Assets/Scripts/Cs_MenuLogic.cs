using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_MenuLogic : MonoBehaviour
{
    public string goToScene;
    float f_Timer;

	// Use this for initialization
	void Start ()
    {
        f_Timer = 0;
        Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        f_Timer += Time.deltaTime;

        print(f_Timer);

        if (f_Timer >= 5.0f)
        {
            SceneManager.LoadScene(goToScene);
        }
	}
}
