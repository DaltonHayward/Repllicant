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
    [SerializeField] private GameObject craftingCanvas;
    [SerializeField] UnityEngine.UI.Button craftingButton;

    [Header("Recipes")]
    [SerializeField] BaseItemRecipe[] recipes;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] Transform recipeParent;
    
    public InventoryInteraction inventoryInteraction;

    public List<ItemTypeAndCount> items = new List<ItemTypeAndCount>();

    public static CraftingManager instance;


    private void Awake()
    {
         // singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Crafting Manager in the scene");
            Destroy(this);
        }
        
        craftingCanvas.SetActive(false);
        inventoryInteraction = CraftingManager.instance.GetComponent<InventoryInteraction>();
    }

    private void Update() 
    {
    
    }
    public static CraftingManager GetInstance()
    {
        return instance;
    }

    private void OnEnable() 
    {
        UpdateCraftingUI();
        craftingButton.enabled = false;
        
    }

    public void EnterCraftingMode()
    {
        craftingCanvas.SetActive(true);
        isCraftingOpened = true;
    }

    public void ExitCraftingMode()
    {
        //yield return new WaitForSeconds(0.2f);
        craftingCanvas.SetActive(false);
        isCraftingOpened = false;
    }

    

    
    private void UpdateCraftingUI()
    {
        foreach (Transform child in recipeParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < recipes.Length; i++)
        {
            GameObject newRecipe = Instantiate(recipePrefab, recipeParent);
        }
    }

    public void OpenAndCloseCrafting()
    {
        if (isCraftingOpened)
        {
            isCraftingOpened = false;
        }
        else
        {
            isCraftingOpened = true;
        }
    }


  

    


}
