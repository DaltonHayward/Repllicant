using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
public class NewInventoryItem : MonoBehaviour
{
  
    private NewItem itemData;
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
    public int OnGridPositionX;
    public int OnGridPositionY;
    public bool rotated = false;


    public void Construct(NewItem itemData){
        this.itemData = itemData;
        GetComponent<Image>().sprite  = itemData.invIcon;
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
        // this.itemData = itemData;
        // GetComponent<Image>().sprite  = itemData.itemIcon;
        // Vector2 size = new Vector2();
        // size.x = itemData.width * ItemGrid.tileSizeWidth;
        // size.y = itemData.height * ItemGrid.tileSizeHeight;
        // GetComponent<RectTransform>().sizeDelta = size;
    }

}
