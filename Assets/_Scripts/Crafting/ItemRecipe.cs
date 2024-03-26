using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class ItemRecipe : MonoBehaviour
{
    public BaseItemRecipe itemRecipe;


    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] GameObject ingredientNamePrefab;
    [SerializeField] GameObject ingredientSpacingPrefab;
    [SerializeField] GameObject ingredientHavePrefab;
    [SerializeField] GameObject ingredientNeedPrefab;
    [SerializeField] GameObject recipePrefab;
    //[SerializeField] GameObject craftableSpritePrefab;

    
    
    
    public void OnPointerClick()
    {
        CraftingManager.instance.UpdateCraftingUI();
        CraftingManager.instance.recipePanel.SetActive(true);
        CraftingManager.instance.selectedRecipe = itemRecipe;
        GetComponent<UnityEngine.UI.Image>().canvasRenderer.SetAlpha(100);
        
        if (CraftingManager.instance.inventoryInteraction.ItemCheck(itemRecipe.input))
        {
            CraftingManager.instance.craftingButton.interactable = true;
        }
        else
        {
            CraftingManager.instance.craftingButton.interactable = false;
        }
        UpdateRecipeUI(CraftingManager.instance.selectedRecipe);
    }

    public void UpdateRecipeUI(BaseItemRecipe newItemRecipe)
    {
        itemRecipe = newItemRecipe;

        Transform recipeParent = CraftingManager.instance.recipeParent;
        

        foreach (Transform child in recipeParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < itemRecipe.input.Length; i++)
        {   
            // instantiate vertical layout group
            GameObject newRecipe = Instantiate(recipePrefab, recipeParent);
            // instantiate ingredient sprite
            GameObject newIngredient = Instantiate(ingredientPrefab, recipeParent);
            ItemData item = InventoryController.instance.itemDataDictionary[itemRecipe.input[i].name];
            newIngredient.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = item.sprites[0].sprite;  
            newIngredient.transform.SetParent(newRecipe.transform);
            // instantiate ingredient name
            GameObject newIngredientName = Instantiate(ingredientNamePrefab, recipeParent);
            newIngredientName.transform.GetComponent<TextMeshProUGUI>().text = itemRecipe.input[i].name;
            newIngredientName.transform.SetParent(newRecipe.transform);
            // instantiate spacing between name and have/needs
            GameObject newSpacing =Instantiate(ingredientSpacingPrefab,transform);
            newSpacing.transform.SetParent(newRecipe.transform);
            // instantiate number of ingredients that we have
            GameObject newIngredientHave = Instantiate(ingredientHavePrefab,recipeParent);
            newIngredientHave.transform.GetComponent<TextMeshProUGUI>().text = CraftingManager.instance.inventoryInteraction.ItemCountCheck(itemRecipe.input[i].name).ToString(); 
            // set number panel to red if we don't have enough items
            /*if (CraftingManager.instance.inventoryInteraction.ItemCountCheck(itemRecipe.input[i].name) < itemRecipe.input[i].count)
            {
                newIngredientHave.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(251,0,0,50);
            }*/
            newIngredientHave.transform.SetParent(newRecipe.transform);
            // instantiate number of ingredients that we need
            GameObject newIngredientNeed = Instantiate(ingredientNeedPrefab, recipeParent);
            newIngredientNeed.transform.GetComponent<TextMeshProUGUI>().text = " / " + itemRecipe.input[i].count.ToString();
            newIngredientNeed.transform.SetParent(newRecipe.transform);
        }
        

    }
}
