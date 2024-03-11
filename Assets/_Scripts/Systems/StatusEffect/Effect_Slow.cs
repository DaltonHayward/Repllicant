using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Slow", fileName = "Effect_Slow")]
public class Effect_Slow : BaseEffect
{
    [SerializeField] float MovementSpeedModifier = 0.5f;
    public override float Effect_MovementSpeed(float originalMovementSpeed)
    {
        return originalMovementSpeed * MovementSpeedModifier;
    }
}
