using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;


public class InventoryController : MonoBehaviour
{
    [System.Serializable]
    public class ItemDataEntry
    {
        public string Name;
        public ItemData itemData;
    }


    [HideInInspector]
    private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid; set
        {
            selectedItemGrid = value;
            InventoryHighlight.setParent(selectedItemGrid);
        }
    }

    public GameObject Player;
    public bool Droppable;
    Inventory_Item selectedItem;

    Inventory_Item overLappingItem;
    RectTransform selectedItemTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    InventoryHighlight InventoryHighlight;
    Inventory_Item itemToHighlight;
    Vector2 oldPosition;
    public GameObject staticPlayerInventory;

    public static ItemGrid playerInventory;
    public static InventoryController instance;

    [SerializeField] public List<ItemDataEntry> itemDataEntries;

    public Dictionary<string, ItemData> itemDataDictionary;

    // [SerializeField] public ItemDataEntry test;


    /// <summary>
    /// Called when the script instance is being loaded. Responsible for doing singleton logic.
    /// sets inventory highlight and player inventory.
    /// </summary>
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        InventoryHighlight = GetComponent<InventoryHighlight>();
        playerInventory = staticPlayerInventory.GetComponent<ItemGrid>();
        itemDataDictionary = new Dictionary<string, ItemData>();

        foreach (ItemDataEntry entry in itemDataEntries)
        {
            itemDataDictionary.Add(entry.Name, entry.itemData);
        }
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

        if (selectedItemGrid == null)
        {
            InventoryHighlight.Display(false);
            return;
        }
        HandleHighlight();

        if (InputManager.instance.AttackInput)
        {
            PickUpandMove();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateRandomItem();
            Inventory_Item itemtoInsert = selectedItem;
            selectedItem = null;
            InsertItem(itemtoInsert);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selectedItem != null)
            {
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
    public void InsertItem(Inventory_Item itemtoInsert)
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

    /// <summary>
    /// Handles the highlighting logic for the inventory controller highlighting.
    /// by grabbing the item, and highlight the grid that the item exists in. 
    /// </summary>
    private void HandleHighlight()
    {
        Vector2Int gridPosition = mouseToGridTranslation();
        if (gridPosition == oldPosition) { return; }

        oldPosition = gridPosition;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(gridPosition.x, gridPosition.y);
            if (itemToHighlight != null)
            {
                InventoryHighlight.Display(true);
                InventoryHighlight.Highlight(itemToHighlight);
                InventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                InventoryHighlight.Display(false);
            }

        }
        else
        {
            InventoryHighlight.Display(selectedItemGrid.BoundryCheck(gridPosition.x, gridPosition.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            InventoryHighlight.Highlight(selectedItem);
            InventoryHighlight.SetPosition(selectedItemGrid, selectedItem, gridPosition.x, gridPosition.y);

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
            selectedItem = selectedItemGrid.PickUpItem(gridPosition.x, gridPosition.y);

            if (selectedItem != null)
            {
                selectedItemTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        else
        {
            if (selectedItemGrid == null)
            {
                Debug.Log("No grid selected");
                DropItem(selectedItem);
                selectedItem = null;
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
            mousePosition.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            mousePosition.y += (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;


        }
        Vector2Int gridPosition = selectedItemGrid.GetTileGridPosition(mousePosition);
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
    private void CreateRandomItem()
    {

        Inventory_Item newItem = Instantiate(itemPrefab).GetComponent<Inventory_Item>();

        selectedItem = newItem;

        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);

        int selectedUID = Random.Range(0, items.Count);
        newItem.Set(items[selectedUID]);
    }


    /// <summary>
    /// @ OVERRIDE/OVERLOAD Inserts a new item into the inventory by instantiating the item prefab and setting the item data.
    /// </summary>
    /// <param name="itemtoInsert">The item to insert.</param>
    /// <param name="staticselectedItemGrid">The selected item grid.</param>
    public void InsertNewItem(Inventory_Item itemtoInsert, ItemGrid staticselectedItemGrid)
    {

        Inventory_Item newItem = Instantiate(itemPrefab).GetComponent<Inventory_Item>();
        selectedItem = newItem;
        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);

        ItemData item = new();
        item.construct(itemtoInsert.width, itemtoInsert.height, itemtoInsert.itemIcon, itemtoInsert.thisItemFab);
        newItem.Set(item);
        newItem = selectedItem;
        selectedItem = null;
        Vector2Int? storePos = staticselectedItemGrid.FindSpace(newItem);
        if (storePos != null)
        {
            staticselectedItemGrid.putItemInInventory(newItem, storePos.Value.x, storePos.Value.y);
        }
        else
        {
            return;
        }
    }

    public Inventory_Item LookUpInv(string name)
    {

        return itemDataDictionary[name].invModel.GetComponent<Inventory_Item>();
    }

    public GameObject LookUpEnv(string name)
    {

        return itemDataDictionary[name].envModel;
    }

    public void DropItem(Inventory_Item item)
    {
        Debug.Log("Dropping item");
        GameObject droppedItem = Instantiate(InventoryController.instance.LookUpEnv(item.itemName));
        droppedItem.transform.position = Player.transform.position;
        Destroy(item.gameObject);
    }

}
