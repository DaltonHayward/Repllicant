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

        story.BindExternalFunction("npcAddItem", (string itemToAdd) => NPCAddItem(itemToAdd));

        story.BindExternalFunction("UpdateTutorialStatus", (int status) => UpdateTutorialStatus(status));
        
    }


    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("OpenCrafting");

        story.UnbindExternalFunction("npcAddItem");
    }

    public void OpenCrafting()
    {
        CraftingManager.instance.inventoryInteraction.OpenCrafting();
    }

    public void NPCAddItem(string itemToAdd)
    {
        CraftingManager.instance.inventoryInteraction.NPCAddItem(itemToAdd);
    }

    public void UpdateTutorialStatus(int status)
    {
        TutorialManager.instance.UpdateTutorialProgress(status);
    }

}