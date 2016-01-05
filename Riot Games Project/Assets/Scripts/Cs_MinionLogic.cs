using UnityEngine;
using System.Collections;

public class CheckpointObject
{
    public Vector3 checkpointPos;
    public string checkpointName;
}

public class Cs_MinionLogic : MonoBehaviour
{
    // bool IsTeamB;
    public float speed = 5f;
    float currentSpeed = 5f;
    int currCheckpoint;
    
    CheckpointObject[] checkpoint = new CheckpointObject[10];
    GameObject[] enemyArray = new GameObject[10];
    bool b_IsEnemyArrayLoaded = false;

    bool b_IsPushed = false;
    bool b_IsMelee = true;

    // Health & Stats Information
    int i_AttackDamage;
    int i_Health;

	// Use this for initialization
	void Start ()
    {

    }

    public void Initialize_Minion(CheckpointObject[] checkpointList_, TeamTypes minionTeam_, int i_SpawnTime_, bool b_IsMelee_)
    { 
        checkpoint = checkpointList_;
        gameObject.GetComponent<HealthSystem>().SetTeamType(minionTeam_);
        gameObject.GetComponent<HealthSystem>().SetObjectType(ObjectTypes.Minion);

        // Used to calculate stats
        i_AttackDamage = 12 + (i_SpawnTime_ / 180); // 12 + 1 dmg every 3 minutes
        i_Health = 450 + (20 * (i_SpawnTime_ / 180)); // 450 HP + 20 dmg every 3 minutes
        gameObject.GetComponent<HealthSystem>().SetHealth(i_Health);
        b_IsMelee = b_IsMelee_;

        if (minionTeam_ == TeamTypes.BlueTeam)
        {
            GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        if(!b_IsMelee) GetComponent<Renderer>().material.color += new Color(0.0f, 0.7f, 0.0f, 0.0f);

        // Starts the minion's checkpoint to 1 so they don't go into their fountain
        currCheckpoint = 1;
    }

    // Update is called once per frame
    void Update ()
    {        
        if(currentSpeed < speed)
        {
            currentSpeed += Time.deltaTime * 5;
        }

        // If there's an enemy within the array, attack them. Otherwise, go to the next checkpoint.
        if(b_IsEnemyArrayLoaded)
        {
            if(!b_IsPushed) AttackEnemyInEnemyArray();
            else
            {
                // if(gameObject.GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0))
                if (gameObject.GetComponent<Rigidbody>().velocity.magnitude == 0f)
                {
                    b_IsPushed = false;
                }
            }
        }
        else    // I figure it is better to rarely have a 1-frame lapse where we're not chasing a checkpoint,
                // rather than check against a bool twice every frame.
        {
            GoToCurrentCheckpoint();
        }
        // print(checkpoint[currCheckpoint].checkpointName);

        
        /*
        for(var i = 0; i < enemyArray.Length; ++i)
        {
            if(enemyArray[i] != null)
            {
                print(gameObject.name + ": " + i + " = " + enemyArray[i].name);
            }
        }*/
	}

    void AttackEnemyInEnemyArray()
    {
        // Find the first enemy in the array and attack them.
        int enemyNum = -1;
        for(var i = 0; i < enemyArray.Length; ++i)
        {
            if (enemyArray[i] != null)
            {
                enemyNum = i;
                break;
            }
        }

        // If there is no enemy, set the bool to false and kick out.
        if(enemyNum == -1)
        {
            b_IsEnemyArrayLoaded = false;
            return;
        }
        else // Because there has to be an enemy here, attack it.
        {
            // Rotate toward current checkpoint
            // Grab current rotation
            Quaternion Q_CurrRot = transform.rotation;

            // Grab rotation toward object (Yes this is messy)
            transform.LookAt(enemyArray[enemyNum].gameObject.transform);
            Quaternion Q_NewRot = transform.rotation;

            // Lerp toward the new rotation
            transform.rotation = Quaternion.Lerp(Q_CurrRot, Q_NewRot, 0.1f);

            // If this minion is melee, move toward it.
            if(b_IsMelee)
            {
                // Move forward at set pace
                gameObject.GetComponent<Rigidbody>().velocity = (transform.forward * currentSpeed);
            }
            else // This minion is ranged and requires different actions to attack.
            {
                // NOTE: We don't move the minion while attacking, so no need to change velocity.
            }
        }
    }

    // I call this from within the Minion Collider since it's the only thing that knows when an enemy enters it.
    public void SetEnemyArray(GameObject[] enemyArray_)
    {
        // Replace previous objects in array with new array objects
        // (Done manually since otherwise objects remain null)
        for(var i = 0; i < enemyArray.Length; ++i)
        {
            enemyArray[i] = enemyArray_[i];
        }

        // Reset bool check
        b_IsEnemyArrayLoaded = false;

        // Only activates the bool check if there is an item in the array (should activate on i == 0)
        for (var i = 0; i < enemyArray.Length; ++i)
        {
            if (enemyArray[i] != null)
            {
                b_IsEnemyArrayLoaded = true;
                break;
            }
        }
    }

    void GoToCurrentCheckpoint()
    {
        // Set the y to be that of the minion (To resolve tilting)
        checkpoint[currCheckpoint].checkpointPos.y = gameObject.transform.position.y;

        // Rotate toward current checkpoint
        // Grab current rotation
        Quaternion Q_CurrRot = transform.rotation;

        // Grab rotation toward object (Yes this is messy)
        transform.LookAt(checkpoint[currCheckpoint].checkpointPos);
        Quaternion Q_NewRot = transform.rotation;

        // Lerp toward the new rotation
        transform.rotation = Quaternion.Lerp(Q_CurrRot, Q_NewRot, 0.1f);

        // Move forward at set pace
        gameObject.GetComponent<Rigidbody>().velocity = (transform.forward * currentSpeed);
    }
    
    // Used for Checkpoint use
    void OnTriggerEnter(Collider collider_)
    {
        // Checks against Checkpoints
        if (collider_.gameObject.name == checkpoint[currCheckpoint].checkpointName)
        {
            // Adds to the checkpoint counter if possible. Otherwise, destroys itself.
            if (checkpoint[currCheckpoint + 1] != null) ++currCheckpoint; else GameObject.Destroy(gameObject);
        }
    }

    // Used for Minion/Player/Tower use
    void OnCollisionEnter(Collision collision_)
    {
        if(collision_.gameObject.GetComponent<HealthSystem>())
        {
            b_IsPushed = true;

            currentSpeed = 0f;

            // If the object we touch is a minion...
            if(collision_.gameObject.GetComponent<HealthSystem>().GetObjectType() == ObjectTypes.Minion)
            {
                var myTeam = gameObject.GetComponent<HealthSystem>().GetTeamType();
                var myType = gameObject.GetComponent<HealthSystem>().GetObjectType();
                collision_.gameObject.GetComponent<HealthSystem>().ApplyDamage(myTeam, myType, i_AttackDamage);

                // If the minion is on the same team, we bounce sideways.
                if (collision_.gameObject.GetComponent<HealthSystem>().GetTeamType() == gameObject.GetComponent<HealthSystem>().GetTeamType())
                {
                    // Apply random left/right force
                    if (Random.value >= 0.5)
                    {
                        gameObject.GetComponent<Rigidbody>().velocity += transform.right * 1;
                    }
                    else
                    {
                        gameObject.GetComponent<Rigidbody>().velocity += transform.right * -1;
                    }
                }
                // Otherwise, the minion is on the other team.
                else
                {
                    gameObject.GetComponent<Rigidbody>().velocity = -transform.forward * 3;
                }
            }
        }
    }
}

