using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
     [HideInInspector]
    private Inventory selectedInventory;
    public Inventory SelectedInventory { get => selectedInventory; set{
        selectedInventory = value;
        InventoryHighlight.setParent(selectedInventory);
    } }
    
    public GameObject Player;
    NewInventoryItem selectedItem;

    NewInventoryItem overLappingItem;
    RectTransform selectedItemTransform;    

    [SerializeField] List<NewItem> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    NewInvHighlight InventoryHighlight;
    NewInventoryItem itemToHighlight;
    Vector2 oldPosition;
    public GameObject staticPlayerInventory;
   
    public static Inventory playerInventory;
    public static InventoryManager instance;


    /// <summary>
    /// Called when the script instance is being loaded. Responsible for doing singleton logic.
    /// sets inventory highlight and player inventory.
    /// </summary>
    private void Awake() {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        InventoryHighlight= GetComponent<NewInvHighlight>();
        playerInventory = staticPlayerInventory.GetComponent<Inventory>();
    }

    /// <summary>
    /// This method is called once per frame and is responsible for updating the inventory controller.
    /// It handles icon dragging, creating random items, highlighting the selected item, picking up and moving items,
    /// inserting items, and rotating items.
    /// </summary>
    private void Update()
    {

        IconDrag();
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedInventory == null) { 
            InventoryHighlight.Display(false);
            return; }
        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            PickUpandMove();

       
        } if(Input.GetKeyDown(KeyCode.F)){

            CreateRandomItem();
            NewInventoryItem itemtoInsert = selectedItem;
            Debug.Log("Item to insert: " + itemtoInsert);
            selectedItem=null;
            InsertItem(itemtoInsert);
        }
        if(Input.GetKeyDown(KeyCode.R)){
            if(selectedItem!=null){
                RotateItem();
            }
        }
    }
    /// <summary>
    /// Rotates the selected item.
    /// </summary>
    private void RotateItem()
    {
        selectedItem.Rotate();
    }

    /// <summary>
    /// Inserts an item into the inventory as long as there is space.
    /// </summary>
    /// <param name="itemtoInsert">The item to insert.</param>
    public void InsertItem(NewInventoryItem itemtoInsert)
    {
        Vector2Int? storePos = selectedInventory.FindSpace(itemtoInsert);
        if (storePos != null)
        {
            selectedInventory.putItemInInventory(itemtoInsert, storePos.Value.x, storePos.Value.y);
        }
        else
        {
           return;
        }
    }

    /// <summary>
    /// Handles the highlighting logic for the inventory controller highlighting.
    /// by grabbing the item, and highlight the grid that the item exists in. 
    /// </summary>
    private void HandleHighlight()
    {
        Vector2Int gridPosition = mouseToGridTranslation();
        if (gridPosition == oldPosition) { return; }

        oldPosition=gridPosition;
        if (selectedItem == null)
        {
            itemToHighlight = selectedInventory.GetItem(gridPosition.x, gridPosition.y);
            if (itemToHighlight != null)
            {
                InventoryHighlight.Display(true);
                InventoryHighlight.Highlight(itemToHighlight);
                InventoryHighlight.SetPosition(selectedInventory, itemToHighlight);
            }
            else{
                InventoryHighlight.Display(false);
            }
            
        }
        else{
            InventoryHighlight.Display(selectedInventory.BoundryCheck(gridPosition.x, gridPosition.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            InventoryHighlight.Highlight(selectedItem);
            InventoryHighlight.SetPosition(selectedInventory, selectedItem, gridPosition.x, gridPosition.y);
            
        }
    }

    /// <summary>
    /// Picks up and moves an item in the inventory grid using mouse position, and stroing the item in 
    /// selected item grid.
    /// </summary>
    private void PickUpandMove()
    {
        Vector2Int gridPosition = mouseToGridTranslation();
        if (selectedItem == null)
        {
            selectedItem = selectedInventory.PickUpItem(gridPosition.x, gridPosition.y);

            if (selectedItem != null)
            {
                selectedItemTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        else
        {
            bool complete = selectedInventory.storeItem(selectedItem, gridPosition.x, gridPosition.y, ref overLappingItem);


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

    /// <summary>
    /// Translates the mouse position to grid position and returns it as a Vector2Int.
    /// </summary>
    /// <returns>The grid position as a Vector2Int type.</returns>
    private Vector2Int mouseToGridTranslation()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (selectedItem != null)
        {
            mousePosition.x -= (selectedItem.WIDTH - 1) * Inventory.tileSizeWidth / 2;
            mousePosition.y += (selectedItem.WIDTH - 1) * Inventory.tileSizeWidth / 2;


        }
        Vector2Int gridPosition = selectedInventory.GetTileGridPosition(mousePosition);
        return gridPosition;
    }

    /// <summary>
    /// Drags the icon of the selected item to the current mouse position.
    /// </summary>
    private void IconDrag()
    {
        if (selectedItem != null)
        {
            selectedItemTransform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// Creates a random item in the inventory ** TESTING PURPOSES **.
    /// </summary>
    private void CreateRandomItem(){
        NewInventoryItem newItem= Instantiate(itemPrefab).GetComponent<NewInventoryItem>();
        selectedItem = newItem;

        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);

        int selectedUID= Random.Range(0, items.Count);
        newItem.Set(items[selectedUID]);
    }

    
    /// <summary>
    /// @ OVERRIDE/OVERLOAD Inserts a new item into the inventory by instantiating the item prefab and setting the item data.
    /// </summary>
    /// <param name="itemtoInsert">The item to insert.</param>
    /// <param name="staticselectedInventory">The selected item grid.</param>
    public void InsertNewItem(NewItem item, Inventory staticselectedInventory)
        {
        item.construct();    
        NewInventoryItem newItem= item.invModel.GetComponent<NewInventoryItem>();
        selectedItem = newItem;
        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);
        Vector2Int? storePos = staticselectedInventory.FindSpace(newItem);
        if (storePos != null)
        {
            staticselectedInventory.putItemInInventory(newItem, storePos.Value.x, storePos.Value.y);
            Debug.Log("Called");
            selectedItem=null;
        }
        else
        {
           return;
        }
        // NewInventoryItem newItem= Instantiate(itemPrefab).GetComponent<NewInventoryItem>();
        // selectedItem = newItem;
        // selectedItemTransform = newItem.GetComponent<RectTransform>();
        // selectedItemTransform.SetParent(canvasTransform);

        // NewItem item= new();
        // item.construct(itemtoInsert.width, itemtoInsert.height, itemtoInsert.itemIcon, itemtoInsert.thisItemFab);
        // newItem.Set(item);
        // newItem = selectedItem;
        // selectedItem=null;
        // Vector2Int? storePos = staticselectedInventory.FindSpace(newItem);
        // if (storePos != null)
        // {
        //     staticselectedInventory.putItemInInventory(newItem, storePos.Value.x, storePos.Value.y);
        // }
        // else
        // {
        //    return;
        // }           
        
        }

}
