using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    Item itemType;
    int condition; 
    int stackSize; 
    bool onFire; 

    public ItemInstance(Item itemType, int condition, int stackSize, bool onFire)
    {
        this.itemType = itemType;
        this.condition = condition;
        this.stackSize = stackSize;
        this.onFire = onFire;
    }
    public void DamageItem(float amount)
    {
        condition -= (int)amount;
        if (condition <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
