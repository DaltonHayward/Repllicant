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

    [SerializeField] private FMODUnity.EventReference _attack01;
    private FMOD.Studio.EventInstance attack01;

    [SerializeField] private FMODUnity.EventReference _toolswing;
    private FMOD.Studio.EventInstance toolswing;


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

        if (! _attack01.IsNull)
        {
            
            attack01 = FMODUnity.RuntimeManager.CreateInstance(_attack01);
        }

        if (! _toolswing.IsNull)
        {
            
            toolswing = FMODUnity.RuntimeManager.CreateInstance(_toolswing);
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
        if (attack01.isValid())
        {
            attack01.start();
        }

    }

    public void PlayAttack02()
    {
        
    }

    public void PlayToolSwing()
    {
        if (toolswing.isValid())
        {
            toolswing.start();
        }
    }

    public void PlayHurt()
    {

    }

    
}
