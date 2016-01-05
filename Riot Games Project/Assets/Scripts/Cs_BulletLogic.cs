using UnityEngine;
using System.Collections;

public class Cs_BulletLogic : MonoBehaviour
{
    public GameObject go_FinalTargetObject;
    public float f_Speed;
    int i_Damage;
    float f_yPos;

	// Use this for initialization
	void Start ()
    {
        InitializeBullet(null, 5.0f, 1337);
	}

    void InitializeBullet(GameObject go_FinalTargetObject_, float f_Speed_, int i_Damage_)
    {
        // Set the bullet's game object goal
        go_FinalTargetObject = go_FinalTargetObject_;

        // Set the bullet's speed
        f_Speed = f_Speed_;

        // Set the bullet's damage
        i_Damage = i_Damage_;

        // Store the bullet's Y Position
        f_yPos = gameObject.transform.position.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Find the bullet's game object goal
        if (go_FinalTargetObject)
        {
            Vector3 bulletPos = gameObject.transform.position;

            // Rotate toward it
            Quaternion Q_CurrRot = transform.rotation;

            // Grab rotation toward object (Yes this is messy)
            transform.LookAt(go_FinalTargetObject.gameObject.transform);
            Quaternion Q_NewRot = transform.rotation;

            // Lerp toward the new rotation
            transform.rotation = Quaternion.Lerp(Q_CurrRot, Q_NewRot, 0.1f);
            
            // Move forward at set pace
            gameObject.GetComponent<Rigidbody>().velocity = (transform.forward * f_Speed);
        }
    }

    void OnCollisionEnter(Collision collision_)
    {
        print("Hit: " + collision_.gameObject.name);

        // If we collide with the bullet object's goal, damage it and destroy self
        if (collision_.gameObject == go_FinalTargetObject.gameObject)
        {
            print("Hit");
            GameObject.Destroy(gameObject);
        }
    }
}
