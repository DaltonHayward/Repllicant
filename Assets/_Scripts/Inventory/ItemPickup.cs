using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Item : MonoBehaviour
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
                    InventoryController.instance.InsertNewItem(this.gameObject.GetComponent<Inventory_Item>(),InventoryController.playerInventory);
                    
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
