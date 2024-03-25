using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OWNER: Spencer Martin
/// Anything with this script attached to it can have effects applied to it
/// To add new effect, copy from example effect and make sure to add a virtual method to BaseEffect
/// Once this is done, you can create a scriptable object that will apply effect 
/// NOTE: check Effect_Slow.cs for an example of this implementation
/// </summary>
public class EffectableObject : MonoBehaviour
{
    List<BaseEffect> ActiveEffects = new List<BaseEffect>();

    // Update is called once per frame
    void Update()
    {
        // tick active effects - cleanup any that are finished (loops backwards on purpose)
        for(int index = ActiveEffects.Count -1; index >= 0; --index)
        {
            ActiveEffects[index].TickEffect();

            // check if effect is finished
            if (!ActiveEffects[index].IsActive)
            {
                ActiveEffects.RemoveAt(index);
            }
        }
    }

    public void ApplyEffect(BaseEffect effectTemplate)
    {
        // create a new instance
        var newEffect = ScriptableObject.Instantiate(effectTemplate);

        // make the effect active
        newEffect.EnableEffect(); 
        ActiveEffects.Add(newEffect);
    }
    
    // example effect
    public float Effect_MovementSpeed(float originalMovementSpeed)
    {
        float workingMovementSpeed = originalMovementSpeed;

        // apply any active effects
        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }
            workingMovementSpeed = ActiveEffects[index].Effect_MovementSpeed(workingMovementSpeed);
        }
        return workingMovementSpeed;
    }

    public float Effect_PlayerHealth(float originalCurrentHealth)
    {
        float workingCurrentHealth = originalCurrentHealth;
        // apply any active effects
        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }
            workingCurrentHealth = ActiveEffects[index].Effect_PlayerHealth(workingCurrentHealth);
        }

        return workingCurrentHealth;
    }
}
