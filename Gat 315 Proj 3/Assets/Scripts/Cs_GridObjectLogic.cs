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

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
