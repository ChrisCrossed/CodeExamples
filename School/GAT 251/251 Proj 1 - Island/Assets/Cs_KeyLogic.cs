using UnityEngine;
using System.Collections;

public class Cs_KeyLogic : MonoBehaviour
{
    [SerializeField]
    bool b_IsFake = false;
    bool b_KillKey = false;

	// Use this for initialization
	void Start ()
    {
	    if(b_IsFake)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!b_IsFake)
        {
            // Force Rotation
            RotateKey();
        }
        else
        {
            if(b_KillKey)
            {
                float f_Alpha = gameObject.GetComponent<MeshRenderer>().material.color.a - Time.deltaTime;

                if (f_Alpha <= 0) Destroy(gameObject);
                else
                {
                    Color tempColor = gameObject.GetComponent<MeshRenderer>().materials[0].color;
                    gameObject.GetComponent<MeshRenderer>().materials[0].color = new Color(tempColor.r, tempColor.g, tempColor.b, f_Alpha);

                    tempColor = gameObject.GetComponent<MeshRenderer>().materials[1].color;
                    gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(tempColor.r, tempColor.g, tempColor.b, f_Alpha);
                }
            }
        }
	}

    void RotateKey()
    {

    }

    public void DestroyKey()
    {
        b_KillKey = true;
    }

    void OnTriggerEnter(Collider collision_)
    {
        if (collision_.gameObject.name == "Capsule")
        {
            Destroy(gameObject);
        }
    }
}
