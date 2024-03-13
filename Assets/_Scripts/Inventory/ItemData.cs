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
    public GameObject invModel;
    public string Name; 
    void Start()
    {   
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void construct(int width, int height, Sprite itemIcon, GameObject prefab)
    {
        this.width = width;
        this.height = height;
        this.itemIcon = itemIcon;
        this.envModel = prefab;
    }
}
