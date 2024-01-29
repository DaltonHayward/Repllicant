using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ItemGrid selectedItemGrid;

    private void Update(){

        if (selectedItemGrid==null){return;}

        if (Input.GetMouseButtonDown(0)){
            Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        }
    }
}
