using UnityEngine;
using System.Collections;

public class Cs_EndGame : MonoBehaviour
{
    [SerializeField] GameObject go_Door;

    bool b_EndGame;
    float f_EndGameTimer;

    GameObject go_Player;

    void Start()
    {
        go_Player = GameObject.Find("Player");
    }

    void Update()
    {
        if(b_EndGame)
        {
            f_EndGameTimer += Time.deltaTime;

            if (f_EndGameTimer >= 5.0f)
            {
                Application.Quit();

                print("WE QUIT");
            }
        }
    }

	void OnTriggerEnter( Collider collider_ )
    {
        if (collider_.transform.root.gameObject.tag == "Player")
        {
            if (go_Door != null)
            {
                go_Door.SetActive(true);
            }

            b_EndGame = true;

            if(go_Player != null)
            {
                if(go_Player.GetComponent<Cs_PlayerController>())
                {
                    go_Player.GetComponent<Cs_PlayerController>().Set_FadeState(true);
                }
            }
        }
    }
}
