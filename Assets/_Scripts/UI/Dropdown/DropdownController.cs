using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    public static DropdownController instance;

    private GameObject dropdown;

    public GameObject buttonPrefab;

    private Inventory_Item _clickedOnItem; 


    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        dropdown = transform.GetChild(0).gameObject;
    }

    public void SetClickedItem(Inventory_Item item)
    {
        _clickedOnItem = item;
    }

    public Inventory_Item GetClickedOnItem()
    {
        return _clickedOnItem;
    }

    public void Show()
    {
        dropdown.SetActive(true);

        RemoveChildren();

        

        if (_clickedOnItem != null && _clickedOnItem.itemData.isEquipable && !_clickedOnItem.isEquipped)
        {
            GameObject equipButton = Instantiate(buttonPrefab, dropdown.transform);
            equipButton.GetComponent<Button>().onClick.AddListener(() => InventoryController.instance.EquipTool(_clickedOnItem));
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
        }
        else if (_clickedOnItem != null && _clickedOnItem.itemData.isEquipable && _clickedOnItem.isEquipped)
        {
            GameObject unequipButton = Instantiate(buttonPrefab, dropdown.transform);
            unequipButton.GetComponent<Button>().onClick.AddListener(() => InventoryController.instance.UnequipTool(_clickedOnItem));
            unequipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
        }

        GameObject discardButton = Instantiate(buttonPrefab, dropdown.transform);
        discardButton.GetComponent<Button>().onClick.AddListener(() => InventoryController.instance.DropItem());
        discardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Discard";


        Vector2 position = Input.mousePosition;

        instance.dropdown.transform.position = position;
    }

    public void Hide() 
    {
        RemoveChildren();
        dropdown.SetActive(false);
    }   

    /// <summary>
    /// Remove all children from dropdown
    /// </summary>
    private void RemoveChildren()
    {
        if (dropdown.transform.childCount != 0)
        {
            for (int i = dropdown.transform.childCount - 1 ; i >= 0; i--)
            {
                Destroy(dropdown.transform.GetChild(i).gameObject);
            }
        }
    }

    public bool DropdownEnabled()
    {
        return dropdown.activeInHierarchy;
    }
}
