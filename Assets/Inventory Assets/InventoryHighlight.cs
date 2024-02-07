using System;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Display(bool b){

        highlighter.gameObject.SetActive(b);
    }

    public void Highlight(Inventory_Item item)
    {
        Vector2 size = new Vector2(item.WIDTH * ItemGrid.tileSizeWidth, item.HEIGHT * ItemGrid.tileSizeHeight);
        highlighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid target, Inventory_Item item)
    {
        setParent(target);

        Vector2 newPosition = target.CalculateItemPosition(item, item.OnGridPositionX, item.OnGridPositionY);

        highlighter.localPosition = newPosition;
    }

    //Oerloaded method to set the position of the highlighter
    public void SetPosition(ItemGrid target, Inventory_Item item, int x, int y)
    {
        setParent(target);

        Vector2 newPosition = target.CalculateItemPosition(item, x, y);
        highlighter.localPosition = newPosition;
    }
    public void setParent(ItemGrid target)
    {
        if(target == null)
        {            return;
        }
        highlighter.SetParent(target.GetComponent<RectTransform>());
  
    }

}
