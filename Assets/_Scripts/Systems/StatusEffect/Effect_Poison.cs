using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OWNER: Spencer Martin
/// Example effect, derives from effect base class BaseEffect, slows player movement
/// Note use of overrides
/// </summary>
[CreateAssetMenu(menuName = "Effects/Poison", fileName = "Effect_Poison")]
public class Effect_Poison : BaseEffect
{
    [SerializeField] float DamageModifier = 0.99f;
    public override float Effect_PlayerHealth(float originalCurrentHealth)
    {
        return originalCurrentHealth * DamageModifier;
    }
}
