using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Tool, ISubscriber, Burnable
{
    private bool isBurning = false;
    private float burnTime = 15f;

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
            if (!isBurning)
            {
                TakeFireDamage();
                Debug.Log("Sword");
            }
            
        }
    }

    public void TakeFireDamage()
    {
        float igniteChance = Random.value;

        //Debug.Log(igniteChance);

        if (igniteChance < 0.01)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        StartCoroutine(BurnTimer());
    }

    IEnumerator BurnTimer()
    {
        isBurning = true;
        Inventory_Item item = GetComponent<Inventory_Item>();
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[1].sprite);
        yield return new WaitForSeconds(burnTime);
        isBurning = false;
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[0].sprite);
    }
}
