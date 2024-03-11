using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : ScriptableObject
{
    [SerializeField] protected float Duration = 0f;

    public bool IsActive => DurationRemaining > 0f;

    float DurationRemaining = 0f;

    public void EnableEffect()
    {
        DurationRemaining = Duration;
    }
    public void TickEffect()
    {
        // tick the effect
        if (DurationRemaining > 0)
        {
            DurationRemaining -= Time.deltaTime;
        }
    }

    // example effect
    public virtual float Effect_MovementSpeed(float originalMovementSpeed)
    {
        return originalMovementSpeed;
    }
}
