using UnityEngine;
using System.Collections;

public enum CharacterTypes
{
    Player, Turret, Objective, Boss
}

public class HealthSystem : MonoBehaviour
{
    // Health Values
    public int i_MaxHealth = 10;
    int i_CurrHealth;

    // Store what type of object this is
    public CharacterTypes charType = CharacterTypes.Turret;

    // Flash timers
    float f_FlashModelTimer;
    float f_Timer;
    float f_Percent;
    Color startColor;

    bool b_IsActive = true;
    bool b_IsAlive = true;

	// Use this for initialization
	void Start ()
    {
        if(gameObject.GetComponent<MeshRenderer>())
        {
            startColor = gameObject.GetComponent<MeshRenderer>().material.color;
        }

        i_CurrHealth = i_MaxHealth;
        f_FlashModelTimer = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        FlashModel();
	}

    // When this object dies
    void OnDeath()
    {
        // If player dies...

        // If enemy dies...

        // GameObject.Destroy(gameObject);
    }

    // Disable the object if it is an Objective
    public void SetObjectiveStatus(bool b_Status_)
    {
        if(charType == CharacterTypes.Objective)
        {
            b_IsActive = b_Status_;

            if (gameObject.GetComponent<Cs_DoorLogic>()) gameObject.GetComponent<Cs_DoorLogic>().OpenDoor();
        }

        if(charType == CharacterTypes.Turret)
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

        print(charType.ToString() + " has " + i_CurrHealth + " remaining");

        if (i_CurrHealth <= 0) OnDeath();
    }

    void FlashModel()
    {
        if (b_IsAlive)
        {
            if (i_CurrHealth > 0)
            {
                // Keep counting upward to compare against
                if (f_FlashModelTimer <= 1) f_FlashModelTimer += Time.deltaTime;

                if (f_FlashModelTimer < 0.2f)
                {
                    Color currColor = gameObject.GetComponent<MeshRenderer>().material.color;
                    currColor.g = 1;
                    currColor.b = 1;
                    currColor.r = 0;
                    currColor.a = 1;
                    gameObject.GetComponent<MeshRenderer>().material.color = currColor;
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().material.color = startColor;

                    f_Timer += Time.deltaTime * 2;

                    // Sin waves between 0 & 1
                    f_Percent = (Mathf.Sin(f_Timer) / 2f) + 0.5f;

                    var currPos = gameObject.GetComponent<MeshRenderer>().material.color;
                    currPos.a = f_Percent;
                    gameObject.GetComponent<MeshRenderer>().material.color = currPos;
                }
            }
            else // Turns off the button
            {
                // Kill the boss
                
                // Turn on all turrets (Minus the ones in the boss room)

                // Run the Countdown Clock
            }
        }
    }

    void HealthCheckpoints()
    {
        if (i_CurrHealth > 30) print("Stage One");
        if(i_CurrHealth == 30)
        {
            print("Stage Two");
            GameObject.Find("Boss_Wall_1").GetComponent<Cs_BossWallTrigger>().SetState(true);
            GameObject.Find("Boss_Wall_2").GetComponent<Cs_BossWallTrigger>().SetState(true);
        }
        if(i_CurrHealth == 15 || i_CurrHealth == 5)
        {
            print("Stage Three");

            GameObject.Find("EnergyBox_Boss_2").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Boss_1").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
        }
        if(i_CurrHealth <= 0)
        {
            print("Stage Four");
            GameObject.Find("EscapeHatch").GetComponent<Cs_DoorLogic>().OpenDoor();

            GameObject.Find("Boss_Wall_1").GetComponent<Cs_BossWallTrigger>().EndGame();
            GameObject.Find("Boss_Wall_2").GetComponent<Cs_BossWallTrigger>().EndGame();
        }
    }

    void OnTriggerEnter(Collider collider_)
    {
        if(charType == CharacterTypes.Boss)
        {
            if (collider_.tag == "Laser")
            {
                ApplyDamage(1);

                HealthCheckpoints();

                f_FlashModelTimer = 0;
            }
        }
    }
}
