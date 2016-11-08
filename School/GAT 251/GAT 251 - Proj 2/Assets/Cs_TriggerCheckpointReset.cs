using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_TriggerCheckpointReset : MonoBehaviour
{
    [SerializeField] int i_CheckpointPos;
    [SerializeField] bool b_StartEnabled = true;

    void Start()
    {
        if(!b_StartEnabled)
        {
            print("Turning off: " + gameObject.name);

            gameObject.SetActive(false);
        }
    }
    
    void OnTriggerEnter( Collider collider_ )
    {
        if(collider_.gameObject.tag == "Player")
        {
            if(i_CheckpointPos == 0)
            {
                // Reset level
                SceneManager.LoadScene(i_CheckpointPos);
            }
            else
            {
                // Move player to position based on i_CheckpointPos

            }
        }
    }
}
