using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
  private bool isPickedUp = false;
    void Start()
    {
        
    }
    void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true); ;
            if (Input.GetKeyDown("l"))
            {
                if (!isPickedUp){
                    isPickedUp = true;
                    InventoryManager.instance.InsertNewItem(this.gameObject.GetComponent<EnvItem>().itemType,InventoryManager.playerInventory);
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
