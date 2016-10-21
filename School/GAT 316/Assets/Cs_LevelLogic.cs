using UnityEngine;
using System.Collections;

public class Cs_LevelLogic : MonoBehaviour
{
    [SerializeField] float f_MaxTimer_FromChaseToInvestigate;
    float f_Timer_FromChaseToInvestigate;

    [SerializeField] float f_MaxTimer_FromInvestigateToPatrol;
    float f_Timer_FromInvestigateToPatrol;

    [SerializeField] GameObject[] DisableObjectsOnChase = new GameObject[5];
    [SerializeField] GameObject[] EnableObjectsOnChase = new GameObject[5];

    [SerializeField] GameObject[] DisableObjectsOnInvestigate = new GameObject[5];
    [SerializeField] GameObject[] EnableObjectsOnInvestigate = new GameObject[5];

    [SerializeField] GameObject[] DisableObjectsOnPatrol = new GameObject[5];
    [SerializeField] GameObject[] EnableObjectsOnPatrol = new GameObject[5];

    [SerializeField] GameObject[] EnemiesToControl = new GameObject[5];
    Enum_EnemyState e_EnemiesState = Enum_EnemyState.Patrol;

    GameObject go_Player;

    // Use this for initialization
    void Start ()
    {
        go_Player = GameObject.Find("Player");
	}

    bool CheckEnemyInList( GameObject go_EnemyObject_ )
    {
        // If an enemy is provided, check to see if this enemy is within the list of controllable enemies
        if (go_EnemyObject_ != null)
        {
            for( int i_ = 0; i_ < EnemiesToControl.Length; ++i_)
            {
                if( go_EnemyObject_ == EnemiesToControl[i_] )
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Set_ChaseState( GameObject go_EnemyObject_ = null)
    {
        if(go_EnemyObject_ != null)
        {
            if(CheckEnemyInList( go_EnemyObject_ ) == false)
            {
                return;
            }
        }

        // When the player is spotted, reset the f_Timer
        f_Timer_FromChaseToInvestigate = f_MaxTimer_FromChaseToInvestigate;

        // Set the state of the enemies involved
        e_EnemiesState = Enum_EnemyState.ChasePlayer;

        for(int i_ = 0; i_ < EnemiesToControl.Length; ++i_)
        {
            if(EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>())
            {
                EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>().GoToState_ChasePlayer(go_Player.transform.position, true);
            }
        }
    }

    public void Set_InvestigateState( bool b_InvesticatePlayerLocation_ , GameObject go_EnemyObject_ = null )
    {
        // Check to make sure the enemy calling this is within the list
        if (go_EnemyObject_ != null)
        {
            if (CheckEnemyInList(go_EnemyObject_) == false)
            {
                return;
            }
        }

        // Reset timer
        f_Timer_FromInvestigateToPatrol = f_MaxTimer_FromInvestigateToPatrol;

        // Set the state of the enemies involved
        e_EnemiesState = Enum_EnemyState.InvestigateLocation;

        for (int i_ = 0; i_ < EnemiesToControl.Length; ++i_)
        {
            if (EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>())
            {
                if(b_InvesticatePlayerLocation_)
                {
                    EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>().GoToState_InvestigateLocation(go_Player.transform.position);
                }
                else
                {
                    // Goes to their last known investigation point
                    EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>().GoToState_InvestigateLocation();
                }
            }
        }
    }

    public void Set_PatrolState( GameObject go_EnemyObject_ = null )
    {
        if (go_EnemyObject_ != null)
        {
            if (CheckEnemyInList(go_EnemyObject_) == false)
            {
                return;
            }
        }

        f_Timer_FromChaseToInvestigate = f_MaxTimer_FromChaseToInvestigate;
        f_Timer_FromInvestigateToPatrol = f_MaxTimer_FromInvestigateToPatrol;

        e_EnemiesState = Enum_EnemyState.Patrol;

        for (int i_ = 0; i_ < EnemiesToControl.Length; ++i_)
        {
            if (EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>())
            {
               EnemiesToControl[i_].GetComponent<Cs_EnemyLogic_Grunt>().GoToState_Patrol();
            }
        }
    }

    public float Get_CurrentTimer(Enum_EnemyState e_EnemyState_, bool b_GetMaxTimer = false, GameObject go_EnemyObject_ = null )
    {
        if (go_EnemyObject_ != null)
        {
            if (CheckEnemyInList(go_EnemyObject_) == false)
            {
                return -1f;
            }
        }

        if (e_EnemyState_ == Enum_EnemyState.ChasePlayer)
        {
            if (!b_GetMaxTimer) return f_Timer_FromChaseToInvestigate;
            else return f_MaxTimer_FromChaseToInvestigate;
        }
        else if(e_EnemyState_ == Enum_EnemyState.InvestigateLocation)
        {
            if (!b_GetMaxTimer) return f_Timer_FromInvestigateToPatrol;
            else return f_MaxTimer_FromInvestigateToPatrol;
        }

        return 0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(e_EnemiesState == Enum_EnemyState.ChasePlayer)
        {
            f_Timer_FromChaseToInvestigate -= Time.deltaTime;

            if(f_Timer_FromChaseToInvestigate <= 0)
            {
                // Reset timer
                f_Timer_FromChaseToInvestigate = f_MaxTimer_FromChaseToInvestigate;

                // Change state
                Set_InvestigateState(true);
            }
        }
        else if(e_EnemiesState == Enum_EnemyState.InvestigateLocation)
        {
            f_Timer_FromInvestigateToPatrol -= Time.deltaTime;

            if(f_Timer_FromInvestigateToPatrol <= 0)
            {
                // Reset timer
                f_Timer_FromInvestigateToPatrol = f_MaxTimer_FromInvestigateToPatrol;

                // Change state
                Set_PatrolState();
            }
        }
	}
}
