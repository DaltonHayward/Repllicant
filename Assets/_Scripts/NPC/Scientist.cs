using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;
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
        if (!interacted)
        {
            progressManager.scientist = true;
            player.GetComponent<PlayerHealth>().Heal(player.GetComponent<PlayerHealth>().maxHealth);
        }
    }
}
