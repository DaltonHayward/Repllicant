using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PlayerSounds : MonoBehaviour
{
    
    [SerializeField] private FMODUnity.EventReference _footsteps;
    private FMOD.Studio.EventInstance footsteps;

    [SerializeField] private FMODUnity.EventReference _dodge;
    private FMOD.Studio.EventInstance dodge;

    private void Awake() 
    {
        if (! _footsteps.IsNull)
        {
            
            footsteps = FMODUnity.RuntimeManager.CreateInstance(_footsteps);
        }

        if (! _dodge.IsNull)
        {
            
            dodge = FMODUnity.RuntimeManager.CreateInstance(_dodge);
        }
    }

    public void PlayFootstep()
    {
        if (footsteps.isValid())
        {
            footsteps.start();
        }
        
    }

    public void PlayFootstepRunning()
    {

    }

    public void PlayDodge()
    {
        if (dodge.isValid())
        {
            dodge.start();
        }
        
    }

    public void PlayAttack01()
    {

    }

    public void PlayAttack02()
    {
        
    }

    public void PlayToolSwing()
    {

    }

    public void PlayHurt()
    {

    }

    
}
