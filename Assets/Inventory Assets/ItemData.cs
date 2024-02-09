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
    public GameObject itemPrefab;

    void Start()
    {
        GameObject item = this.itemPrefab;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
