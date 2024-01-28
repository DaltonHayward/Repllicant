using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchMenu : MonoBehaviour
{
    //This is on the workbench
    public Canvas craftingMenu;
    public bool isCrafting;

    private void Start()
    {
        craftingMenu.enabled = false;
    }

    public void InteractionEvent()
    {
        if (isCrafting)
        {
            CloseCraftingMenu();
        }
        else
        {
            OpenCraftingMenu();
        }
    }

    public void OpenCraftingMenu()
    {
        // This displays the crafing menu
        craftingMenu.enabled = true;
        isCrafting = true;

        // Time.timeScale = 0f; Optional time stop while crafting
    }

    public void CloseCraftingMenu()
    {
        // Disable crafting menu
        craftingMenu.enabled = false;
        isCrafting = false;

        // Time.timescale = 1f; Resume regular time if stopped when crafting

    }
}
