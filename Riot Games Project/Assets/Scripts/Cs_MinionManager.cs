using UnityEngine;
using System.Collections;

public class Cs_MinionManager : MonoBehaviour
{
    public GameObject[] MidLaneCheckpointList = new GameObject[10];

    CheckpointObject[] RedCheckpoint = new CheckpointObject[10];
    CheckpointObject[] BlueCheckpoint = new CheckpointObject[10];

    // Spawn Points
    public GameObject RedSpawn_Minion;
    public GameObject RedSpawn_Player;
    public GameObject BlueSpawn_Minion;
    public GameObject BlueSpawn_Player;

    // Timers
    float f_GameClock;
    float f_MinionSpawnTimer = 0f;
    int NUM_MINIONS_TO_SPAWN = 4;
    int i_MinionsSpawned = 10;
    int TIME_BETWEEN_MINIONS_BREAK = 30;
    int TIME_TO_START_MINIONS = 0;
    bool b_CanSpawnMinions = false;

    /*
    int i_MinionSpawnTimer;
    
    int TIME_BETWEEN_MINIONS_SPAWNED = 1;
    int TIME_TO_START_MINIONS = 0;
    */

    // Use this for initialization
    void Start ()
    {
        SetCheckpointLists();
	}

    void SetCheckpointLists()
    {
        // Determine how many checkpoints actually exist
        var numCheckpoints = 0;
        for(var i = 0; i < MidLaneCheckpointList.Length; ++i)
        {
            if (MidLaneCheckpointList[i] != null) ++numCheckpoints;
        }

        // Set the Red Minion Checkpoint List
        for (int j = 0; j < numCheckpoints; ++j)
        {
            RedCheckpoint[j] = new CheckpointObject();

            RedCheckpoint[j].checkpointName = MidLaneCheckpointList[j].gameObject.name;
            RedCheckpoint[j].checkpointPos = MidLaneCheckpointList[j].gameObject.transform.position;
        }

        // Set the Blue Minion Checkpoint List (Opposite of the Red Checkpoint List)
        for(int k = numCheckpoints - 1; k >= 0; --k)
        {
            BlueCheckpoint[k] = new CheckpointObject();

            BlueCheckpoint[k].checkpointName = MidLaneCheckpointList[(numCheckpoints - 1) - k].gameObject.name;
            BlueCheckpoint[k].checkpointPos = MidLaneCheckpointList[(numCheckpoints - 1) - k].gameObject.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        f_GameClock += Time.deltaTime;

        if (((int)f_GameClock % TIME_BETWEEN_MINIONS_BREAK == 0) && (int)f_GameClock >= TIME_TO_START_MINIONS && !b_CanSpawnMinions)
        {
            i_MinionsSpawned = 0;
        }

        // Check to see if we're allowed to spawn minions
        if(i_MinionsSpawned < NUM_MINIONS_TO_SPAWN)
        {
            // Spawn Minions only once per second
            if (f_MinionSpawnTimer <= 0.0f && i_MinionsSpawned < NUM_MINIONS_TO_SPAWN)
            {
                ++i_MinionsSpawned;

                if (i_MinionsSpawned >= NUM_MINIONS_TO_SPAWN)
                {
                    b_CanSpawnMinions = false;
                }
                else
                {
                    if(i_MinionsSpawned <= 2) SpawnMinions(true, i_MinionsSpawned); else SpawnMinions(false, i_MinionsSpawned);

                    f_MinionSpawnTimer = 1.0f;
                }
            }

            f_MinionSpawnTimer -= Time.deltaTime;
        }
	}

    void SpawnMinions(bool b_IsMelee_, int i_MinionNumber_)
    {
        // var RedMinion = Instantiate(Resources.Load("Minion"), RedMinionSpawn.transform.position, RedMinionSpawn.transform.rotation) as GameObject;
        GameObject RedMinion = Instantiate(Resources.Load("Minion")) as GameObject;
        string redMinionName = "";
        if (b_IsMelee_) redMinionName = "Red Melee Minion"; else redMinionName = "Red Ranged Minion ";
        redMinionName += i_MinionNumber_;
        RedMinion.name = redMinionName;
        RedMinion.transform.position = RedSpawn_Minion.transform.position;
        RedMinion.transform.rotation = RedSpawn_Minion.transform.rotation;
        RedMinion.GetComponent<Cs_MinionLogic>().Initialize_Minion(RedCheckpoint, TeamTypes.RedTeam, (int)f_GameClock, b_IsMelee_);

        GameObject BlueMinion = Instantiate(Resources.Load("Minion")) as GameObject;
        string blueMinionName = "";
        if(b_IsMelee_) blueMinionName = "Blue Melee Minion"; else blueMinionName = "Blue Ranged Minion ";
        blueMinionName += i_MinionNumber_;
        BlueMinion.name = blueMinionName;
        BlueMinion.transform.position = BlueSpawn_Minion.transform.position;
        BlueMinion.transform.rotation = BlueSpawn_Minion.transform.rotation;
        BlueMinion.GetComponent<Cs_MinionLogic>().Initialize_Minion(BlueCheckpoint, TeamTypes.BlueTeam, (int)f_GameClock, b_IsMelee_);
    }
}
