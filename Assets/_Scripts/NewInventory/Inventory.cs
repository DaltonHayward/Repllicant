using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;
    Vector2 poisitionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    RectTransform rectTransform;
    public NewInventoryItem[,] invItemSlots;
    [SerializeField] public int InventoryWidth;
    [SerializeField] public int InventoryHeight;
    [SerializeField] GameObject itemPrefab;

    /// <summary>
    /// Grabs the component of the current item, and initializes the inventory grid.
    /// </summary>
    private void Start(){
        rectTransform = GetComponent<RectTransform>();
        Init(InventoryWidth,InventoryHeight);  
    }

    /// <summary>
    /// Initializes the inventory grid, sets the size of the grid and creates the inventory grid.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Init(int width, int height)
    {
        invItemSlots = new NewInventoryItem[width, height];
        //creates the size for objects?
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    /// <summary>
    /// Given the current Vector2 mouse position, it will return the grid position of the mouse.
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns>a Vector2Int that is the tileGridPosition </returns>
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {

        poisitionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        poisitionOnTheGrid.y = rectTransform.position.y - mousePosition.y;
        tileGridPosition.x = (int)(poisitionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(poisitionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
        
    }

    /// <summary>
    /// Stores the item in the inventory grid, if there is an overlapping item it will return false, if there is no overlapping item it will return true.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="overLappingItem"></param>
    /// <returns>true or false</returns>
    public bool storeItem(NewInventoryItem item, int x, int y, ref NewInventoryItem overLappingItem)
    {
      
        if (BoundryCheck(x, y, item.WIDTH, item.HEIGHT) == false) { return false; }
        if (OverlapCheck(x, y, item.WIDTH, item.HEIGHT, ref overLappingItem) == false)
        {
            overLappingItem = null;
            return false;
        }
        if (overLappingItem != null)
        {
            CleanUpTiles(overLappingItem);
        }

        putItemInInventory(item, x, y);
        return true;
    }
    /// <summary>
    /// Just places the item in the inventory grid, used by the storeItem function.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void putItemInInventory(NewInventoryItem item, int x, int y)
    {   
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);


        for (int itemx = 0; itemx < item.WIDTH; itemx++)
        {
            for (int itemy = 0; itemy < item.HEIGHT; itemy++)
            {

                invItemSlots[x + itemx, y + itemy] = item;

            }
        }
        invItemSlots[x, y] = item;
        item.OnGridPositionX = x;
        item.OnGridPositionY = y;
        Vector2 position = CalculateItemPosition(item, x, y);
        rectTransform.localPosition = position;
        Debug.Log("Item placed at " + x + " " + y);
    }

    /// <summary>
    /// Calculates the position of the item in the grid,
    /// CURRENTLY CAUSING ISSUES WITH IMAGE ON ROTATION
    /// ** maybe check the rotation of the item and adjust the position accordingly
    /// </summary>
    /// <param name="item"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>returns a Vector2 type</returns>
    public Vector2 CalculateItemPosition(NewInventoryItem item, int x, int y)
    {
        Vector2 position = new Vector2();
        position.x = x * tileSizeWidth;
        position.y = -(y * tileSizeHeight);
        return position;
    }

    /// <summary>
    /// Checks to see if there is any overlapping items in the grid on placement, 
    /// if there is it will return false, if there isn't it will return true.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="overLappingItem"></param>
    /// <returns>true or false</returns>
    private bool OverlapCheck(int x, int y, int width, int height, ref NewInventoryItem overLappingItem)
    {
        if (BoundryCheck(x, y, width, height) == false) { return false; }
        for (int itemx = 0; itemx < width; itemx++)
        {
            for (int itemy = 0; itemy < height; itemy++)
            {
                if (invItemSlots[x + itemx, y + itemy] != null)
                {
                    if( overLappingItem == null){
                        overLappingItem = invItemSlots[x + itemx, y + itemy];
                    }
                    else{
                        if (overLappingItem != invItemSlots[x + itemx, y + itemy]){return false;}

                        overLappingItem = invItemSlots[x + itemx, y + itemy];
                        return false;}
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Checks if there is free space in the grid to store the item, by 
    /// looping through the inventory grid and checking if there is a free space
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns>true if there is a free space, false if there is not</returns>
    private bool CheckFreeSpace(int x, int y, int width, int height)
        {
            if (BoundryCheck(x, y, width, height) == false) { return false; }
            for (int itemx = 0; itemx < width; itemx++)
            {
                for (int itemy = 0; itemy < height; itemy++)
                {
                    if (invItemSlots[x + itemx, y + itemy] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    
    
    /// <summary>
    /// Just " picks up " the item in the incentory and cleans up the tiles afterwards, used by 
    /// another function.
    /// </summary>
    /// <returns>The picked up item as type Inventory Item.</returns>
    public NewInventoryItem PickUpItem(int x, int y)
    {
        NewInventoryItem item = invItemSlots[x, y];

        if (item == null) { return null; }

        CleanUpTiles(item);

        invItemSlots[x, y] = null;
        return item;
    }

    /// <summary>
    /// Removes the item from the grid, going to be used in the inventory controller to remove the item from the grid.
    /// just goes through the grid and sets the items to null
    /// </summary>
    /// <param name="item"></param>
    private void CleanUpTiles(NewInventoryItem item)
    {
        for (int itemx = 0; itemx < item.WIDTH; itemx++)
        {
            for (int itemy = 0; itemy < item.HEIGHT; itemy++)
            {
                invItemSlots[item.OnGridPositionX + itemx, item.OnGridPositionY + itemy] = null;
            }
        }
    }
    /// <summary>
    /// checks to make sure that the item is within the bounds of the grid, if it is it will return true, if it isn't it will return false
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool CanStoreItem( int x, int y)
    {
        if (x < 0 || y < 0){
            return false;
        }
        if (x > InventoryWidth|| y > InventoryHeight){
            return false;
        }
        return true;

    }

    /// <summary>
    /// Checks if the item can be stored in the grid, if it can it will return true, if it can't it will return false
    /// </summary>
    /// <param name="origix"></param>
    /// <param name="origiy"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool BoundryCheck(int origix, int origiy, int width, int height)
    {
        if (CanStoreItem(origix, origiy) == false || CanStoreItem(origix + width, origiy + height) == false)
        {
            return false;
        }
       
        
        return true;
    }
    /// <summary>
    /// returns the item that is located at x and y coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    internal NewInventoryItem GetItem(int x, int y)
    {
        return invItemSlots[x, y];
    }

    /// <summary>
    /// Finds a space in the inventory grid to store the item, if there is no space it will return null.
    /// </summary>
    /// <param name="itemtoInsert"></param>
    /// <returns></returns>
    public Vector2Int? FindSpace(NewInventoryItem itemtoInsert)
    {
        int height= itemtoInsert.HEIGHT;
        int width = itemtoInsert.WIDTH;
        for (int y =0 ; y < InventoryHeight-height +1; y++)
        {
            for (int x = 0; x < InventoryWidth - width+ 1; x++)
            {
                if(CheckFreeSpace(x, y, width, height)== true){
                    return new Vector2Int(x, y);
                }

            }
        }
        return null;
    }

    /// <summary>
    /// Returns the item that is being hovered over, if there is no item being hovered over it will return null.
    /// </summary>
    /// <param name="mouseposition"></param>
    /// <returns></returns>
    public NewInventoryItem ItemHovered(Vector2 mouseposition){
        Vector2Int gridPosition = GetTileGridPosition(mouseposition);
        NewInventoryItem item = GetItem(gridPosition.x, gridPosition.y);
        if (item != null){
            return item;
        }
        return null;
    }

    /// <summary>
    /// NOT IMPLEMENTED YET
    /// Removes the item from the grid, going to be used in the inventory controller to remove the item from the grid.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(NewInventoryItem item)
    {
        CleanUpTiles(item);
    }

    public void AddCollectable(CollectableItem item){
        item.CollectItem();
        

    }
    public void RemoveCollectable(CollectableItem item){

    }
}