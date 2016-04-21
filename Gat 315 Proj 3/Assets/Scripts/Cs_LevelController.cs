using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PurchaseObjects
{
    Turret,
    Wall,
    Upgrade
}

public class Cs_LevelController : MonoBehaviour
{
    public int i_Currency = 900;
    public int i_Cost_Wall = 200;
    public int i_Cost_Turret = 350;
    public int i_Cost_Upgrade = 100;
    public int i_GoldPerSecond = 5;
    float f_GoldTimer;

    public GameObject[] SpawnLocations = new GameObject[4];
    float f_EnemyTimer;
    int i_CurrEnemies = 1;
    int i_CurrLevel = 0;
    bool b_GameOver = false;

    public GameObject GUI;

	// Use this for initialization
	void Start ()
    {
        GameObject.Find("Resources").GetComponent<Text>().text = "Resources: " + i_Currency;
        GameObject.Find("TowerCost").GetComponent<Text>().text = "Tower Cost: " + i_Cost_Wall;
        GameObject.Find("UpgradeCost").GetComponent<Text>().text = "Upgrade Cost: " + i_Cost_Upgrade;

        f_EnemyTimer = 5;
	}

    public void ReceiveCurrency(int i_CurrToReceive_)
    {
        i_Currency += i_CurrToReceive_;

        print(i_Currency);
    }

    public bool CheckToBuy(PurchaseObjects purchaseObj_)
    {
        if (purchaseObj_ == PurchaseObjects.Wall)
        {
            if (i_Currency >= i_Cost_Wall)
            {
                i_Currency -= i_Cost_Wall;

                // Update UI if necessary
                GameObject.Find("Resources").GetComponent<Text>().text = "Resources: " + i_Currency;

                return true;
            }
        }
        else if (purchaseObj_ == PurchaseObjects.Turret)
        {
            if (i_Currency >= i_Cost_Turret)
            {
                i_Currency -= i_Cost_Turret;

                // Update UI if necessary
                GameObject.Find("Resources").GetComponent<Text>().text = "Resources: " + i_Currency;

                return true;
            }
        }
        else if(purchaseObj_ == PurchaseObjects.Upgrade)
        {
            if(i_Currency >= i_Cost_Upgrade)
            {
                i_Currency -= i_Cost_Upgrade;

                // Update UI if necessary
                GameObject.Find("Resources").GetComponent<Text>().text = "Resources: " + i_Currency;

                return true;
            }
        }

        return false;
    }

    void GoldTimerBullshit()
    {
        f_GoldTimer += Time.deltaTime;

        if (f_GoldTimer >= 1.0f)
        {
            f_GoldTimer -= 1.0f;

            ReceiveCurrency(i_GoldPerSecond);

            // Update UI if necessary
            GameObject.Find("Resources").GetComponent<Text>().text = "Resources: " + i_Currency;
        }
    }

    void SpawnEnemies()
    {
        // Randomly pick one of the four locations
        int location = Random.Range(0, 4);

        SpawnLocations[location].GetComponent<Cs_EnemySpawnLogic>().SpawnEnemy();
    }

    public void EndGame()
    {
        print("GAME OVER");
        b_GameOver = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!b_GameOver)
        {
            GoldTimerBullshit();

            f_EnemyTimer -= Time.deltaTime;

            if(f_EnemyTimer <= 0.0f)
            {
                f_EnemyTimer = 1.0f;

                SpawnEnemies();

                --i_CurrEnemies;

                if(i_CurrEnemies <= 0)
                {
                    ++i_CurrLevel;

                    GameObject.Find("CurrentLevel").GetComponent<Text>().text = "Current Level: " + i_CurrLevel;

                    f_EnemyTimer = 10.0f;
                    i_CurrEnemies = i_CurrLevel;
                }
            }
        }
	}
}
