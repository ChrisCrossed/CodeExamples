﻿using UnityEngine;
using System.Collections;

enum Enum_EnemyState
{
    Patrol,
    InvestigateLocation,
    ChasePlayer
}

public class Cs_EnemyLogic_Grunt : MonoBehaviour
{
    [SerializeField] GameObject[] go_PatrolPath = new GameObject[20];

    [SerializeField] Enum_EnemyState e_EnemyState = Enum_EnemyState.Patrol;

    Vector3 v3_LastKnownLocation;

    GameObject go_ExclamationMark;
    GameObject go_QuestionMark;

	// Use this for initialization
	void Start ()
    {
        // Set the first patrol point for the guard
        if(gameObject.GetComponent<NavMeshAgent>().enabled)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if(go_PatrolPath[0] != null)
            {
                agent.destination = go_PatrolPath[0].transform.position;
            }
            else { print(gameObject.name + " NEEDS A PATROL PATH "); }
        }

        // Set the wait time for the first patrol point
        if(go_PatrolPath[0] != null)
        {
            f_MAX_WAIT_TIME = go_PatrolPath[0].GetComponent<Cs_PatrolPointLogic>().GetWaitTime();
        }

        // Set the models above the player
        go_ExclamationMark = transform.Find("Mdl_ExclamationMark").gameObject;
        go_QuestionMark = transform.Find("Mdl_QuestionMark").gameObject;
    }

    float f_PatrolWaitTimer;
    float f_MAX_WAIT_TIME = 0.0f;
    int i_PatrolPoint = 0;

    float f_InvestigateTimer;
    float f_MAX_INVESTIGATE_TIME = 5.0f;

    float f_BasicMoveSpeed = 3.5f;
    public void GoToState_Patrol()
    {
        #region Reset Basic Details
        // Reset timer
        f_PatrolWaitTimer = 0f;

        // Set next wait timer
        f_MAX_WAIT_TIME = go_PatrolPath[i_PatrolPoint].GetComponent<Cs_PatrolPointLogic>().GetWaitTime();

        // Reset speed
        gameObject.GetComponent<NavMeshAgent>().speed = f_BasicMoveSpeed;
        #endregion

        // Go To State
        e_EnemyState = Enum_EnemyState.Patrol;
    }

    Vector3 v3_InvestigateLocation;
    float f_SprintMoveSpeed = 6f;
    public void GoToState_InvestigateLocation( Vector3 v3_InvestigateLocation_ )
    {
        #region Reset Basic Details
        f_InvestigateTimer = 0.0f;

        gameObject.GetComponent<NavMeshAgent>().destination = v3_InvestigateLocation_;
        gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 0.1f;
        gameObject.GetComponent<NavMeshAgent>().speed = f_SprintMoveSpeed;
        gameObject.GetComponent<NavMeshAgent>().acceleration = 5.0f;
        #endregion

        // Go To State
        e_EnemyState = Enum_EnemyState.InvestigateLocation;
    }
    
    public void GoToState_ChasePlayer( Vector3 v3_PlayerLastKnownLocation_, bool b_SeeThePlayer_ = false)
    {
        #region Reset basic details
        // Go To State
        e_EnemyState = Enum_EnemyState.ChasePlayer;

        v3_LastKnownLocation = v3_PlayerLastKnownLocation_;

        gameObject.GetComponent<NavMeshAgent>().destination = v3_LastKnownLocation;
        gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 0.1f;
        gameObject.GetComponent<NavMeshAgent>().speed = f_SprintMoveSpeed;
        gameObject.GetComponent<NavMeshAgent>().acceleration = 5.0f;

        #endregion

    }

    void SetIconState( Enum_EnemyState e_EnemyState_ )
    {
        // Set the exclamation mark
        if(e_EnemyState_ == Enum_EnemyState.ChasePlayer)
        {
            go_ExclamationMark.GetComponent<MeshRenderer>().enabled = true;
            go_QuestionMark.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(e_EnemyState_ == Enum_EnemyState.InvestigateLocation)
        {
            go_ExclamationMark.GetComponent<MeshRenderer>().enabled = false;
            go_QuestionMark.GetComponent<MeshRenderer>().enabled = true;
        }
        else // Default state
        {
            go_ExclamationMark.GetComponent<MeshRenderer>().enabled = false;
            go_QuestionMark.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    float f_UpdateTimer;
	void Update ()
    {
        if (e_EnemyState == Enum_EnemyState.Patrol)
        {
            // If within a set distance of the patrol point, increment the Wait Timer
            if (gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                Vector3 v3_PatrolPos = go_PatrolPath[i_PatrolPoint].transform.position;

                gameObject.GetComponent<NavMeshAgent>().destination = v3_PatrolPos;
                gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 0.1f;

                // if (gameObject.GetComponent<NavMeshAgent>().remainingDistance <= gameObject.GetComponent<NavMeshAgent>().radius + 0.15f)
                if (Vector3.Distance(gameObject.transform.position, v3_PatrolPos) <= gameObject.GetComponent<NavMeshAgent>().radius + 0.15f)
                {
                    f_PatrolWaitTimer += 0.1f;

                    // If the Wait Timer reaches a certain point, go to the next point & reset the timer
                    if (f_PatrolWaitTimer >= f_MAX_WAIT_TIME)
                    {
                        // Increment/Reset
                        if (go_PatrolPath[i_PatrolPoint + 1] != null && (i_PatrolPoint + 1) < go_PatrolPath.Length)
                        {
                            ++i_PatrolPoint;
                        }
                        else
                        {
                            i_PatrolPoint = 0;
                        }

                        // Set next wait timer
                        f_MAX_WAIT_TIME = go_PatrolPath[i_PatrolPoint].GetComponent<Cs_PatrolPointLogic>().GetWaitTime();

                        // Reset timer
                        f_PatrolWaitTimer = 0f;
                    }
                }
            }


            // If the enemy sees the player, chase them (Reset Wait Timer, record last known patrol position)

            // If the enemy hears a sound, investigate (Reset Wait Timer, record last known patrol position)
        }
        else if (e_EnemyState == Enum_EnemyState.InvestigateLocation)
        {
            print("Investigate: " + v3_LastKnownLocation);

            // if (Vector3.Distance(gameObject.transform.position, v3_InvestigateLocation) <= gameObject.GetComponent<NavMeshAgent>().radius + 0.15f)
            if (gameObject.GetComponent<NavMeshAgent>().remainingDistance <= 0.15f)
            {
                f_InvestigateTimer += Time.deltaTime;

                // If the Wait Timer reaches a certain point, go to the next point & reset the timer
                if (f_InvestigateTimer >= f_MAX_INVESTIGATE_TIME)
                {
                    GoToState_Patrol();
                }
            }
        }
        else if (e_EnemyState == Enum_EnemyState.ChasePlayer)
        {
            // print("Chase: " + v3_LastKnownLocation);
            
            gameObject.GetComponent<NavMeshAgent>().destination = v3_LastKnownLocation;
        }

        SetIconState(e_EnemyState);
    } // End Update
}
