using UnityEngine;
using System.Collections;

public class Cs_RockLogic : MonoBehaviour
{
    float f_Magnitude;
    float f_Timer;
    float f_LiveTimer;
    bool b_IsDead;

    void Start()
    {
        // gameObject.GetComponent<Rigidbody>().velocity = transform.up * 3;
    }

    // Update is called once per frame
	void Update ()
    {
        UpdateRaycast();

        if(!b_IsDead)
        {
            CheckMagnitude();
        }
        else
        {
            FadeToOblivion();
        }

        UpdateLiveTimer();
	}

    void UpdateRaycast()
    {
        RaycastHit hit;

        Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 0.2f);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "Ground")
            {
                gameObject.GetComponent<Collider>().isTrigger = false;
            }
        }
        else
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }

    void CheckMagnitude()
    {
        f_Magnitude = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if (f_Magnitude >= 0.5f)
        {
            f_Timer = 0f;
        }
        else
        {
            f_Timer += Time.deltaTime;

            if (f_Timer >= 3.0f)
            {
                b_IsDead = true;
            }
        }

        Vector3 v3_OldVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        v3_OldVelocity.y -= (Time.deltaTime);
        gameObject.GetComponent<Rigidbody>().velocity = v3_OldVelocity;
    }

    void FadeToOblivion()
    {
        Color c_CurrColor = gameObject.GetComponent<MeshRenderer>().material.color;
        c_CurrColor.a -= Time.deltaTime;

        if(c_CurrColor.a <= 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = c_CurrColor;
        }
    }

    void UpdateLiveTimer()
    {
        f_LiveTimer += Time.deltaTime;

        if (f_LiveTimer >= 10.0f)
        {
            b_IsDead = true;
        }
    }
}
