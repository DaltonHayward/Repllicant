using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvTool : MonoBehaviour, ISubscriber, Burnable
{
    public bool isBurning = false;
    public float burnTime = 15f;
    public bool isShocked = false;
    public void ReceiveMessage(string channel)
    {
        if (channel == "Burning")
        {
            if (!isBurning)
            {
                TakeFireDamage();
            }

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
        StartCoroutine(BurnTimer());
    }

    IEnumerator BurnTimer()
    {
        isBurning = true;
        Inventory_Item item = GetComponent<Inventory_Item>();
        //Update sprite
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[1].sprite);

        yield return new WaitForSeconds(burnTime);

        // reset sprite
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[0].sprite);
        isBurning = false;
    }
}
