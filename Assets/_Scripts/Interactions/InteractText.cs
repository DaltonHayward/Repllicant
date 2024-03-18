using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    [SerializeField] Interactor interactor;
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = interactor.interactMessage + "[" + InputManager.instance.GetInteractKey() + "]";
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized);
    }
}
