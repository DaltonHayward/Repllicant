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
        else if (channel == "Shock")
        {
            Shock();
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

    public void Shock()
    {
        StartCoroutine(ShockTimer());
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
         if(isBurning && isShocked){
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[3].sprite);
        }
        else{
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[1].sprite);
        
        }

        yield return new WaitForSeconds(burnTime);

        // reset sprite
        if(isShocked){
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[2].sprite);
        }
        else{
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[0].sprite);
        }
        isBurning = false;
    }

    IEnumerator ShockTimer()
    {
        isShocked = true;
        Inventory_Item item = GetComponent<Inventory_Item>();
        //Update sprite
        if(isBurning && isShocked){
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[3].sprite);
        }
        else{
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[2].sprite);
        
        }

        yield return new WaitForSeconds(burnTime);

        isShocked = false;

        // reset sprite
        if(isBurning){
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[1].sprite);
        }
        else{
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[0].sprite);
        }
    }

   
}
