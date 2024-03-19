using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Recipe", menuName = "Scriptable Objects/Item Recipe")]
public class BaseItemRecipe : ScriptableObject
{
    public string recipeName;

    public ItemTypeAndCount[] input;
    public ItemTypeAndCount[] output;
}

[System.Serializable]
public class ItemTypeAndCount
{
    
    public Inventory_Item item;
    public int count;

    public ItemTypeAndCount(Inventory_Item i, int c)
    {
        item = i;
        //items.Add(i);
        count = c;
    }
}