using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Collectible, ISubscriber
{
    public GameObject dropWhenStoned; // drops when tree is stoned
    [HideInInspector]public GameObject baseItemDrop; // drops when tree isn't stoned

    public bool isStoned = false;
    private bool isBurning = false;

    public Material stoneMaterial;
    public Material woodMaterial;

    [SerializeField]
    public ParticleSystem fireSystem;
    private IEnumerator burnCD;

    private void Start()
    {
        baseItemDrop = sureToDrop;
        burnCD = BurnCooldown();
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
        sureToDrop = baseItemDrop;
        GetComponent<MeshRenderer>().material = woodMaterial;
    }

    private void Burn()
    {
        isBurning = true;

        if (!fireSystem.isPlaying)
        {
            fireSystem.Play();
        }

        StartCoroutine(burnCD);
    }

    IEnumerator BurnCooldown()
    {
        float rand = Random.Range(3f, 10f);
        yield return new WaitForSeconds(rand);
        isBurning = false;

        if (fireSystem.isPlaying)
        {
            fireSystem.Stop();
        }
    }

    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Petrified"))
        {
            Stoned();
        }

        if (channel.Equals("Burning"))
        {
            if (isBurning)
            {
                // reset burn cooldown
                StopCoroutine(burnCD);
                Burn();
            }
            else
            {
                float rand = Random.value;
                if (rand < 0.6)
                {
                    Burn();
                }
            }
        }
    }
}
