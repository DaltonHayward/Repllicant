using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;
    Vector2 poisitionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    RectTransform rectTransform;
    Inventory_Item[,] invItemSlots;
    [SerializeField] public int InventoryWidth;
    [SerializeField] public int InventoryHeight;
    [SerializeField] GameObject itemPrefab;


    private void Start(){
        rectTransform = GetComponent<RectTransform>();
        Init(InventoryWidth,InventoryHeight);
        
       
    }

    private void Init(int width, int height)
    {
        invItemSlots = new Inventory_Item[width, height];
        //creates the size for objects?
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {

        poisitionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        poisitionOnTheGrid.y = rectTransform.position.y - mousePosition.y;
        tileGridPosition.x = (int)(poisitionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(poisitionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
        
    }

    /*
    Handles the storing of the item in the grid, checks if the item can be stored in the grid, if it can it will store the item in the grid and return true, 
    if it can't it will return false
    */
    public bool storeItem(Inventory_Item item, int x, int y, ref Inventory_Item overLappingItem)
    {
        
        if (BoundryCheck(x, y, item.itemData.width, item.itemData.height) == false){return false;}
        if (OverlapCheck(x, y, item.itemData.width, item.itemData.height, ref overLappingItem) == false){
            overLappingItem = null;
            return false;}
        if (overLappingItem != null){
            CleanUpTiles(overLappingItem);
        }
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
       

        for ( int itemx =0 ; itemx < item.itemData.width; itemx++)
        {
            for (int itemy = 0; itemy < item.itemData.height; itemy++)
            {
               
                invItemSlots[x + itemx, y + itemy] = item;
                
            }
        }

        if (x + item.itemData.width > InventoryWidth || y + item.itemData.height > InventoryHeight){
            Debug.Log("Item is too big for the grid");
            Destroy(item.gameObject);
        }
        else
        {
            invItemSlots[x, y] = item;


            item.OnGridPositionX = x;
            item.OnGridPositionY = y;
            Vector2 position = CalculateItemPosition(item,x, y);
            rectTransform.localPosition = position;
            return true;

        }
        return false;
        
    }

    public Vector2 CalculateItemPosition(Inventory_Item item, int x, int y)
    {
        Vector2 position = new Vector2();
        position.x = x * tileSizeWidth;
        position.y = -(y * tileSizeHeight);
        return position;
    }

    /*
    Checks if the item is overlapping with another item in the grid, if it is it will return false and set the overLappingItem to the item that is overlapping
    */
    private bool OverlapCheck(int x, int y, int width, int height, ref Inventory_Item overLappingItem)
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
    /*
    if the item is not null, it will remove the item from the grid and return the item
    */
    public Inventory_Item PickUpItem(int x, int y)
    {
        Inventory_Item item = invItemSlots[x, y];

        if (item == null) { return null; }

        CleanUpTiles(item);

        invItemSlots[x, y] = null;
        return item;

    }

    private void CleanUpTiles(Inventory_Item item)
    {
        for (int itemx = 0; itemx < item.itemData.width; itemx++)
        {
            for (int itemy = 0; itemy < item.itemData.height; itemy++)
            {
                invItemSlots[item.OnGridPositionX + itemx, item.OnGridPositionY + itemy] = null;
            }
        }
    }

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

    bool BoundryCheck(int origix, int origiy, int width, int height)
    {
        if (CanStoreItem(origix, origiy) == false || CanStoreItem(origix + width-1, origiy + height-1) == false)
        {
            return false;
        }
       
        
        return true;
    }

    internal Inventory_Item GetItem(int x, int y)
    {
        return invItemSlots[x, y];
    }
}
