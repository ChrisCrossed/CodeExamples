using UnityEngine;
using System.Collections;

public class Cs_EndGame : MonoBehaviour
{
    [SerializeField] GameObject go_Door;

    bool b_EndGame;
    float f_EndGameTimer;

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
            print("Touched");

            go_Door.SetActive(true);

            b_EndGame = true;
        }
    }
}
