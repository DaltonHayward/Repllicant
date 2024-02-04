using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ItemGrid selectedItemGrid;
    Inventory_Item selectedItem;

    Inventory_Item overLappingItem;
    RectTransform selectedItemTransform;    

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    InventoryHighlight InventoryHighlight;
    Inventory_Item itemToHighlight;
    private void Awake() {
        InventoryHighlight= GetComponent<InventoryHighlight>();    
    }

    private void Update()
    {

        IconDrag();
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null) { 
            InventoryHighlight.Display(false);
            return; }
        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            PickUpandMove();

        }
    }

    private void HandleHighlight()
    {
        Vector2Int gridPosition = mouseToGridTranslation();
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(gridPosition.x, gridPosition.y);
            if (itemToHighlight != null)
            {
                InventoryHighlight.Display(true);
                InventoryHighlight.Highlight(itemToHighlight);
                InventoryHighlight.setParent(selectedItemGrid);
                InventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else{
                InventoryHighlight.Display(false);
            }
            
        }
        else{
            InventoryHighlight.Display(true);
            InventoryHighlight.Highlight(selectedItem);
            InventoryHighlight.setParent(selectedItemGrid);
            InventoryHighlight.SetPosition(selectedItemGrid, selectedItem, gridPosition.x, gridPosition.y);
            
        }
    }

    private void PickUpandMove()
    {
        Vector2Int gridPosition = mouseToGridTranslation();

        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.PickUpItem(gridPosition.x, gridPosition.y);

            if (selectedItem != null)
            {
                selectedItemTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        else
        {
            bool complete = selectedItemGrid.storeItem(selectedItem, gridPosition.x, gridPosition.y, ref overLappingItem);

            if (complete)
            {
                selectedItem = null;
                if (overLappingItem != null)
                {
                    selectedItem = overLappingItem;
                    selectedItemTransform = selectedItem.GetComponent<RectTransform>();
                    overLappingItem = null;
                }
            }

        }
    }

    private Vector2Int mouseToGridTranslation()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (selectedItem != null)
        {
            mousePosition.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            mousePosition.y += (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;


        }
        Vector2Int gridPosition = selectedItemGrid.GetTileGridPosition(mousePosition);
        return gridPosition;
    }

    private void IconDrag()
    {
        if (selectedItem != null)
        {
            selectedItemTransform.position = Input.mousePosition;
        }
    }

    private void CreateRandomItem(){
        Inventory_Item newItem= Instantiate(itemPrefab).GetComponent<Inventory_Item>();
        selectedItem = newItem;

        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);

        int selectedUID= Random.Range(0, items.Count);
        newItem.Set(items[selectedUID]);

    }



}
