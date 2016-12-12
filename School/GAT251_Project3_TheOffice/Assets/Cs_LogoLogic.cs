using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_LogoLogic : MonoBehaviour
{
    [SerializeField] int i_NextScene;
    [SerializeField] bool b_IsMainMenu = false;
    float f_Timer;
    static float f_Timer_Max = 7f;

    // Update is called once per frame
	void Update ()
    {
        if(!b_IsMainMenu)
        {
            f_Timer += Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            f_Timer = f_Timer_Max;
        }

        if(f_Timer > f_Timer_Max)
        {
            SceneManager.LoadScene(i_NextScene);
        }
	}
}
