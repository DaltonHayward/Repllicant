using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public class Wood : Collectible, ISubscriber
{
    public GameObject dropWhenStoned; // drops when tree is stoned
    public GameObject burningItemDrop;
    [HideInInspector]public GameObject dropItemStart; // drops when tree isn't stoned

    public bool isStoned = false;
    private bool isBurning = false;

    public Material stoneMaterial;
    public Material woodMaterial;


    // burn system
    [SerializeField]
    public ParticleSystem fireSystem;
    [Range(0,1)]
    public float burnChance = 0.7f;
    private Coroutine burnCD;
    private Coroutine burnTick;
    private Light fireLight;

    private void Awake()
    {
        dropItemStart = sureToDrop;
        fireLight = fireSystem.GetComponentInChildren<Light>();
    }

    override public void TakeDamage(float damage)
    {
        hp -= damage;
        // add collision sound trigger here - check if 'stoned' lmao
        if (!isBurning)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Tool/WoodHit", GetComponent<Transform>().position);
        }
        if (isStoned)
        {
            // playoneshot for stone strike
        }
        
        if (hp <= 0) {
            Destroy(gameObject);
            // add tree falling sound here
            InstantiateLoot();
        }
    }

    // change tree to stoned; changes drop and material
    public void Stoned()
    {
        StopBurn();
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

    private void Burn()
    {
        if (isStoned) { return; }

        isBurning = true;
        sureToDrop = burningItemDrop;
        fireLight.enabled = true;

        if (!fireSystem.isPlaying)
        {
            fireSystem.Play();
        }

        burnCD = StartCoroutine(BurnCooldown());
        burnTick = StartCoroutine(BurnTick());
    }

    private void StopBurn()
    {
        // stop all fire related effects when set to stone
        if (isBurning)
        {
            isBurning = false;
            StopCoroutine(burnTick);
            StopCoroutine(burnCD);
            fireLight.enabled = false;

            if (fireSystem.isPlaying)
            {
                fireSystem.Stop();
            }
        }
    }

    IEnumerator BurnCooldown()
    {
        float rand = Random.Range(5f, 10f);
        yield return new WaitForSeconds(rand);
        isBurning = false;
        sureToDrop = dropItemStart;
        fireLight.enabled = false;

        if (fireSystem.isPlaying)
        {
            fireSystem.Stop();
        }

        StopCoroutine(burnTick);
    }

    IEnumerator BurnTick()
    {
        while (true)
        {
            TakeDamage(3f);
            yield return new WaitForSeconds(1);
        }
    }

    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Petrified"))
        {
            Stoned();
        }
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
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
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

        if (channel.Equals("Burning"))
        {
            if (isBurning)
            {
                // reset burn cooldown
                StopCoroutine(burnCD);
                StopCoroutine(burnTick);
                Burn();
            }
            else
            {
                float rand = Random.value;
                if (rand < burnChance)
                {
                    Burn();
                }
            }
        }
    }
}
