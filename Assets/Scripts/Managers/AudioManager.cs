using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip SplitSFX;
    public AudioClip PerfectSFX;


    public void PlaySplitSFX()
    {
        AudioSource _as = ObjectPooler.instance.GetPooledObject("AudioSource", Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
        _as.clip = SplitSFX;
        _as.Play();
        

    }


    public void PlayPerfectSFX(int PerfectCounter)
    {
        AudioSource _as = ObjectPooler.instance.GetPooledObject("AudioSource", Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
        _as.clip = PerfectSFX;
        _as.pitch = 1 + ((PerfectCounter -1 )* 0.1f);
        _as.Play();        

    }

}
