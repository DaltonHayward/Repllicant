using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stone : Collectible, ISubscriber
{
    public float hardnessPercent = 0.2f; // reduces damage taken by a percentage

    public void ReceiveMessage(string channel)
    {
            if (channel.Contains("Shocked")){
            Debug.Log("Enemy is shocked");
            string[] sections = channel.Split(':');
            string[] values = sections[1].Split(',');

            if (values.Length == 2)
            {
                float damage;
                int jumps;

                if (float.TryParse(values[0].Trim(), out damage) && int.TryParse(values[1].Trim(), out jumps))
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
                    Collider[] CollectiblesColliders = colliders.Where(collider => collider.gameObject != this.gameObject && collider.gameObject.GetComponent<Collectible>() != null).ToArray();
                    Debug.Log("Wood collectibles:"+CollectiblesColliders.Length);
                    if (CollectiblesColliders.Length > 0)
                    {
                        Collider collider = CollectiblesColliders[Random.Range(0, CollectiblesColliders.Length)];
                        if(jumps > 0)
                        {
                            GameObject lightning = Instantiate(InventoryController.instance.lightningEffect);
                            
                            lightning.transform.position = gameObject.transform.position;
                            lightning.GetComponent<LightningBullet>().SetDirection(collider.bounds.center,damage-5,"Shocked:"+damage);
                            damage = (float)(damage *.90);
                            jumps--;
                            
                            collider.gameObject.GetComponent<ISubscriber>().ReceiveMessage("Shocked:" + damage + "," + jumps);
                            TakeDamage(damage);
                        }
                        
                    }
                    else
                    {
                    TakeDamage(damage);
                    }
                }
            }
        }
    }

    // probably want some sort of way to penetrate hardness with a good pickaxe

    override public void TakeDamage(float damage)
    {
        if (hardnessPercent < 1) 
        {
            hp -= (damage - (damage * hardnessPercent));
        }

        if (hp <= 0) 
        {
            InstantiateLoot();
            Destroy(gameObject);
        }
         
    }
}
