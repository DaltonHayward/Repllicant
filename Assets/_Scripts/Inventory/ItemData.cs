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
    public int width;
    public int height;
    public List<ItemSprites> sprites;
    public GameObject envModel;
    public string Name;
    public string[] effects;
    public int range;
    public bool isEquipable;
}
