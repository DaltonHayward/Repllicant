using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemGrid lootGrid;
    public InventoryController inventoryController;
    public Canvas LootUI;
    private bool isChestOpen = false;
    public int minItems = 1;
    public int maxItems = 5;
    void Start()
    {
        ItemGrid lootgrid = GetComponentInChildren<ItemGrid>(); 
        InventoryController.instance.RollLoot(lootgrid, minItems, maxItems);
    }    
    public void OpenTheStash()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player._playerState == PlayerController.State.MOVING || player._playerState == PlayerController.State.STANDING || player._playerState == PlayerController.State.INTERACTING)
        {
            LootUI.enabled = !isChestOpen;
            InventoryController.instance.playerCanvas.enabled = !isChestOpen;
            isChestOpen = !isChestOpen;
            if (isChestOpen)
            {
                player.SetState(PlayerController.State.INTERACTING);
            }
            else
            {
                player.SetState(PlayerController.State.STANDING);
            }
        }
    }

    
}
