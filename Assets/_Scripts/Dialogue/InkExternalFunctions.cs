using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    private CraftingManager _CraftingManager;
    private DialogueManager _DialogueManager;
    public void Bind(Story story, CraftingManager craftingManager)
    {
        story.BindExternalFunction("craftingMenu", () => _CraftingManager.EnterCraftingMode());
    }

    public void Unbind(Story story) 
    {
        story.UnbindExternalFunction("craftingMenu");
    }

    public void craftingMenu()
    {
        if (_CraftingManager != null) 
        {
            _CraftingManager.EnterCraftingMode();
            _DialogueManager.ExitDialogueMode();
        }
        else 
        {
            Debug.LogWarning("Tried to open crafting menu, but CraftingManager was "
                + "not initialized when entering dialogue mode.");
        }
    }
    
}