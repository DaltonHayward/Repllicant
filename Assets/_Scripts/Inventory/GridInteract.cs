using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    InventoryController invController;
    ItemGrid itemGrid;
    
    private void Awake(){
        //Optimize at some point
        invController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        invController.SelectedItemGrid = itemGrid;
        // throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        invController.SelectedItemGrid = null;
        // throw new System.NotImplementedException();
    }

  
}
