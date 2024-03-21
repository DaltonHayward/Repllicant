using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipes/Item_Recipe", fileName = "Item_Recipe")]
public class BaseItemRecipe : ScriptableObject
{
    public string recipeName;

    public ItemTypeAndCount[] input;
    public ItemData output;
}

[System.Serializable]
public class ItemTypeAndCount
{
    
    //public Inventory_Item item;
    public string name;
    
    public int count;

    public ItemTypeAndCount(string n, int c)
    {
        name = n;
        count = c;
    }
}