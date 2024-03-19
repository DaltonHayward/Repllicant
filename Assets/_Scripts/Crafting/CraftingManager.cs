using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Search;
using UnityEngine;


public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [SerializeField] BaseItemRecipe[] recipes;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] Transform recipeParent;

    public List<ItemTypeAndCount> items = new List<ItemTypeAndCount>();

    private ItemGrid itemsInInventory;

    private static CraftingManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Crafting Manager in the scene");
        }
        instance = this;
    }


    public void GetAllItems()
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

    }

    public void ItemCheck()
    {

        GetAllItems();
        int i = 0;

        foreach (ItemTypeAndCount ItemAndCount in items)
        {

            Debug.Log(items[i].name);

            Debug.Log(items[i].count);


            i++;
        }


    }

    private void Update()
    {
        ItemCheck();
    }


}
