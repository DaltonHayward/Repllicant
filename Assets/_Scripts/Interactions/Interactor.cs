using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    // these are just the messages displayed during/failing an interaction
    [SerializeField] public String interactMessage = "Interact ";
    [SerializeField] public String failedCheckText = "Cannot Interact";
    [SerializeField] TextMeshProUGUI interactText;
    // this can be changed to prevent interactions
    [SerializeField] public bool interactable = true;
    // assign any behaviour on interaction
    [SerializeField] UnityEvent interactBehaviour;
    private OpenStash stashScript;
    private PlayerController _playerController;

    private void Awake()
    {
        stashScript = GetComponentInParent<OpenStash>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }


    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (InputManager.instance.InteractInput)
            {
                if (interactable)
                {
                    if (interactBehaviour != null)
                    {
                        interactBehaviour.Invoke();
                    }
                }
                else
                {
                    StartCoroutine(DisplayFailedConditionalText());
                }
            }
        }
    }

    public void FailedCheck()
    {
        StartCoroutine(DisplayFailedConditionalText());
    }

    // on fail
    IEnumerator DisplayFailedConditionalText()
    {
        interactText.text = failedCheckText;
        interactText.color = Color.red;
        yield return new WaitForSeconds(1);
        interactText.text = interactMessage + "[" + InputManager.instance.GetInteractKey() + "]";
        interactText.color = Color.white;
    }

    void OnTriggerExit(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
