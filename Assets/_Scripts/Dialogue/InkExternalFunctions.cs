using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.Runtime.CompilerServices;


public class InkExternalFunctions
{
    //private InventoryInteraction inventoryInteraction;
    public void Bind(Story story)
    {
        story.BindExternalFunction("craftingMenu", () => CraftingManager.instance.inventoryInteraction.OpenCrafting());
        //BindExternalFunction(string funcName, Func<object> func, bool lookaheadSafe=false)
    }


    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("craftingMenu");
    }

    

}