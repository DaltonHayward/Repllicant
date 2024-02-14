using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalyMisic : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip[]  allclip;
     public void PlayJump()
    {
        audiosource.clip = allclip[0];
        audiosource.Play();
    }
    public void PlayHit()
    {
        audiosource.clip = allclip[1];
        audiosource.Play();
    }


 
}
