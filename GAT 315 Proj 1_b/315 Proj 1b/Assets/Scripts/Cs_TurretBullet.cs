using UnityEngine;
using System.Collections;

public class Cs_TurretBullet : MonoBehaviour
{
    public float f_Speed = 5;
    public int i_Damage = 5;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Stupid update
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * f_Speed;
	}

    void OnTriggerEnter(Collider collider_)
    {
        if (collider_.gameObject.GetComponent<HealthSystem>())
        {
            collider_.gameObject.GetComponent<HealthSystem>().ApplyDamage(i_Damage);
        }

        GameObject.Destroy(gameObject);
    }
}
