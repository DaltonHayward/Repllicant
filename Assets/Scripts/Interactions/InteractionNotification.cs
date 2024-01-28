using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNotification : MonoBehaviour
{
    // This is on the player
    private GameObject interactable;
    private Material originalMaterial;

    public Canvas craftingMenu;
    public Material highlightMaterial;

    private void OnTriggerEnter(Collider other)
    {
        // Workbenches and other utilities must be tagged "Interactable"
        if (other.gameObject.CompareTag("Interactable"))
        {
            interactable = other.gameObject;
            Renderer renderer = interactable.GetComponent<Renderer>();

            // Save the original material only if it hasn't been saved yet
            if (originalMaterial == null)
            {
                originalMaterial = renderer.material;
            }

            // Notify player immediately upon interaction to set colour the first time
            NotifyPlayer();
        }
    }

    public void NotifyPlayer()
    {
        Renderer renderer = interactable.GetComponent<Renderer>();

        // Make sure the originalMaterial is not null before assigning highlightMaterial
        if (originalMaterial != null)
        {
            renderer.material = highlightMaterial;
        }
        else
        {
            Debug.LogError("Original material is null. Check the OnTriggerEnter method.");
        }
    }

    public void DenotifyPlayer()
    {
        Renderer renderer = interactable.GetComponent<Renderer>();

        // Restore the original material only if it's not null
        if (originalMaterial != null)
        {
            renderer.material = originalMaterial;

            // Deactivate the crafting menu if the player walks away.
            craftingMenu.enabled = false;

        }
        else
        {
            Debug.LogError("Original material is null. Check the OnTriggerEnter method.");
        }
    }
}