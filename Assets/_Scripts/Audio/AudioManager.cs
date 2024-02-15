using FMODUnity; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   [SerializeField] EventReference FootStepEvent;
   [SerializeField] float rate;
   [SerializeField] GameObject player;
   [SerializeField] EventReference SirenSongEvent;

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
