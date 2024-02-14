using System.Collections;
using System.Collections.Generic;
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

    new public void TakeDamage(float damage)
    {

        hp -= damage;
        
        if (hp <= 0)
            Destroy(gameObject);
        instantiateLoot();
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
    }
}
