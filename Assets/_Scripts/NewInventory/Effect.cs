using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    String channel; 
    float EnvBroadcastRange;
    int InvBroadcastRange;
    bool inInventory;
    float EmitFrequency; 
    IEnumerator emissionCoroutine;
  
    /// <summary>
    /// sets the parameters of the Effect
    /// </summary>
    /// <param name="channel"> What signal you want to send/ items to target</param>
    /// <param name="EnvBroadcastRange"> A unit measurement to effect in a raidius in the env group  </param>
    /// <param name="InvBroadcastRange"> the grid radius that the item will send out</param>
    /// <param name="EmitFrequency"> how often the signal will be sent out</param>
    void setParamerters(String channel , float EnvBroadcastRange , int InvBroadcastRange , float EmitFrequency)
    {
        this.channel = channel;
        this.EnvBroadcastRange = EnvBroadcastRange;
        this.InvBroadcastRange = InvBroadcastRange;
        this.EmitFrequency = EmitFrequency;
    }

    /// <summary>
    /// Emits a signmal to all the subscribers in the range of the emitter 
    ///
    /// </summary>
    /// <returns> Nothing </returns>
     private IEnumerator CoEmit()
    {
        if (inInventory)
        {
            //no logic yet
        }
        else
        {
            while (true)
            {
                print(gameObject.name);
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, EnvBroadcastRange);

                foreach (Collider2D c in targets)
                {
                    ISubscriber r = c.GetComponent<ISubscriber>();
                    if (r != null)
                    {
                        r.ReceiveMessage(channel);
                    }
                }

                yield return new WaitForSeconds(EmitFrequency);
            }
        }
 
    }
  
}
