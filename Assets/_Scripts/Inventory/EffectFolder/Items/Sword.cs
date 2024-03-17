using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour, ISubscriber
{
    public void ReceiveMessage(string channel)
    {
        if (channel == "Burning")
        {
            Debug.Log("sword sub");
        }
    }
}

