using UnityEngine;
using System.Collections;

public class Cs_RockLogic : MonoBehaviour
{
    float f_Magnitude;
    float f_Timer;
    float f_LiveTimer;
    bool b_IsDead;
    bool b_HasMadeSound;
    float f_HitTimer;

    void Start()
    {
        
    }

    // Update is called once per frame
	void Update ()
    {
        // if(gameObject.GetComponent<MeshCollider>().isTrigger)
        if (!b_HasMadeSound)
        {
            UpdateRaycast();
        }

        if(!b_IsDead)
        {
            CheckMagnitude();
        }
        else
        {
            FadeToOblivion();
        }

        UpdateLiveTimer();

        if (f_HitTimer > 0) f_HitTimer += Time.deltaTime;
	}

    void UpdateRaycast()
    {
        RaycastHit hit;

        int layer_mask = LayerMask.GetMask("Ground", "Wall");

        Vector3 v3_VelocityNormalized = gameObject.GetComponent<Rigidbody>().velocity.normalized;

        Debug.DrawRay(gameObject.transform.position, v3_VelocityNormalized, Color.red, Time.deltaTime);

        if(Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 0.2f, layer_mask))
        {
            // Tell 'sound collider' to inform enemies (Occurs as a trigger BEFORE making the change to 'IsTrigger = true'
            if (!b_HasMadeSound)
            {
                gameObject.transform.Find("Sound_Collider").GetComponent<Cs_RockSoundLogic>().MakeSound();

                f_HitTimer += Time.deltaTime;

                b_HasMadeSound = true;
            }

            // gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
        }
        else if (Physics.Raycast(gameObject.transform.position, v3_VelocityNormalized, out hit, 0.2f, layer_mask))
        {
            // Tell 'sound collider' to inform enemies (Occurs as a trigger BEFORE making the change to 'IsTrigger = true'
            if (!b_HasMadeSound)
            {
                gameObject.transform.Find("Sound_Collider").GetComponent<Cs_RockSoundLogic>().MakeSound();

                f_HitTimer += Time.deltaTime;

                b_HasMadeSound = true;
            }

            // gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            // gameObject.GetComponent<Collider>().isTrigger = true;
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

    void OnCollisionEnter( Collision collision_ )
    {
        if(f_HitTimer >= 1f)
        {
            print(f_HitTimer);
            gameObject.transform.Find("Sound_Collider").GetComponent<Cs_RockSoundLogic>().MakeSound();
        }
    }
}
