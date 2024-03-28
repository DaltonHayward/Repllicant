using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON; // move this to dialogue manager


   // [SerializeField] public string nameNPC;
    private bool playerInRange;

    private PlayerController _playerController;

    //private DialogueVariables dialogueVariables;


    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.instance.InteractInput)
            {
                _playerController.SetState(PlayerController.State.DIALOG);
                StartCoroutine(_playerController.SlowDown());
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON); 
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    
}
