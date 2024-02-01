using System.Collections;
using System.Collections.Generic;
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
    private void Update()
    {

        IconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            PickUpandMove();

        }
    }

    private void PickUpandMove()
    {
        Vector2Int gridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

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
          bool complete=  selectedItemGrid.storeItem(selectedItem, gridPosition.x, gridPosition.y, ref overLappingItem);

            if (complete)
            {
                selectedItem = null;
                if (overLappingItem != null){
                    selectedItem = overLappingItem;
                    selectedItemTransform = selectedItem.GetComponent<RectTransform>();
                    overLappingItem = null;
                }
            }

        }
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
