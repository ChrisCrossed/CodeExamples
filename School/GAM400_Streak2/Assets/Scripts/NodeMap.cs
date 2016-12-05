using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeMap : MonoBehaviour {

    [SerializeField]
    GameObject up;
    [SerializeField]
    GameObject right;
    [SerializeField]
    GameObject down;
    [SerializeField]
    GameObject left;

    [SerializeField]
    Color active;
    [SerializeField]
    Color inactive;
    [SerializeField]
    Color disabled;

    [SerializeField]
    GameObject menuToLoad;
    [SerializeField]
    AnimationCurve activeCurve;

    Stack<GameObject> menus;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
