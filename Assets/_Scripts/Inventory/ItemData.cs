using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    // Start is called before the first frame update
    public int width;
    public int height;
    public Sprite itemIcon;
    public GameObject envModel;
    public string Name; 
    public string Effect;
    public Inventory_Item script;

    public int range; 

    public void construct(int width, int height, Sprite itemIcon, GameObject prefab, string Effect)
    {
        this.width = width;
        this.height = height;
        this.itemIcon = itemIcon;
        this.envModel = prefab;
        this.Effect = Effect;
    }
}
