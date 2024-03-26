using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.Runtime.CompilerServices;


public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("OpenCrafting", () => OpenCrafting());
        //BindExternalFunction(string funcName, Func<object> func, bool lookaheadSafe=false)
        // story.BindExternalFunction("OpenCrafting", () => CraftingManager.instance.inventoryInteraction.OpenCrafting());
    }


    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("OpenCrafting");
    }

    public void OpenCrafting()
    {
        CraftingManager.instance.inventoryInteraction.OpenCrafting();
    }

    

}