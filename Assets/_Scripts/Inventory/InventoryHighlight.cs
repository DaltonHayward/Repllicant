using System;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Display(bool b){

        highlighter.gameObject.SetActive(b);
    }

    /// <summary>
    /// Highlights the specified inventory item. Bases what is highlighted on the item's width and height.
    /// </summary>
    /// <param name="item">The inventory item to highlight.</param>
    public void Highlight(Inventory_Item item)
    {
        Vector2 size = new Vector2(item.WIDTH * ItemGrid.tileSizeWidth, item.HEIGHT * ItemGrid.tileSizeHeight);
        highlighter.sizeDelta = size;
    }
    /// <summary>
    /// Sets the position of the highlighter to the specified item.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="item"></param>
    public void SetPosition(ItemGrid target, Inventory_Item item)
    {
        setParent(target);

        Vector2 newPosition = target.CalculateItemPosition(item, item.OnGridPositionX, item.OnGridPositionY);

        highlighter.localPosition = newPosition;
    }

    /// <summary>
    /// Sets the position of the highlighter to the specified item after calculating the items position.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="item"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPosition(ItemGrid target, Inventory_Item item, int x, int y)
    {
        setParent(target);

        Vector2 newPosition = target.CalculateItemPosition(item, x, y);
        highlighter.localPosition = newPosition;
    }
    /// <summary>
    /// Sets the parent of the highlighter to the itemGrid that gets passed in.
    /// </summary>
    /// <param name="target"></param>
    public void setParent(ItemGrid target)
    {
        if(target == null){return;}
        highlighter.SetParent(target.GetComponent<RectTransform>());
    }

}
