using UnityEngine;
using System.Collections;

public class Cs_HUDManager : MonoBehaviour
{
    public Camera hudCamera;

    public GameObject ui_Player;
    public GameObject GO_Player;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Vector3 playerPos = GO_Player.transform.position;
        Vector3 playerPos = new Vector3(GO_Player.transform.position.x, ui_Player.transform.position.y, GO_Player.transform.position.z);
        // print(ui_Player.transform.position);

        Vector2 viewportPoint = hudCamera.WorldToViewportPoint(playerPos);
        print(new Vector3(0, 0, 0));
        ui_Player.transform.position = new Vector3(playerPos.x * 0.15f * (650 / hudCamera.transform.position.y), ui_Player.transform.position.y, playerPos.z * 0.15f * (650 / hudCamera.transform.position.y));

        /*
        Vector2 viewportPoint = hudCamera.WorldToViewportPoint(playerPos);
        print(GO_Player.transform.position.x * 0.5f);
        ui_Player.GetComponent<RectTransform>().transform.position = new Vector3(playerPos.x * 0.125f * hudCamera.transform.position.y, ui_Player.transform.position.y, playerPos.z * 0.125f * hudCamera.transform.position.y);
        */

        /*
        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        rectTransform.anchorMin = viewportPoint;  
        rectTransform.anchorMax = viewportPoint; 
        */
    }
}
