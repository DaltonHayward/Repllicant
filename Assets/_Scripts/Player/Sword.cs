using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Tool, ISubscriber, Burnable
{
    private bool isBurning = false;

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
            TakeFireDamage();
            Debug.Log("Sword");
        }
    }

    public void TakeFireDamage()
    {
        float igniteChance = Random.value;

        //Debug.Log(igniteChance);

        if (igniteChance < 0.05)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        isBurning = true;
        //gameObject.GetComponent<Inventory_Item>().itemIcon
    }
}
