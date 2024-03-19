using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryManager invController;
    Inventory inventory;
    /// <summary>
    /// finds type InventoryController, and also gets the item grid attached.
    /// </summary>
    private void Awake(){
        //Optimize at some point
        invController = FindObjectOfType(typeof(InventoryManager)) as InventoryManager;
        inventory = GetComponent<Inventory>();
        Debug.Log("Inventory Interact Awake");
    }
    /// <summary>
    /// Sets the selected item grid to the item grid that the pointer is over.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        invController.SelectedInventory = inventory;
        Debug.Log("Pointer Enter"); 
    }
    /// <summary>
    /// Sets the selected item grid to null when the pointer exits the item grid.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        invController.SelectedInventory = null;
    }
}
