using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public ItemInstance itemInstance;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public ItemInstance CollectItem()
    {
        return itemInstance;
    }
    public ItemInstance DropItem()
    {
        return itemInstance;
    }
    void DamageItem(float amount){
        itemInstance.DamageItem(amount);

    }
    void ApplyInventoryEffect(Effect effect){

    }

    public class ItemInstance : MonoBehaviour
{
    NewItem itemType;
    int condition; 
    int stackSize; 
    bool onFire; 

    public ItemInstance(NewItem itemType, int condition, int stackSize)
    {
        this.itemType = itemType;
        this.condition = condition;
        this.stackSize = stackSize;
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

    
}
