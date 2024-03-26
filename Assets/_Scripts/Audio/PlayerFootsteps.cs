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
            
            //footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            //FMODUnity.RuntimeManager.AttachInstanceToGameObject(footsteps, transform);
            footsteps.start();
            
            //footsteps.release();
        }
        //footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
        
    }
}
