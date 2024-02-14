using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// base abstract class for other bridge classes
/// </summary>

public abstract class TimeBridge_Base : MonoBehaviour
{
    public abstract void OnTick(float CurrentTime);
    
}
