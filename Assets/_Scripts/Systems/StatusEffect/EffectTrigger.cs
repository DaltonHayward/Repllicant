using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OWNER: Spencer Martin
/// This is a simple script to test effect activation, same logic could be applied to enemy attacks/spells
/// </summary>
public class EffectTrigger : MonoBehaviour
{
    [SerializeField] BaseEffect Effect;
   
    private void OnTriggerEnter(Collider other) 
    {
        // apply the effect if supported
        var effectableObject = other.GetComponent<EffectableObject>();

        if (effectableObject != null)
        {
            effectableObject.ApplyEffect(Effect);
        }
    }
}
