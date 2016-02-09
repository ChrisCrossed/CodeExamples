using UnityEngine;
using System.Collections;

public enum CharacterTypes
{
    Player, Enemy, Objective
}

public class HealthSystem : MonoBehaviour
{
    // Health Values
    public int i_MaxHealth = 10;
    int i_CurrHealth;

    // Store what type of object this is
    public CharacterTypes charType = CharacterTypes.Enemy;

    bool b_IsActive = true;

	// Use this for initialization
	void Start ()
    {
        i_CurrHealth = i_MaxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // When this object dies
    void OnDeath()
    {
        // If player dies...

        // If enemy dies...

        GameObject.Destroy(gameObject);
    }

    // Disable the object if it is an Objective
    public void SetObjectiveStatus(bool b_Status_)
    {
        if(charType == CharacterTypes.Objective)
        {
            b_IsActive = b_Status_;

            if (gameObject.GetComponent<Cs_DoorLogic>()) gameObject.GetComponent<Cs_DoorLogic>().OpenDoor();
        }

        if(charType == CharacterTypes.Enemy)
        {
            print("Got Here");
            gameObject.GetComponentInChildren<Cs_TurretAxel>().SetState(b_Status_);
            gameObject.GetComponentInChildren<Cs_TurretJoint>().SetTurretState(b_Status_);
        }
    }

    // Apply damage to this unit
    public void ApplyDamage(int i_DamageReceived_)
    {
        i_CurrHealth -= i_DamageReceived_;

        if (i_CurrHealth <= 0) OnDeath();
    }
}
