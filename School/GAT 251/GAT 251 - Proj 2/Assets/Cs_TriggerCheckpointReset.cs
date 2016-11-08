using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_TriggerCheckpointReset : MonoBehaviour
{
    [SerializeField] int i_CheckpointPos;
    
    void OnTriggerEnter( Collider collider_ )
    {
        if(collider_.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }
    }
}
