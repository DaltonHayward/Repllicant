using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wood : Collectible, ISubscriber
{
    public GameObject dropWhenStoned; // drops when tree is stoned
    [HideInInspector]public GameObject dropItemStart; // drops when tree isn't stoned

    public bool isStoned = false;

    public Material stoneMaterial;
    public Material woodMaterial;

    private void Start()
    {
        dropItemStart = sureToDrop;
    }

    override public void TakeDamage(float damage)
    {

        hp -= damage;
        
        if (hp <= 0) {
            Destroy(gameObject);
            InstantiateLoot();
        }
    }

    // change tree to stoned; changes drop and material
    public void Stoned()
    {
        isStoned = true;
        sureToDrop = dropWhenStoned;
        GetComponent<MeshRenderer>().material = stoneMaterial;
    }

    public void UnStoned() // changes tree back to normal
    {
        isStoned = false;
        sureToDrop = dropItemStart;
        GetComponent<MeshRenderer>().material = woodMaterial;
    }

    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Petrified"))
        {
            Stoned();
        }
        else if (channel.Contains("Shocked")){
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
}
