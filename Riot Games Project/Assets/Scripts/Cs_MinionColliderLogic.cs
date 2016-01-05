using UnityEngine;
using System.Collections;

public class Cs_MinionColliderLogic : MonoBehaviour
{
    GameObject[] EnemyArray = new GameObject[10];
    TeamTypes teamType;

	// Use this for initialization
	void Start ()
    {
        // Turn off the visual component
        gameObject.GetComponent<Renderer>().enabled = false;
        GetTeamType();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    void GetTeamType()
    {
        teamType = gameObject.GetComponentInParent<HealthSystem>().GetTeamType();
    }

    void ArrangeEnemyArray_BubbleSort()
    {
        int firstNull = EnemyArray.Length;
        // Reverse through the array to find the first null object
        for(var i = EnemyArray.Length - 1; i > 0; --i)
        {
            if (EnemyArray[i] == null) firstNull = i;
        }

        int lastGameObject = 0;
        // Run through the array to find the last GameObject
        for(var j = 0; j < EnemyArray.Length; ++j)
        {
            if (EnemyArray[j] != null) lastGameObject = j;
        }

        // If the first null object is less than the last game object's position, then swap and restart.
        if(firstNull < lastGameObject)
        {
            // Move the the game object to the earlier position in the array
            EnemyArray[firstNull] = EnemyArray[lastGameObject];

            // Make the previous GameObject position null
            EnemyArray[lastGameObject] = null;

            // Re-run the function
            ArrangeEnemyArray_BubbleSort();
        }

        // Finally, pass the enemy array to the minion
        
        gameObject.GetComponentInParent<Cs_MinionLogic>().SetEnemyArray(EnemyArray);

    }

    // Should be triggered when an object's collider touches the minion's collider
    void OnTriggerEnter(Collider collider_)
    {
        if(collider_.GetComponent<HealthSystem>())
        {
            if (collider_.GetComponent<HealthSystem>().GetTeamType() != teamType)
            {
                for (var i = 0; i < EnemyArray.Length; ++i)
                {
                    if (EnemyArray[i] == collider_.gameObject) break;

                    if (EnemyArray[i] == null)
                    {
                        EnemyArray[i] = collider_.gameObject;
                        ArrangeEnemyArray_BubbleSort();
                        break;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision_)
    {
        /*
        if (collision_.gameObject.GetComponent<HealthSystem>())
        {
            if (collision_.gameObject.GetComponent<HealthSystem>().GetTeamType() != teamType)
            {
                for(var i = 0; i < EnemyArray.Length; ++i)
                {
                    if (EnemyArray[i] == collision_.gameObject) break;

                    if(EnemyArray[i] == null)
                    {
                        EnemyArray[i] = collision_.gameObject;
                        ArrangeEnemyArray_BubbleSort();
                        
                        break;
                    }
                }
            }
        }
        */
    }
}
