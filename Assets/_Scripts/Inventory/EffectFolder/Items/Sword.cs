using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Inventory_Item, Burnable
{
    // Start is called before the first frame update
    public int damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Burnable.ApplyEffect()
    {
        damage = damage * 2;
    }

    void Burnable.RemoveEffect()
    {
        throw new System.NotImplementedException();
    }
}
