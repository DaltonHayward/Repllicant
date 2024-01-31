using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour, ISubscriber
{

    private Canvas _canvas;

    void Awake()
    {
        _canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
        _canvas.enabled = false;
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
 
}
