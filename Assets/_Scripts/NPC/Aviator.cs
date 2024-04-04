using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aviator : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;
    [SerializeField] BaseItemRecipe recipe;

    public void SendToBase()
    {
        if (progressManager != null) { progressManager.aviator = true; }
        // check player inventory for reagents
        InventoryInteraction inventory = CraftingManager.instance.GetComponent<InventoryInteraction>();

        var item1 = new ItemTypeAndCount("SyntheticPlumCore", 1);
        Debug.Log("test");

        ItemTypeAndCount[] items = { item1 };
        Debug.Log(inventory.ItemCheck(items));
        if (inventory.ItemCheck(items))
        {
            inventory.CraftItem(recipe);
        }
    }
}
