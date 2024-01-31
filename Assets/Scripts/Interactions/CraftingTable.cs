using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour, ISubscriber
{

    private Canvas _canvas;
    private Color _originalOutlineColor;

    void Awake()
    {
        // _canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
        // foreach (Transform child in transform)
        // {
        //     if (child.CompareTag("Canvas"))
        //     {
        //         _canvas = child.GetComponent<Canvas>();
        //     }
        // }

        _canvas = GetComponentInChildren<Canvas>();

        _canvas.enabled = false;
        _originalOutlineColor = GetComponent<Renderer>().material.GetColor("_OutlineColor");
    }

    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("OpenMenu"))
        {
            _canvas.enabled = true;
            Debug.Log("OpenMenu caught");
        }
        else if (channel.Equals("CloseMenu"))
        {
            _canvas.enabled = false;
            Debug.Log("CloseMenu caught");
        }
    }

    public Color GetOriginalOutline()
    {
        return _originalOutlineColor;
    }
}
