using UnityEngine;
using System.Collections;

public enum ObjectTypes
{
    Player,
    Minion,
    Tower,
    Bullet,
    None
}

public enum TeamTypes
{
    RedTeam,
    BlueTeam
}

public class HealthSystem : MonoBehaviour
{
    // Left Public so that the Player/Tower can be set manually in Unity. Minions are set in MinionManager
    public ObjectTypes objectType;
    public TeamTypes teamType;

    int i_StartingHealth;
    int i_CurrentHealth;

	// Use this for initialization
	void Start ()
    {
        // objectType = ObjectTypes.None;
	}

    public void SetHealth(int i_Health_)
    {
        i_StartingHealth = i_Health_;
        i_CurrentHealth = i_Health_;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetObjectType(ObjectTypes objType_)
    {
        objectType = objType_;
    }

    public ObjectTypes GetObjectType()
    {
        return objectType;
    }

    public void SetTeamType(TeamTypes teamType_)
    {
        teamType = teamType_;
    }

    public TeamTypes GetTeamType()
    {
        return teamType;
    }

    public void ApplyDamage(TeamTypes teamTakenDamageFrom_, ObjectTypes objectTakenDamageFrom_, int damageTaken_)
    {
        // print(teamTakenDamageFrom_ + " hit " + gameObject.name);

        // Friendly Fire Off
        if (teamTakenDamageFrom_ != teamType)
        {
            

            // If this gameObject is a Minion...
            if (objectType == ObjectTypes.Minion)
            {
                // Decrease current health by damage taken
                i_CurrentHealth -= damageTaken_;

                // print(gameObject.name + " received " + damageTaken_ + ", new HP: " + i_CurrentHealth);

                // Kill Minion if under a certain amount of health
                if(i_CurrentHealth <= 0)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}
