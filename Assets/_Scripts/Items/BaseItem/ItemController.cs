using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ItemController : MonoBehaviour
{
    private Boolean isPickedUp = false;
    public String Name;

    private ItemData itemData;


    private void Start()
    {
        itemData = InventoryController.instance.LookUpItem(Name);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (InputManager.instance.InteractInput)
            {

                //Inventory_Item newItem = Instantiate(InventoryController.instance.ItemPrefab).GetComponent<Inventory_Item>();
                if (!isPickedUp && InventoryController.playerInventory.CheckForFreeSpace(itemData))
                {
                    Debug.Log("Picked up " + itemData.Name + isPickedUp);
                    isPickedUp = true;
                    InventoryController.instance.InsertNewItem(itemData, InventoryController.playerInventory);
                    Debug.Log("Added " + itemData.Name + " to inventory");
                    CraftingManager.instance.inventoryInteraction.AddItemNotification(itemData.Name);
                    Debug.Log("Setting child to false");
                    transform.GetChild(0).gameObject.SetActive(false);
                    Debug.Log("Should destroy");
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(DisplayInventoryFullText());
                }

            }
        }
    }

    IEnumerator DisplayInventoryFullText()
    {
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "Inventory Full";
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        yield return new WaitForSeconds(1);
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "Pick Up [" + InputManager.instance.GetInteractKey() + "]";
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    void OnTriggerExit(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
