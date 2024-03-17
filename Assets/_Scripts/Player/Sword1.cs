using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword1 : Tool, ISubscriber
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit weapon");
            ISubscriber subscriber = other.GetComponent<ISubscriber>();
            if (subscriber != null)
            {
                subscriber.ReceiveMessage("Attacked:" + Damage);
            }
        }
    }

    public void ReceiveMessage(string channel)
    {
        if (channel == "Burning")
        {
            Debug.Log("sword sub");
        }
    }
}
