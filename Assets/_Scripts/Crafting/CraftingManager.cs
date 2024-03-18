using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using static InventoryController;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [SerializeField] BaseItemRecipe[] recipes;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] Transform recipeParent;

    private List<InventoryController.ItemDataEntry> items = new List<InventoryController.ItemDataEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        items = InventoryController.instance.itemDataEntries;
    }
        
}
