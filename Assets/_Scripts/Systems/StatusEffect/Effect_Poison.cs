using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OWNER: Spencer Martin
/// Example effect, derives from effect base class BaseEffect, damages player via poison effect
/// Note use of overrides
/// </summary>
[CreateAssetMenu(menuName = "Effects/Poison", fileName = "Effect_Poison")]
public class Effect_Poison : BaseEffect
{
    [SerializeField] float DamageModifier = 0.999f;

    public override float Effect_PlayerHealth(float originalCurrentHealth)
    {
        Debug.Log("Poison Active");
        return originalCurrentHealth * DamageModifier;
    }
}
