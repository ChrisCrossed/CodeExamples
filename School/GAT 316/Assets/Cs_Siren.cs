using UnityEngine;
using System.Collections;

public class Cs_Siren : MonoBehaviour
{
    bool b_IsEnabled;
    bool b_SoundEnabled;
    bool b_PreviouslyActivated;

    GameObject go_LeftSirenModel;
    GameObject go_RightSirenModel;

    AudioSource as_AudioSource;
    AudioClip ac_Siren;
    AudioClip ac_AllClear;

    float f_RotSpeed = 135f;

    // Use this for initialization
    void Start ()
    {
        go_LeftSirenModel = transform.Find("Mdl_SoundVisual_Left").gameObject;
        go_RightSirenModel = transform.Find("Mdl_SoundVisual_Right").gameObject;

        as_AudioSource = gameObject.GetComponent<AudioSource>();
        ac_Siren = Resources.Load("SFX_Alarm") as AudioClip;
        ac_AllClear = Resources.Load("SFX_AllClear") as AudioClip;

        Set_Enabled = false;
    }

    public bool Set_Enabled
    {
        set
        {
            // Set the visual siren object
            go_LeftSirenModel.GetComponent<MeshRenderer>().enabled = value;
            go_RightSirenModel.GetComponent<MeshRenderer>().enabled = value;

            b_IsEnabled = value;

            // Resets the bool so the 'All Clear' plays when appropriate
            if(!value)
            {
                b_PreviouslyActivated = value;
            }

            // If at least one frame has passed and thus no audio has played yet...
            if(b_SoundEnabled)
            {
                // If the alarm hasn't been playing, play it
                if(!b_PreviouslyActivated)
                {
                    b_PreviouslyActivated = true;

                    // If we've enabled the siren, play the Alarm clip & loop it
                    if (b_IsEnabled)
                    {
                        as_AudioSource.Stop();
                        as_AudioSource.loop = true;
                        as_AudioSource.clip = ac_Siren;
                        as_AudioSource.Play();
                    }
                    // Otherwise, we turn off the alarm and play "All Clear" only once.
                    else
                    {
                        as_AudioSource.Stop();
                        as_AudioSource.loop = false;
                        as_AudioSource.clip = ac_AllClear;
                        as_AudioSource.Play();

                        b_PreviouslyActivated = false;
                    }
                }
            }

            // Forces the audio to wait at least one frame.
            b_SoundEnabled = true;
        }
        get
        {
           return b_IsEnabled;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if( b_IsEnabled )
        {
            // Rotate the model
            Vector3 v3_Rotation = gameObject.transform.eulerAngles;
            v3_Rotation.y += Time.deltaTime * f_RotSpeed;
            v3_Rotation.y = Mathf.Clamp(v3_Rotation.y, 0f, 360f);
            gameObject.transform.eulerAngles = v3_Rotation;
        }
	}
}
