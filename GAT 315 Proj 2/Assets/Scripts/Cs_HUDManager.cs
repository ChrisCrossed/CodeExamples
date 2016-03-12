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
        print(ui_Player.transform.position);
        
        Vector2 viewportPoint = hudCamera.WorldToViewportPoint(playerPos);
        print(viewportPoint);
        ui_Player.GetComponent<RectTransform>().transform.position = new Vector3((viewportPoint.x * 25), ui_Player.transform.position.y, viewportPoint.y);

        /*
        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        rectTransform.anchorMin = viewportPoint;  
        rectTransform.anchorMax = viewportPoint; 
        */
    }
}
