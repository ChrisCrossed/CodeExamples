using UnityEngine;
using System.Collections;

public class Cs_RadioLogic : MonoBehaviour
{
    AudioClip ac_Russia;
    AudioClip ac_Static;
    AudioClip ac_Mus_1;
    AudioClip ac_Mus_2;
    AudioClip ac_Mus_3;
    AudioClip ac_Mus_4;
    AudioClip ac_Mus_5;

    AudioSource as_Source;

    int i_SongNum;

    // Use this for initialization
    void Start ()
    {
        ac_Mus_1 = Resources.Load("mus_1") as AudioClip;
        ac_Mus_2 = Resources.Load("mus_2") as AudioClip;
        ac_Mus_3 = Resources.Load("mus_3") as AudioClip;
        ac_Mus_4 = Resources.Load("mus_4") as AudioClip;
        ac_Mus_5 = Resources.Load("mus_5") as AudioClip;
        ac_Russia = Resources.Load("mus_Anthem") as AudioClip;
        ac_Static = Resources.Load("SFX_Static") as AudioClip;

        as_Source = gameObject.GetComponent<AudioSource>();

        Set_ResetRadio();
    }

    public void Set_ResetRadio()
    {
        i_SongNum = Random.Range(0, 3);
    }

    float f_Timer;
    bool b_IsMusic;
    bool b_StaticPlaying;
    public void Use()
    {
        f_Timer = 0f;
        b_IsMusic = false;
        b_StaticPlaying = false;
        if(i_SongNum < 5) ++i_SongNum;
    }

	// Update is called once per frame
	void Update ()
    {
        if(f_Timer < 0.25f)
        {
            f_Timer += Time.deltaTime;

            if(!b_StaticPlaying)
            {
                b_StaticPlaying = true;
                as_Source.clip = ac_Static;
                as_Source.Play();
            }
        }
        else
        {
            if(!b_IsMusic)
            {
                b_IsMusic = true;

                // Find random song to play that isn't this one
                if (i_SongNum == 0) as_Source.clip = ac_Mus_1;
                else if (i_SongNum == 1) as_Source.clip = ac_Mus_2;
                else if (i_SongNum == 2) as_Source.clip = ac_Mus_3;
                else if (i_SongNum == 3) as_Source.clip = ac_Mus_4;
                else if (i_SongNum == 4) as_Source.clip = ac_Mus_5;
                else if (i_SongNum == 5)
                {
                    as_Source.clip = ac_Russia;

                    // Connect to Objective and tell it we're done
                    // gameObject.GetComponent<Cs_Objective>().
                }

                as_Source.Play();
            }
        }
	}
}
