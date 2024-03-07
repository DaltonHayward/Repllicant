using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Collectible
{
    public float hardnessPercent = 0.2f; // reduces damage taken by a percentage

    // probably want some sort of way to penetrate hardness with a good pickaxe

    override public void TakeDamage(float damage)
    {
        if (hardnessPercent < 1) 
        {
            hp -= (damage - (damage * hardnessPercent));
        }

        if (hp <= 0) 
        {
            InstantiateLoot();
            Destroy(gameObject);
        } 
    }
}
