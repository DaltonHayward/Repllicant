using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStash : MonoBehaviour
{
    public Canvas stashUI;
    public Canvas PlayerUI;
    private bool isStashOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*// Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (InputManager.instance.InteractInput){
                PlayerController player = other.GetComponent<PlayerController>();
                if (player._playerState == PlayerController.State.MOVING || player._playerState == PlayerController.State.STANDING || player._playerState == PlayerController.State.INTERACTING)
                {
                stashUI.enabled=!isStashOpen;
                PlayerUI.enabled=!isStashOpen;
                isStashOpen = !isStashOpen;
                if(isStashOpen){
                    player.SetState(PlayerController.State.INTERACTING);
                }
                else{
                    player.SetState(PlayerController.State.STANDING);
                }
                }
            }
        }
    }*/

    public void OpenTheStash()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player._playerState == PlayerController.State.MOVING || player._playerState == PlayerController.State.STANDING || player._playerState == PlayerController.State.INTERACTING)
        {
            stashUI.enabled = !isStashOpen;
            PlayerUI.enabled = !isStashOpen;
            isStashOpen = !isStashOpen;
            if (isStashOpen)
            {
                player.SetState(PlayerController.State.INTERACTING);
                StartCoroutine(player.SlowDown());
            }
            else
            {
                player.SetState(PlayerController.State.STANDING);
            }
        }
    }

    public bool IsStashOpen()
    {
        return isStashOpen;
    }
}
