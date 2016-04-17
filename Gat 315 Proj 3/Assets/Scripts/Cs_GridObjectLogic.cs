using UnityEngine;
using System.Collections;

class GridPosition
{
    public int x { get; set; }
    public int y { get; set; }
    public GridQuadrant gridQuadrant { get; set; }

    public void Initialize(int x_, int y_, GridQuadrant gridQuadrant_)
    {
        x = x_;
        y = y_;
        gridQuadrant = gridQuadrant_;
    }    
}

public class Cs_GridObjectLogic : MonoBehaviour
{
    // GridPosition testGridObject = new GridPosition { x = 0, y = 0, gridQuadrant = GridPosition.GridQuadrant.Center };
    GameObject go_CurrentGameObject;

    // State of the GridObject
    bool b_IsEnabled;

    // Prototype information. Remove later.
    int i_CurrTestPos;

    // Changes through the colors of the walls when clicked on
    public void ToggleGameObjects()
    {
        // No Game Object, Instantiate it
        if(i_CurrTestPos == 0)
        {
            print("Initializing the Tower");

            go_CurrentGameObject = Instantiate(Resources.Load("GO_Wall")) as GameObject;
            go_CurrentGameObject.GetComponent<Cs_WallTowerLogic>().Initialize(10, 10);

            Vector3 newPos = gameObject.transform.position;
            newPos.y = 0.5f;
            go_CurrentGameObject.transform.position = newPos;
        }
        // Run through three colors on the tower (Blue)
        else if(i_CurrTestPos == 1)
        {
            go_CurrentGameObject.GetComponent<Cs_WallTowerLogic>().SetNewMaterialColor(Colors.Blue);
        }

        // Run through three colors on the tower (Red)
        else if (i_CurrTestPos == 2)
        {
            go_CurrentGameObject.GetComponent<Cs_WallTowerLogic>().SetNewMaterialColor(Colors.Red);
        }

        // Run through three colors on the tower (Green)
        else if (i_CurrTestPos == 3)
        {
            go_CurrentGameObject.GetComponent<Cs_WallTowerLogic>().SetNewMaterialColor(Colors.Green);
        }

        // Turn Tower Semi-transparent
        else if (i_CurrTestPos == 4)
        {
            go_CurrentGameObject.GetComponent<Cs_WallTowerLogic>().SetNewMaterialColor(Colors.SemiTransparent);
        }

        // Destroy the Tower
        else if (i_CurrTestPos == 5)
        {
            // Destroy tower
            GameObject.Destroy(go_CurrentGameObject);
        }

        // Set/Reset Counter
        if(++i_CurrTestPos > 5) i_CurrTestPos = 0;
    }

    public void SetGridObjectState(bool b_IsEnabled_)
    {
        b_IsEnabled = b_IsEnabled_;

        // Set the Mouse Collider
        gameObject.GetComponent<BoxCollider>().enabled = b_IsEnabled;

        // Set the Mesh Renderer
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = b_IsEnabled;
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
