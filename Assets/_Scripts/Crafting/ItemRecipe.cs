using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class ItemRecipe : MonoBehaviour
{
    public BaseItemRecipe itemRecipe;

    InventoryInteraction _InventoryInteraction;

    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] GameObject ingredientNamePrefab;
    [SerializeField] GameObject ingredientSpacingPrefab;
    [SerializeField] GameObject ingredientHavePrefab;
    [SerializeField] GameObject ingredientNeedPrefab;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] GameObject craftableSpritePrefab;
    
    public void OnPointerClick()
    {
        UpdateRecipeUI(itemRecipe);
    }
    public void UpdateRecipeUI(BaseItemRecipe newItemRecipe)
    {
        itemRecipe = newItemRecipe;
        

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < itemRecipe.input.Length; i++)
        {
            GameObject newIngredient = Instantiate(ingredientPrefab, transform);
            //newIngredient.transform.GetChild(0).GetComponent<Image>().sprite = itemRecipe.input[i]..sprites[0].sprite;
            GameObject newIngredientName = Instantiate(ingredientNamePrefab, transform);
            newIngredientName.transform.GetComponent<TextMeshProUGUI>().text = itemRecipe.input[i].name;
            Instantiate(ingredientSpacingPrefab,transform);
            GameObject newIngredientHave = Instantiate(ingredientHavePrefab,transform);
            newIngredientHave.transform.GetComponent<TextMeshProUGUI>().text = _InventoryInteraction.ItemCountCheck(itemRecipe.input[i].name).ToString(); // update this line of code upon adding craftable check
            GameObject newIngredientNeed = Instantiate(ingredientNeedPrefab, transform);
            newIngredientNeed.transform.GetComponent<TextMeshProUGUI>().text = " / " + itemRecipe.input[i].count.ToString();
        }
        

    }
}
