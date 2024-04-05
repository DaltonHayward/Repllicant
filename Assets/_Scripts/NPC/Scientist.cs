using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;
    [SerializeField] BaseItemRecipe recipe;
    private GameObject player;
    // keep track of if player has interacted with this npc in this scene instance
    public bool interacted = false;

    public void Start()
    {
        // Find the player GameObject by its tag
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject not found! Make sure you have tagged your player GameObject correctly.");
        }
    }

    public void SendToBase()
    {
        if (progressManager != null) 
        { 
            progressManager.scientist = true;
            if ((progressManager.aviator == true) && (progressManager.tech == true) && (progressManager.scientist == true) && (progressManager.tProgress < 11))
            {
                progressManager.UpdateTutorialProgress(11);
            }
        }
        HealPlayer();
        // check player inventory for reagents
        InventoryInteraction inventory = CraftingManager.instance.GetComponent<InventoryInteraction>();

        var item1 = new ItemTypeAndCount("IonizedEtherCrystal", 1);
        Debug.Log("test");

        ItemTypeAndCount[] items = { item1 };
        Debug.Log(inventory.ItemCheck(items));
        if (inventory.ItemCheck(items))
        {
            inventory.CraftItem(recipe);
        }

    }

    public void HealPlayer()
    {
        if (!interacted)
        {
            player.GetComponent<PlayerHealth>().Heal(player.GetComponent<PlayerHealth>().maxHealth);
        }
    }
}
