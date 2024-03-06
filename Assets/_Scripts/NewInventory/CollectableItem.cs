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
        return null;
    }
    void DamageItem(float amount){
        itemInstance.DamageItem(amount);

    }
    void ApplyInventoryEffect(Effect effect){

    }
    
}
