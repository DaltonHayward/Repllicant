using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;

using UnityEngine;

using UnityEngine.UI;



public class CraftingManager : MonoBehaviour
{


    [HideInInspector] public bool isCraftingOpened;

    [Header("Crafting UI")]
    [SerializeField] public GameObject craftingCanvas;
    [SerializeField] UnityEngine.UI.Button craftingButton;

    [Header("Recipes")]
    [SerializeField] BaseItemRecipe[] recipes;
    [SerializeField] GameObject recipePrefab;
    //[SerializeField] Transform recipeParent;

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
        inventoryInteraction = GetComponent<InventoryInteraction>();
    }


    public void EnterCraftingMode()
    {
        Debug.Log("EnterCraftingMode!");
        craftingCanvas.SetActive(true);
        isCraftingOpened = true;
    }

    public void ExitCraftingMode()
    {
        //craftingCanvas.SetActive(false);
        isCraftingOpened = false;
    }




    /* private void UpdateCraftingUI()
     {
         foreach (Transform child in recipeParent)
         {
             Destroy(child.gameObject);
         }

         for (int i = 0; i < recipes.Length; i++)
         {
             GameObject newRecipe = Instantiate(recipePrefab, recipeParent);
         }
     }*/

    







}
