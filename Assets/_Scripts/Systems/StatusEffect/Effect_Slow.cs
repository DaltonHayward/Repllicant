using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OWNER: Spencer Martin
/// Example effect, derives from effect base class BaseEffect, slows player movement
/// Note use of overrides
/// </summary>
[CreateAssetMenu(menuName = "Effects/Slow", fileName = "Effect_Slow")]
public class Effect_Slow : BaseEffect
{
    [SerializeField] float MovementSpeedModifier = 0.5f;
    public override float Effect_MovementSpeed(float originalMovementSpeed)
    {
        return originalMovementSpeed * MovementSpeedModifier;
    }
}
