using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DamageIndicator))]

public class Enemy : MonoBehaviour, ISubscriber
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, lastAttackTime, lastSkillTime;
    public Transform player;
    public NavMeshAgent navMeshAgent;
    protected Animator animator;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;
    bool rolledLoot= false;

    // for red flash on damage taken
    private DamageIndicator _damageIndicator;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        animator = GetComponent<Animator>();
        _damageIndicator = GetComponent<DamageIndicator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            transform.LookAt(player.position);
            if (Time.time - lastAttackTime > attackSpeed)
            {
                Debug.Log("attack");
                lastAttackTime = Time.time;
            }
            return;
        }
        if (Vector3.Distance(player.position, transform.position) < chaseRange)
        {
            Debug.DrawLine(transform.position, player.position - (player.position - transform.position).normalized * attackRange);
            //navMeshAgent.SetDestination(player.position - (player.position - transform.position).normalized * attackRange);
        }
    }

    public virtual void ReceiveMessage(string channel)
    {
        // split message into parts
        string[] parts = channel.Split(':');

        // handles attack message
        if (channel.StartsWith("Attacked"))
        {
            Debug.Log("Enemy attacked");
            // apply damage from message
            float damage;
            if (float.TryParse(parts[1].Trim(), out damage))
            {
                TakeDamage(damage);
                _damageIndicator.Hurt();
            }
        }
        else if (channel.StartsWith("SpeedChange"))
        {
            navMeshAgent.speed *= float.Parse(parts[1].Trim());
        }
        else if (channel.StartsWith("Shocked:"))
        {
            Debug.Log("Enemy is shocked");
            string[] sections = channel.Split(':');
            string[] values = sections[1].Split(',');
            Debug.Log(values.Length);
            Debug.Log(values[0]);

            if (values.Length == 2)
            {
                float damage;
                int jumps;

                if (float.TryParse(values[0].Trim(), out damage) && int.TryParse(values[1].Trim(), out jumps))
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
                    Collider[] enemyColliders = colliders.Where(collider => collider.gameObject.CompareTag("Enemy") && collider.gameObject!=this.gameObject).ToArray();
                    if(enemyColliders.Length > 0)
                    {
                        Collider collider = enemyColliders[Random.Range(0, enemyColliders.Length)];
                        if(jumps > 0)
                        {
                            GameObject lightning = Instantiate(InventoryController.instance.lightningEffect);
                            
                            lightning.transform.position = gameObject.transform.position;
                            lightning.GetComponent<LightningBullet>().SetDirection(collider.bounds.center,damage-5,"Shocked:"+damage);
                            damage = (float)(damage *.90);
                            jumps--;
                            
                            collider.gameObject.GetComponent<ISubscriber>().ReceiveMessage("Shocked:" + damage + "," + jumps);
                            TakeDamage(damage);
                            _damageIndicator.Hurt();
                        }
                        
                    }
                    else
                    {
                    TakeDamage(damage);
                    _damageIndicator.Hurt();
                    }
                }
            }
        }
    }
    // rolls for loot to instantiate
    private void RollLoot()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability && commonItems.Count > 0)
        {
            Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability && uncommonItems.Count > 0)
        {
            Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability + rareItemsProbability && rareItems.Count > 0)
        {
            Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else if (legendaryItems.Count > 0)
        {
            Instantiate(legendaryItems[Random.Range(0, legendaryItems.Count)], transform.position, Quaternion.identity);
        }
    }

    // on death
    public virtual void Die()
    {
        if (rolledLoot)
        {
            return;
        }
        RollLoot();
        rolledLoot = true;
        Destroy(gameObject);
    }

    // handles damage taken
    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
}
