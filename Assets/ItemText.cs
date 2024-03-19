using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "Pick Up [" + InputManager.instance.GetInteractKey() + "]";
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized);
    }
}
