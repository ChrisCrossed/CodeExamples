using UnityEngine;
using System.Collections;

public class Cs_LimoTrigger : MonoBehaviour
{
    bool b_MeshRendererOn;

	// Use this for initialization
	void Start ()
    {
        Color clr_MeshColor = gameObject.GetComponent<MeshRenderer>().material.color;
        clr_MeshColor.a = 0.0f;
        gameObject.GetComponent<MeshRenderer>().material.color = clr_MeshColor;
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region Lerp Mesh Renderer
        Color clr_MeshColor = gameObject.GetComponent<MeshRenderer>().material.color;

        if (b_MeshRendererOn)
        {
            // Increase the color's alpha
            if (clr_MeshColor.a < 0.75f)
            {
                clr_MeshColor.a += Time.deltaTime;

                if (clr_MeshColor.a > 0.75f) clr_MeshColor.a = 0.75f;
            }
        }
        else
        {
            if (clr_MeshColor.a > 0f)
            {
                clr_MeshColor.a -= Time.deltaTime;

                if (clr_MeshColor.a < 0.05f)
                {
                    clr_MeshColor.a = 0.0f;
                }
            }
        }
        
        gameObject.GetComponent<MeshRenderer>().material.color = clr_MeshColor;

        if(clr_MeshColor.a > 0f)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        #endregion
    }

    public bool Set_MeshRenderer
    {
        set
        {
            b_MeshRendererOn = value;
        }
    }

    void OnTriggerEnter( Collider collider_ )
    {
        if(collider_.transform.root.gameObject.tag == "Player")
        {
            // Tell player controller to enter limo

            // 
        }
    }
}
