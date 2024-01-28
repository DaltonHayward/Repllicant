using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchMenu : MonoBehaviour
{
    public GameObject craftingMenu;// Reference to crafting menu canvas
    public bool isCrafting;

    private void Start()
    {
        craftingMenu.SetActive(false);
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
        craftingMenu.SetActive(true);
        isCrafting = true;

        // Time.timeScale = 0f; Optional time stop while crafting
    }

    public void CloseCraftingMenu()
    {
        // Disable pause menu
        craftingMenu.SetActive(false);
        isCrafting = false;

        // Time.timescale = 1f; Resume regular time if stopped when crafting

    }
}
