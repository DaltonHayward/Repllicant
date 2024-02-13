using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Tool
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit weapon");
            ISubscriber subscriber = other.GetComponent<ISubscriber>();
            if (subscriber != null)
            {
                subscriber.ReceiveMessage("Attacked");
            }
        }
    }
}
