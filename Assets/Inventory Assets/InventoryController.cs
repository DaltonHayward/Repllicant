using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid { get => selectedItemGrid; set{
        selectedItemGrid = value;
        InventoryHighlight.setParent(selectedItemGrid);
    } }

    Inventory_Item selectedItem;

    Inventory_Item overLappingItem;
    RectTransform selectedItemTransform;    

    [SerializeField] List<ItemData2> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    InventoryHighlight InventoryHighlight;
    Inventory_Item itemToHighlight;
    Vector2 oldPosition;


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

       
        } if(Input.GetKeyDown(KeyCode.F)){

            CreateRandomItem();
            Inventory_Item itemtoInsert = selectedItem;
            Debug.Log(itemtoInsert);
            selectedItem=null;
            InsertItem(itemtoInsert);
        }
        if(Input.GetKeyDown(KeyCode.R)){
            if(selectedItem!=null){
                RotateItem();
            }
        }
    }

    private void RotateItem()
    {
        selectedItem.Rotate();
    }

    private void InsertItem(Inventory_Item itemtoInsert)
    {
        Vector2Int? storePos = selectedItemGrid.FindSpace(itemtoInsert);
        if (storePos != null)
        {
            selectedItemGrid.putItemInInventory(itemtoInsert, storePos.Value.x, storePos.Value.y);
        }
        else
        {
           return;
        }
    }

    private void HandleHighlight()
    {
        Vector2Int gridPosition = mouseToGridTranslation();
        if (gridPosition == oldPosition) { return; }

        oldPosition=gridPosition;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(gridPosition.x, gridPosition.y);
            if (itemToHighlight != null)
            {
                InventoryHighlight.Display(true);
                InventoryHighlight.Highlight(itemToHighlight);
                InventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else{
                InventoryHighlight.Display(false);
            }
            
        }
        else{
            InventoryHighlight.Display(selectedItemGrid.BoundryCheck(gridPosition.x, gridPosition.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            InventoryHighlight.Highlight(selectedItem);
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
            mousePosition.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            mousePosition.y += (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;


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
