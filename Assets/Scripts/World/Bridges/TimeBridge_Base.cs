using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class for other bridge classes
public abstract class TimeBridge_Base : MonoBehaviour
{
    public abstract void OnTick(float currentTime);
    
}
