using UnityEngine;
using System.Collections;

public class cs_TriggerAudioClip : MonoBehaviour
{
    [SerializeField] int i_Clip;
    
    void OnTriggerEnter( Collider collider_ )
    {
        if(collider_.gameObject.tag == "Player")
        {
            collider_.gameObject.GetComponent<Cs_SkiingPlayerController>().PlayAudioClip(i_Clip);

            GameObject.Destroy(gameObject);
        }
    }
}
