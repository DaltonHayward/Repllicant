using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{
    [System.Serializable]
    public class ItemSprites
    {
        public string spriteName;
        public Sprite sprite;
    }

    // Start is called before the first frame update
    [Header("Item Fields")]
    public string Name;
    public int width;
    public int height;
    public bool isEquipable;
    public bool isConsumable;
    public float healAmount;
    public GameObject worldModel;
    public List<ItemSprites> sprites;
    [Header("Inventory Effect Fields")]
    public int range;
    public string[] effects;
    [Header("Tool Fields")]
    public GameObject equippedModel;
    public PlayerController.Equipment toolType;
    public string Description = null;
}
