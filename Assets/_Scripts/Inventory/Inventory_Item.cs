using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Item : MonoBehaviour, ISubscriber
{
    public Sprite itemIcon;
    public ItemData itemData;
    public string itemName;

    public int OnGridPositionX;
    public int OnGridPositionY;
    private bool rotated = false;


    public int HEIGHT {
        get {
            if (rotated)
            {
                return itemData.width;
            }
            else
            {
                return itemData.height;
            }
        }
    }
    public int WIDTH
    {
        get
        {
            if (rotated)
            {
                return itemData.height;
            }
            else
            {
                return itemData.width;
            }
        }
    }

    public void ReceiveMessage(string channel)
    {
        if (channel == "Burning"){
            Debug.Log("Item is burning");   
            Destroy(this.gameObject);
            InventoryController.instance.RemoveItem(this);
        }
    }


    /// <summary>
    /// Rotates the inventory item.
    /// <summary>
    internal void Rotate()
    {
        rotated = !rotated;
        RectTransform rt = GetComponent<RectTransform>();
        rt.rotation= Quaternion.Euler(0,0,rotated?90:0);
    }

    /// <summary>
    /// Sets the item data and updates the UI elements accordingly.
    /// </summary>
    /// <param name="itemData">The item data to set.</param>
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        itemName = itemData.Name;
        itemIcon = itemData.itemIcon;
        GetComponent<Image>().sprite = itemIcon;
        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
        GetComponent<Image>().SetNativeSize();
    }

   
}
