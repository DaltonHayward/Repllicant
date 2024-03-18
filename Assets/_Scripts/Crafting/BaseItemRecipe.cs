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
    public BaseItemRecipe item;
    public int count;

    public ItemTypeAndCount(BaseItemRecipe i, int c)
    {
        item = i;
        count = c;
    }
}