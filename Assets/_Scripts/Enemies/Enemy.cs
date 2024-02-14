using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour, ISubscriber
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, lastAttackTime, lastSkillTime;
    public Transform player;
    public NavMeshAgent navMeshAgent;
    public bool closePlayer = false;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLogic();
    }

    public virtual void UpdateLogic()
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

    public void ReceiveMessage(string channel)
    {
        // split message into parts
        string[] parts = channel.Split(':');

        // handles attack message
        if (channel.StartsWith("Attacked"))
        {
            // apply damage from message
            float damage;
            if (float.TryParse(parts[1].Trim(), out damage))
            {
                TakeDamage(damage);
                Hurt();
            }
        }
    }

    // effects for when enemy is hurt
    public virtual void Hurt()
    {

    }

    // rolls for loot to instantiate
    private void RollLoot()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability)
        {
            if (commonItems != null)
            Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability)
        {
            if (uncommonItems != null)
                Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability + rareItemsProbability)
        {
            if (rareItems != null)
                Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else
        {
            if (legendaryItems != null)
                Instantiate(legendaryItems[Random.Range(0, legendaryItems.Count)], transform.position, Quaternion.identity);
        }
    }

    // on death
    public virtual void Die()
    {
        Destroy(gameObject);
        RollLoot();
    }

    // handles damage taken
    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Die();
    }

    // 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = true;
    }

    //
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = false;
    }
}
