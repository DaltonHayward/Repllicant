using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : Inventory_Item, Burnable
{
    // Start is called before the first frame update
    public int damage;

    void Burnable.ApplyEffect()
    {
        damage = damage * 2;
    }

    void Burnable.RemoveEffect()
    {
        throw new System.NotImplementedException();
    }
}
