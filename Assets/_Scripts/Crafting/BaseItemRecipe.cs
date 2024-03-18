using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Recipe", menuName = "Scriptable Objects/Item Recipe")]
public class BaseItemRecipe : ScriptableObject
{
    public string recipeName;

    public ItemCount[] input;
    public ItemCount[] output;
}

[System.Serializable]
public class ItemCount
{
    public BaseItemRecipe item;
    public int count;

    public ItemCount(BaseItemRecipe i, int c)
    {
        item = i;
        count = c;
    }
}