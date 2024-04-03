using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// OWNER: Spencer Martin
/// Example effect, derives from effect base class BaseEffect, heals player
/// Note use of overrides
/// </summary>
/// 
[CreateAssetMenu(menuName = "Effects/Heal", fileName = "Effect_Heal")]
public class Effect_Heal : BaseEffect
{
    [SerializeField] float HealthModifier = 1.1f;

    public override float Effect_PlayerHealth(float originalCurrentHealth)
    {
        Debug.Log("Heal Active");
        return originalCurrentHealth * HealthModifier;
    }
}
