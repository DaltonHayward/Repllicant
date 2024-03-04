using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    String channel; 
    float EnvBroadcastRange;
    int InvBroadcastRange;
    bool inInventory;
    float EmitFrequency; 
    IEnumerator emissionCoroutine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void setParamerters(String channel , float EnvBroadcastRange , int InvBroadcastRange , float EmitFrequency)
    {
        this.channel = channel;
        this.EnvBroadcastRange = EnvBroadcastRange;
        this.InvBroadcastRange = InvBroadcastRange;
        this.EmitFrequency = EmitFrequency;
    }
    void CoEmit(){

    }
  
}
