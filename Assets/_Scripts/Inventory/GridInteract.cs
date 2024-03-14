using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    InventoryController invController;
    ItemGrid itemGrid;
    /// <summary>
    /// finds type InventoryController, and also gets the item grid attached.
    /// </summary>
    private void Awake(){
        //Optimize at some point
        invController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }
    /// <summary>
    /// Sets the selected item grid to the item grid that the pointer is over.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {

        invController.SelectedItemGrid = itemGrid;
        invController.Droppable = false;
    }
    /// <summary>
    /// Sets the selected item grid to null when the pointer exits the item grid.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        invController.SelectedItemGrid = null;
        invController.Droppable = true; 
    }

  
}
