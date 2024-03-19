using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    public NewItem itemType;
    int condition; 
    int stackSize; 
    bool onFire; 

    public ItemInstance(NewItem itemType, int condition, int stackSize, bool onFire)
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

    public NewInventoryItem CollectItem(){
        Destroy(itemType.envModel);

        return itemType.invModel.GetComponent<NewInventoryItem>();
    }
    public GameObject DropItem(){
        Destroy(itemType.invModel);
        return itemType.envModel;
    }
}
