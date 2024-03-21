using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private UnityEngine.TextAsset inkJSON;

    [Header("Crafting Manager")]
    [SerializeField] private CraftingManager craftingManager;

    private bool playerInRange;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }
    private void Update() 
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if(InputManager.instance.InteractInput)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, craftingManager);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
