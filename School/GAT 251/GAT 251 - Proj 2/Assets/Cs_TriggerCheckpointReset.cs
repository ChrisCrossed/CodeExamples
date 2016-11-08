using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_TriggerCheckpointReset : MonoBehaviour
{
    [SerializeField] int i_CheckpointPos;
    [SerializeField] bool b_Enabled = true;

    public void Set_Active( bool b_IsActive_ )
    {
        print("Activated");

        b_Enabled = b_IsActive_;
    }
    
    void OnTriggerStay( Collider collider_ )
    {
        if(b_Enabled)
        {
            if(collider_.gameObject.tag == "Player")
            {
                if(i_CheckpointPos == 0)
                {
                    // Reset level (TODO: Fix with fade)
                    SceneManager.LoadScene(i_CheckpointPos);
                }
                else
                {
                    // Move player to position based on i_CheckpointPos

                }
            }
        }
    }
}
