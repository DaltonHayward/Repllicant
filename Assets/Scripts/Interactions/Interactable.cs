using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Makes an object interactable for the player
// Requires InteractionNotification component on player and InteractionNotif prefab as player child
public class Interactable : MonoBehaviour
{
    //This is on workbench
    private bool _isInRange;
    // public KeyCode interactKey;
    public UnityEvent interactAction;

    // Update is called once per frame
    void Update()
    {
        if (_isInRange)
        {
            if (InputManager.instance.GetKeyDown("interact"))
            {
                interactAction.Invoke();
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInRange = true;
            other.gameObject.GetComponent<InteractionNotification>().NotifyPlayer();
            Debug.Log("Player now in range");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInRange = false;
            other.gameObject.GetComponent<InteractionNotification>().DenotifyPlayer();
            Debug.Log("Player no longer range");
        }
    }
}
