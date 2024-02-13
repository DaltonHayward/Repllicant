using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour, ISubscriber
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed;
    float lastAttackTime;
    Transform player;
    NavMeshAgent navMeshAgent;
    public bool closePlayer = false;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;
    private void OnDestroy()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability)
        {
            //Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability)
        {
            //Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability + rareItemsProbability)
        {
            //Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else
        {
            //Instantiate(legendaryItems[Random.Range(0, legendaryItems.Count)], transform.position, Quaternion.identity);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = false;
    }
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

        if (Vector3.Distance(player.position, transform.position) < attackRange)
        {
            if (Time.time - lastAttackTime > attackSpeed)
            {
                Debug.Log("attack");
                lastAttackTime = Time.time;
            }
            return;
        }
        if (Vector3.Distance(player.position, transform.position) < chaseRange)
        {
            navMeshAgent.SetDestination(player.position - (player.position - transform.position).normalized);
        }

    }

    public void ReceiveMessage(string channel)
    {

        Debug.Log(channel);
        if (channel.Equals("Frequency"))
        {
            Debug.Log("Enemy in siren range");
        }
        else
        {
            
        }
    }
}
