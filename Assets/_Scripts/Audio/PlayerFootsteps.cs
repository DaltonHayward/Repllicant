using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PlayerFootsteps : MonoBehaviour
{
    
    [SerializeField] private FMODUnity.EventReference _footsteps;
    private FMOD.Studio.EventInstance footsteps;

    private void Awake() 
    {
        if (! _footsteps.IsNull)
        {
            
            footsteps = FMODUnity.RuntimeManager.CreateInstance(_footsteps);
        }
    }

    public void PlayFootstep()
    {
        if (footsteps.isValid())
        {
            footsteps.start();
        }
       
        
    }
}
