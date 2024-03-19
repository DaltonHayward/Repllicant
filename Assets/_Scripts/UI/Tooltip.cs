using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.UI;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Numerics;
using Unity.VisualScripting;
using System;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public void SetText(string content, string header)
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        
        layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
        }
        

        //Vector2 position = Input.mousePosition;

        //transform.position = position;

    }


}
