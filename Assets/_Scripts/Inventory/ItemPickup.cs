using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Boolean isPickedUp = false;
    void Start()
    {
        
    }
    void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true); ;
            if (Input.GetKey("l"))
            {
                if (!isPickedUp){
                    isPickedUp = true;
                    Inventory_Item item = InventoryController.instance.LookUp(this.gameObject.GetComponent<EnvItem>().Name);
                    InventoryController.instance.InsertNewItem(item,InventoryController.playerInventory);
                    
                    Destroy(this.gameObject);
                }

            }
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
