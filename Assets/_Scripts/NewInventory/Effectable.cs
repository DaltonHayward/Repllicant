using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effectable : MonoBehaviour, ISubscriber
{
    String emissionChannel;
    bool isEffected;
   
    void applyEffect()
    {

        
    }

    public void ReceiveMessage(string channel)
    {
        if (channel.Equals(emissionChannel))
        {
            applyEffect();
        }
    }
}
