using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.UI;

public class InventoryTempFIx : MonoBehaviour{

    public Canvas Canvas; 

    /// <summary>
    /// This is just to fix a temp bug with the inventory system, causing
    /// items to not appear in the inventory.
    /// </summary>
    void Start()
    {
        Canvas.enabled= true; 
        Canvas.enabled= false; 

    }

    public void Open(){
        Canvas.enabled= true; 

    }
    public void Close(){
        Canvas.enabled= false; 

    }

}
