using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Item : MonoBehaviour
{
    public Sprite itemIcon;

    public Sprite WeaponOverlay;
    public Sprite PickaxeOverlay;
    public Sprite AxeOverlay;
    public Image overlayImage;

    public Sprite WeaponOutline;
    public Sprite PickaxeOutline;
    public Sprite AxeOutline;
    public Image outlineImage;
    
    public ItemData itemData;
    public string itemName;

    public int OnGridPositionX;
    public int OnGridPositionY;
    private bool rotated = false;

    public bool isEquipped;

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


    /// <summary>
    /// Rotates the inventory item.
    /// <summary>
    internal void Rotate()
    {
        rotated = !rotated;
        RectTransform rt = GetComponent<RectTransform>();
        rt.rotation= Quaternion.Euler(0, 0, rotated ? 90 : 0);
    }

    /// <summary>
    /// Sets the item data and updates the UI elements accordingly.
    /// </summary>
    /// <param name="itemData">The item data to set.</param>
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        itemName = itemData.Name;
        itemIcon = itemData.sprites[0].sprite;
        GetComponent<Image>().sprite = itemIcon;
        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
        // set size of outline
        GetComponentsInChildren<RectTransform>()[1].sizeDelta = size; 
        // add effect script
        gameObject.AddComponent(Type.GetType(itemData.Name));

        // set up equip sprites depending on item type
        if (itemData.isEquipable)
        {
            switch (itemData.toolType)
            {
                case PlayerController.Equipment.WEAPON:
                    overlayImage.sprite = WeaponOverlay;
                    outlineImage.sprite = WeaponOutline;
                    break;

                case PlayerController.Equipment.PICKAXE:
                    overlayImage.sprite = PickaxeOverlay;
                    outlineImage.sprite = PickaxeOutline;
                    break;

                case PlayerController.Equipment.AXE:
                    overlayImage.sprite = AxeOverlay;
                    outlineImage.sprite = AxeOutline;
                    break;

                default:
                    break;
            }
        }

        Unequip();

        //GetComponent<Image>().SetNativeSize();
    }

    public void UpdateSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }

    public void Equip()
    {
        isEquipped = true;
        overlayImage.enabled = true;
        outlineImage.enabled = true;
    }

    public void Unequip()
    {
        isEquipped = false;  
        overlayImage.enabled = false;
        outlineImage.enabled = false;
    }


}
