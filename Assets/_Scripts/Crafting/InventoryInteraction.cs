using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// OWNER: Spencer Martin
/// Package of all necessary interactions with the inventory system from outside of the inventory system
/// Can create list of items in inventory, perform checks for required items and call InventoryController addition/removal functions
/// </summary>
public class InventoryInteraction : MonoBehaviour
{
    private ItemGrid itemsInInventory;

    ItemGrid _ItemGrid;

    CraftingManager _CraftingManager;


    public List<ItemTypeAndCount> items = new List<ItemTypeAndCount>();

    // cycle thru player inventory and create list of items and amounts
    public List<ItemTypeAndCount> GetAllItems()
    {
        itemsInInventory = InventoryController.playerInventory;
        items.Clear();


        for (int child = 0; child < itemsInInventory.transform.childCount; child++)
        {
            Inventory_Item itemType = itemsInInventory.transform.GetChild(child).GetComponent<Inventory_Item>();

            int i = 0;
            bool itemWasAdded = false;

            foreach (ItemTypeAndCount ItemAndCount in items)
            {
                if (ItemAndCount.name == itemType.itemData.Name)
                {
                    items[i].count++;
                    itemWasAdded = true;
                }
                i++;
            }
            if (!itemWasAdded)
            {
                items.Add(new ItemTypeAndCount(itemType.itemData.Name, itemType.itemData, 1));
            }
        }

        return items;
    }

    // takes list of type ItemTypeAndCount and checks if player's inventory contains those items - use this for checking crafting/shrine requirements
    public bool ItemCheck(ItemTypeAndCount[] itemsNeed)
    {
        List<ItemTypeAndCount> itemsHave = GetAllItems();
        int foundItems = 0;

        foreach (ItemTypeAndCount neededItemAndCount in itemsNeed)
        {
            foreach (ItemTypeAndCount foundItemAndCount in itemsHave)
            {
                if (foundItemAndCount.name == neededItemAndCount.name && foundItemAndCount.count >= neededItemAndCount.count)
                {
                    foundItems++;
                    break;
                }
            }
        }

        return foundItems == itemsNeed.Length;
    }

    public int ItemCountCheck(string itemName)
    {
        List<ItemTypeAndCount> items = GetAllItems();
        int itemCount = 0;

        foreach (ItemTypeAndCount item in items)
        {
            if (item.name == itemName)
            {
                itemCount = item.count;
                break;
            }
        }


        return itemCount;
    }

    // function for removing item(s) from player inventory
    public void RemoveInventoryItems(ItemTypeAndCount[] itemsToRemove)
    {
        ItemTypeAndCount[] items = itemsToRemove;
        foreach (ItemTypeAndCount item in items)
        {
            foreach (Inventory_Item i in _ItemGrid.invItemSlots)
            {
                if (i.itemData.Name == item.name && item.count > 0)
                {
                    InventoryController.playerInventory.RemoveItem(i);
                    Object.Destroy(i);
                    item.count--;
                }
            }
        }
    }



    // function for adding item(s) from player inventory
    public void AddInventoryItems(ItemData itemToAdd)
    {
        if (InventoryController.playerInventory.CheckForFreeSpace(itemToAdd))
        {
            InventoryController.instance.InsertNewItem(itemToAdd, InventoryController.playerInventory);
        }
        else
        {
            Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
            Object.Instantiate(InventoryController.instance.LookUpItem(itemToAdd.Name).worldModel, new Vector3(playerPos.x + Random.Range(-1f, 1f), 0.8f, playerPos.z + Random.Range(-1f, 1f)), Quaternion.identity);
        }

    }

    // function for crafting an item
    public void CraftItem(BaseItemRecipe itemRecipe)
    {
        // remove crafting ingredient items from inventory
        RemoveInventoryItems(itemRecipe.input);

        // add inventory items
        AddInventoryItems(itemRecipe.output);
    }


    // function for opening crafting menu
    public void OpenCrafting()
    {
        _CraftingManager.EnterCraftingMode();
        
    }
}
