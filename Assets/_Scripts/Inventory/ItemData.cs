using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [System.Serializable]
    public class ItemSprites
    {
        public string spriteName;
        public Sprite sprite;
    }

    // Start is called before the first frame update
    [Header("Inventory")]
    public string Name;
    public int width;
    public int height;
    public List<ItemSprites> sprites;
    public GameObject envModel;
    [Header("Effects")]
    public string[] effects;
    public int range;
    [Header("Tools")]
    public bool isEquipable;
    public PlayerController.Equipment toolType;
}
