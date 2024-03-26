using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;





public class CraftingManager : MonoBehaviour
{


    [HideInInspector] public bool isCraftingOpened;

    [Header("Crafting UI")]
    [SerializeField] public GameObject craftingCanvas;
    [SerializeField] public UnityEngine.UI.Button craftingButton;

    [Header("Recipes")]
    [SerializeField] BaseItemRecipe[] recipes;
    [SerializeField] GameObject craftableItem;
    [SerializeField] Transform contentParent;

    [SerializeField] public Transform recipeParent;

    [SerializeField] public GameObject recipePanel;


    public InventoryInteraction inventoryInteraction;

    private List<ItemTypeAndCount> items = new List<ItemTypeAndCount>();

    public static CraftingManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        craftingCanvas.SetActive(false);
        recipePanel.SetActive(false);
        inventoryInteraction = GetComponent<InventoryInteraction>();
    }

    private void OnEnable()
    {
        UpdateCraftingUI();
    }


    public void EnterCraftingMode()
    {
        Debug.Log("EnterCraftingMode!");
        craftingCanvas.SetActive(true);
        isCraftingOpened = true;
    }

    public void ExitCraftingMode()
    {
        craftingCanvas.SetActive(false);
        isCraftingOpened = false;
    }




    private void UpdateCraftingUI()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < recipes.Length; i++)
        {
            GameObject newCraftable = Instantiate(craftableItem, contentParent);
            newCraftable.name = recipes[i].name;
            newCraftable.GetComponent<ItemRecipe>().itemRecipe = recipes[i];
            newCraftable.GetComponent<UnityEngine.UI.Image>().sprite = recipes[i].output.sprites[0].sprite;
            newCraftable.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = false;
        }
    }









}
