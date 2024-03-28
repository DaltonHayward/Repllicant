
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour, ISubscriber
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, lastAttackTime, lastSkillTime;
    public Transform player;
    public NavMeshAgent navMeshAgent;
    protected Animator animator;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;
    bool rolledLoot= false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        animator = GetComponent<Animator>();
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
                Hurt();
            }
        }
        else if (channel.StartsWith("SpeedChange"))
        {
            navMeshAgent.speed *= float.Parse(parts[1].Trim());
        }
        else if (channel.StartsWith("Shocked:")){
            Debug.Log("Enemy is shocked");
            float damage;
            string[] test = channel.Split(':');
            string[] values = parts[1].Split(',');
            Debug.Log("message "+ test + " "+ values[0]+" "+ values[1);
            if (float.TryParse(parts[1].Trim(), out damage))
            {
                
                Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.GetComponent<ISubscriber>() != null && collider.gameObject.tag == "Enemy" && collider.gameObject != this.gameObject)
                    {
                        if(damage> 0){
                        collider.gameObject.GetComponent<ISubscriber>().ReceiveMessage("Shocked:"+(damage-1));}
                        break;
                    }
                }
                TakeDamage(damage);
                Hurt();
                
            }
        }
    }

    // effects for when enemy is hurt
    public virtual void Hurt()
    {
        // this is specific to the enemy type
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
