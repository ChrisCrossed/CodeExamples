using UnityEngine;
using System.Collections;

public class Cs_KeyboardLogic : MonoBehaviour
{
    GameObject go_Button_1;
    GameObject go_Button_2;
    GameObject go_Button_3;
    GameObject go_Button_4;

    // Use this for initialization
    void Start ()
    {
        // Initialize
        go_Button_1 = transform.Find("Key_1").gameObject;
        go_Button_2 = transform.Find("Key_2").gameObject;
        go_Button_3 = transform.Find("Key_3").gameObject;
        go_Button_4 = transform.Find("Key_4").gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
