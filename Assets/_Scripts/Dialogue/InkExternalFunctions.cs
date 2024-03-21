using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.Runtime.CompilerServices;


public class InkExternalFunctions
{
    private InventoryInteraction inventoryInteraction;
    public void Bind(Story story, InventoryInteraction inventoryInteraction)
    {
        story.BindExternalFunction("craftingMenu", () =>
        {
            if (inventoryInteraction != null)
            {
                inventoryInteraction.OpenCrafting();
            }
            else
            {
                Debug.LogWarning("Tried to open crafting menu, but InventoryInteraction was " + "not initialized when entering dialogue mode.");
            }
        });
    }


    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("craftingMenu");
    }

    

}