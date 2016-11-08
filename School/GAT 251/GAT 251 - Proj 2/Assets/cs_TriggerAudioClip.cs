using UnityEngine;
using System.Collections;

public class cs_TriggerAudioClip : MonoBehaviour
{
    [SerializeField] int i_Clip;

    [SerializeField] bool b_ActivateTriggerSoon;

    [SerializeField] GameObject go_TriggerToActivate;

    float f_Timer;
    void Update()
    {
        if(b_ActivateTriggerSoon)
        {
            f_Timer += Time.deltaTime;

            if(f_Timer > 5.0f)
            {
                go_TriggerToActivate.SetActive(true);
            }
        }
    }
    
    void OnTriggerEnter( Collider collider_ )
    {
        if(collider_.gameObject.tag == "Player")
        {
            collider_.gameObject.GetComponent<Cs_SkiingPlayerController>().PlayAudioClip(i_Clip);

            b_ActivateTriggerSoon = true;

            GameObject.Destroy(gameObject);
        }
    }
}
