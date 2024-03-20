using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;


public class InventoryController : MonoBehaviour,IDataPersistance
{
    public static InventoryController instance;

    [System.Serializable]
    public class ItemDataEntry
    {
        public string Name;
        public ItemData itemData;
    }

    [HideInInspector]
    private ItemGrid selectedItemGrid;
    [HideInInspector]
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid; set
        {
            selectedItemGrid = value;
            InventoryHighlight.setParent(selectedItemGrid);
        }
    }

    public GameObject Player;
    Inventory_Item selectedItem;

    Inventory_Item overLappingItem;
    RectTransform selectedItemTransform;

    // for random spawning
    [SerializeField] List<ItemData> items;

    [SerializeField] public GameObject ItemPrefab;
    [SerializeField] Transform canvasTransform;
    InventoryHighlight InventoryHighlight;
    Inventory_Item itemToHighlight;
    Vector2 oldPosition;

    public GameObject staticPlayerInventory;
    public static ItemGrid playerInventory;

    // look up table
    [SerializeField] public List<ItemDataEntry> itemDataEntries;
    public Dictionary<string, ItemData> itemDataDictionary;

    [SerializeField]
    private float _timeBetweenEffectApplication = 3f;

    [HideInInspector]
    public bool DropdownHovered = false;

    private PlayerController playerController;

    Inventory_Item equippedWeapon;
    Inventory_Item equippedPickaxe;
    Inventory_Item equippedAxe;


    /// <summary>
    /// Called when the script instance is being loaded. Responsible for doing singleton logic.
    /// sets inventory highlight and player inventory.
    /// </summary>
    private void Awake()
    {
        // singleton
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
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        itemDataDictionary = new Dictionary<string, ItemData>();
       

        foreach (ItemDataEntry entry in itemDataEntries)
        {
            itemDataDictionary.Add(entry.Name, entry.itemData);
        }

        StartCoroutine(ApplyEffectsLoop());
    }

    /// <summary>
    /// This method is called once per frame and is responsible for updating the inventory controller.
    /// It handles icon dragging, creating random items, highlighting the selected item, picking up and moving items,
    /// inserting items, and rotating items.
    /// </summary>
    private void Update()
    {
        IconDrag();

        if (selectedItemGrid == null)
        {
            InventoryHighlight.Display(false);
            // Clicked while droping
            if (Input.GetMouseButtonDown(0) && selectedItem != null)
            {
                DropHeldItem(selectedItem);
            }
            // Click while context menu is open
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && DropdownController.instance.DropdownEnabled() && !DropdownHovered)
            {
                HideContextMenu();
            }
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            // hide context menu if it is enabled and not hovered
            if (DropdownController.instance.DropdownEnabled() && !DropdownHovered)
            {
                HideContextMenu();
            }

            PickUpandMove();
        }

        // show context menu, hide it if it is open
        if (Input.GetMouseButtonDown(1))
        {
            Vector2Int mouseGridPos = selectedItemGrid.GetTileGridPosition(Input.mousePosition);
            Inventory_Item item = selectedItemGrid.GetItem(mouseGridPos.x, mouseGridPos.y);
           
            if (item != null )
            {
                SetClickedItem(item);
                ShowContextMenu();
            }
            else if (item == null && DropdownController.instance.DropdownEnabled())
            {
                HideContextMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
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
            //BroadcastEffects();
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
        if (!Input.GetKeyDown(KeyCode.R) && gridPosition == oldPosition) { return; }

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
                selectedItemTransform.SetAsLastSibling();
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
                    overLappingItem = null;
                    selectedItemTransform = selectedItem.GetComponent<RectTransform>();
                    selectedItemTransform.SetAsLastSibling();
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
            mousePosition.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
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

        Inventory_Item newItem = Instantiate(ItemPrefab).GetComponent<Inventory_Item>();

        selectedItem = newItem;

        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);
        selectedItemTransform.SetAsLastSibling();

        int selectedUID = Random.Range(0, items.Count);
        newItem.Set(items[selectedUID]);
    }


    /// <summary>
    /// @ OVERRIDE/OVERLOAD Inserts a new item into the inventory by instantiating the item prefab and setting the item data.
    /// </summary>
    /// <param name="newItem">The item to insert.</param>
    /// <param name="staticselectedItemGrid">The selected item grid.</param>
    public void InsertNewItem(ItemData itemToInsert, ItemGrid staticselectedItemGrid)
    {
        
        Inventory_Item newItem = Instantiate(ItemPrefab).GetComponent<Inventory_Item>();

        selectedItem = newItem;
        selectedItemTransform = newItem.GetComponent<RectTransform>();
        selectedItemTransform.SetParent(canvasTransform);

        newItem.Set(LookUpItem(itemToInsert.Name));
/*        Vector3 overlayPos = new Vector3((itemToInsert.width * ItemGrid.tileSizeWidth) - ItemGrid.tileSizeWidth, (itemToInsert.height * ItemGrid.tileSizeHeight / 2) - ItemGrid.tileSizeHeight / 2, 0);
        Debug.Log(newItem.GetComponentInChildren<RectTransform>().transform.position);
        newItem.GetComponentInChildren<RectTransform>().transform.position = overlayPos;*/
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

    public ItemData LookUpItem(string name)
    {

        return itemDataDictionary[name];
    }

    public void DropHeldItem(Inventory_Item item)
    {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Instantiate(instance.LookUpItem(item.itemData.Name).envModel, new Vector3(playerPos.x + Random.Range(-1f, 1f), 0.8f, playerPos.z + Random.Range(-1f, 1f)), Quaternion.identity);
        if (item.isEquipped) { item.Unequip(); } 
        Destroy(item.gameObject);
    }

    /// <summary>
    /// Loops through the inventory and applies each items effects to other items in its range
    /// </summary>
    private void BroadcastEffects()
    {
        for (int child = 1; child < playerInventory.transform.childCount; child++)
        {

            Inventory_Item broadcastingItem = playerInventory.transform.GetChild(child).GetComponent<Inventory_Item>();

            if (broadcastingItem.itemData.effects.Length == 0) { continue; }

            List<Vector2Int> gridPositions = playerInventory.CalculateGridPositions(broadcastingItem);
            List<Inventory_Item> receivingItems = new List<Inventory_Item>();

            int radius = broadcastingItem.itemData.range;
            int xCoord = broadcastingItem.OnGridPositionX;
            int yCoord = broadcastingItem.OnGridPositionY;

            for (int i = xCoord - radius; i < xCoord + broadcastingItem.WIDTH + radius; i++)
            {
                for (int j = yCoord - radius; j < yCoord + broadcastingItem.HEIGHT + radius; j++)
                {
                    // dont check area outside of grid
                    if (i < 0 || j < 0 || i > playerInventory.InventoryWidth-1 || j > playerInventory.InventoryHeight-1) { continue; }
                    // dont check position of the current item
                    if (gridPositions.Contains(new Vector2Int(i, j))) { continue; }

                   
                    Inventory_Item recievingItem = playerInventory.GetItem(i, j);
                    if (recievingItem != null && !receivingItems.Contains(recievingItem))
                    {
                        receivingItems.Add(recievingItem);
                    }
                        
                }
            }

            foreach (Inventory_Item receivingItem in receivingItems)
            {
                if (receivingItem != null && receivingItem != broadcastingItem)
                {
                    ISubscriber Isub = receivingItem.GetComponent<ISubscriber>();
                    if (Isub != null)
                    {
                        foreach (string effect in broadcastingItem.itemData.effects)
                        {
                            Isub.ReceiveMessage(effect);
                        }
                    }
                }
            } 
        }
    }

    IEnumerator ApplyEffectsLoop()
    {
        while (true)
        {
            BroadcastEffects();
            yield return new WaitForSeconds(_timeBetweenEffectApplication);
        }
    }

    #region - Dropdown -
    public void PlayerDeath()
    {
        staticPlayerInventory.GetComponent<ItemGrid>().DeathDrop();
    }

    public void ShowContextMenu()
    {
        DropdownController.instance.Show();
    }

    public void HideContextMenu()
    {
        DropdownController.instance.Hide();
    }

    public void SetClickedItem(Inventory_Item item)
    {
        DropdownController.instance.SetClickedItem(item);
    }

    public Inventory_Item GetClickedOnItem()
    {
        return DropdownController.instance.GetClickedOnItem();
    }

    public void DropItem()
    {
        Inventory_Item item = DropdownController.instance.GetClickedOnItem();
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Instantiate(instance.LookUpItem(item.itemData.Name).envModel, new Vector3(playerPos.x + Random.Range(-1f, 1f), 0.8f, playerPos.z + Random.Range(-1f, 1f)), Quaternion.identity);
        // clean up 
        SetClickedItem(null);
        HideContextMenu();
        if (item.isEquipped) { item.Unequip(); }
        playerInventory.RemoveItem(item);
        Destroy(item.gameObject);
    }

    public void EquipTool(Inventory_Item item)
    {
        switch (item.itemData.toolType)
        {
            case PlayerController.Equipment.WEAPON:
                //remove outline from currently equipped item
                if (equippedWeapon != null)
                    equippedWeapon.Unequip();
                equippedWeapon = item;
                equippedWeapon.Equip();
                break;

            case PlayerController.Equipment.PICKAXE:
                if (equippedPickaxe != null)
                    equippedPickaxe.Unequip();
                equippedPickaxe = item;
                equippedPickaxe.Equip();
                break;

            case PlayerController.Equipment.AXE: 
                if (equippedAxe != null)
                    equippedAxe.Unequip();
                equippedAxe = item;
                equippedAxe.Equip();
                break;
        }
        HideContextMenu();
        playerController.EquipTool(item.itemData.toolType, item.itemData.envModel);
    }

    public void UnequipTool(Inventory_Item item)
    {
        switch (item.itemData.toolType)
        {
            case PlayerController.Equipment.WEAPON:
                //remove outline from currently equipped item
                if (equippedWeapon != null)
                    equippedWeapon.Unequip();
                equippedWeapon = null;
                break;

            case PlayerController.Equipment.PICKAXE:
                if (equippedPickaxe != null)
                    equippedPickaxe.Unequip();
                equippedPickaxe = null;
                break;

            case PlayerController.Equipment.AXE:
                if (equippedAxe != null)
                    equippedAxe.Unequip();
                equippedAxe = null;
                break;
        }

        HideContextMenu();
    }
    public void LoadData(GameData gameData)
    {
        List<(string,int,int)> newlist= new List<(string,int,int)>();
        newlist= gameData.InvItems;
        Debug.Log("Loading Inventory, count is: " + newlist.Count);
        foreach ((string, int, int) item in newlist)
        {
            ItemData itemData = itemDataDictionary[item.Item1];
            playerInventory.LoadnewItem(itemData, item.Item2, item.Item3);
        }
    }

    public void SaveData(ref GameData gameData)
    {
        Debug.Log("Saving Inventory");
        gameData.InvItems = playerInventory.getAllItems();    
    }
    #endregion

  


}
