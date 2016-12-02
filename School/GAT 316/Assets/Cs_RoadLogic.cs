using UnityEngine;
using System.Collections;

public class Cs_RoadLogic : MonoBehaviour
{
    GameObject go_Car_1;
    GameObject go_Car_2;
    [SerializeField] GameObject go_Limo;

	// Use this for initialization
	void Start ()
    {
        go_Car_1    = GameObject.Find("Car_Black");
        go_Car_2    = GameObject.Find("Car_Red");
        // go_Limo     = GameObject.Find("Limo");
    }

    public void Set_SwitchToLimo()
    {
        // Set the cars to only run in the bottom lane
        GameObject.Destroy(go_Car_1);
        GameObject.Destroy(go_Car_2);
        // go_Car_1.GetComponent<Cs_CarLogic>().Set_BottomLaneOnly();
        // go_Car_2.GetComponent<Cs_CarLogic>().Set_BottomLaneOnly();

        // Enable and initiate the Limo
        go_Limo.SetActive(true);
        go_Limo.GetComponent<Cs_LimoLogic>().Init_Limo();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.P))
        {
            Set_SwitchToLimo();
        }
	}
}
