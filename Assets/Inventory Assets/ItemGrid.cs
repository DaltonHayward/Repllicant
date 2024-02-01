using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemGrid : MonoBehaviour
{
    const float tileSizeWidth = 32;
    const float tileSizeHeight = 32;
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
        
        // rectTransform.sizeDelta = new Vector2(32, 32); How to change size for possible Upgrades 
        Inventory_Item newitem = Instantiate(itemPrefab).GetComponent<Inventory_Item>();
        storeItem(newitem, 1,1);
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

    public void storeItem(Inventory_Item item, int x, int y)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        invItemSlots[x, y] = item;

        Vector2 position= new Vector2();
        position.x = x * tileSizeWidth ;
        Debug.Log(position.x);
        position.y = -(y * tileSizeHeight );
        Debug.Log(position.y);

        rectTransform.localPosition = position;
        Debug.Log(rectTransform.localPosition); 
        
    }
    
    
}
