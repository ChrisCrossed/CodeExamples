using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    float f_CountdownClock = 300;
    float f_DamageClock;
    float f_DeathTimer;

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

        GameObject.Find("Text_Countdown").GetComponent<Text>().text = "";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(charType == CharacterTypes.Boss) FlashModel();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(charType == CharacterTypes.Boss)
            {
                ApplyDamage(10);
            }
        }

        if(charType == CharacterTypes.Boss)
        {
            if(i_CurrHealth <= 0)
            {
                // Run the Countdown Clock
                f_CountdownClock -= Time.deltaTime;

                if(f_CountdownClock <= 0f)
                {
                    f_CountdownClock = 0;

                    f_DamageClock += Time.deltaTime;
                    if(f_DamageClock >= 0.1f)
                    {
                        f_DamageClock = 0;

                        // Damage player by 5 points per second until they die
                        GameObject.Find("Mech").GetComponent<HealthSystem>().ApplyDamage(5);
                    }
                }

                GameObject.Find("Text_Countdown").GetComponent<Text>().text = "Time To Escape: " + f_CountdownClock.ToString("0.0");
            }
        }

        if (charType == CharacterTypes.Player)
        {
            if (i_CurrHealth <= 0) b_IsAlive = false;

            if(!b_IsAlive)
            {
                // GameObject.Find("Canvas").SetActive(false);
                GameObject.Find("Mech").GetComponent<Cs_MechBaseController>().EndGame();

                // Begin dimming the lights
                f_DeathTimer += Time.deltaTime;
                var currColor = GameObject.Find("EndGame").GetComponent<MeshRenderer>().material.color;
                currColor.a = f_DeathTimer;
                GameObject.Find("EndGame").GetComponent<MeshRenderer>().material.color = currColor;

                if (f_DeathTimer >= 5.0f)
                {
                    SceneManager.LoadScene("Level_MainMenu");
                }
            }
        }
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
                    if(gameObject.GetComponent<MeshRenderer>())
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

            GameObject.Find("Text_Countdown").SetActive(true);

            // Enable Turrets
            GameObject.Find("EnergyBox_Test_0").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Test_1").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Test_2").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Test_3").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Turret_1").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Inside_1").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
            GameObject.Find("EnergyBox_Inside_2").GetComponent<Cs_EnergyBoxLogic>().TurnBoxOn();
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
