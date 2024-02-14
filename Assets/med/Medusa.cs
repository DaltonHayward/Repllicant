using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Medusa : MonoBehaviour
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, skillSpeed;
    float lastAttackTime, lastSkillTime;
    Transform player;
    NavMeshAgent navMeshAgent;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemProbability, rareItemProbability, legendaryItemProbability;

    public GameObject projectilePrefab; // Assign this in the Inspector with your projectile prefab
    public Transform projectileSpawnPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed; 
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= attackRange)
        {
            transform.LookAt(player.position);
            if (Time.time - lastAttackTime > attackSpeed)
            {
                Debug.Log("Attacking");
                Attack();
                lastAttackTime = Time.time;
                // Implement attack logic here (e.g., reduce player HP)
            }

            if (Time.time - lastSkillTime > skillSpeed)
            {
                StartCoroutine(Skill());
                lastSkillTime = Time.time;
            }
            return;
        }

        if (distanceToPlayer < chaseRange)
        {
            navMeshAgent.SetDestination(player.position - (player.position - transform.position).normalized * attackRange);
        }
    }

    void Attack()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            // Instantiate the projectile at the spawn point
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb)
            {
                // Calculate the direction from the spawn point to the player
                Vector3 direction = (player.position - projectileSpawnPoint.position).normalized;

                // Add force to the projectile to launch it
                rb.AddForce(direction * speed, ForceMode.VelocityChange);
            }
        }
    }

    public IEnumerator Skill()
    {
        Debug.Log("Using Skill");
        PetrifyTargetsInRange();
        yield return new WaitForSeconds(skillSpeed); // cooldown
    }

    void PetrifyTargetsInRange()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider target in targets)
        {
            if (target.gameObject != gameObject) // Avoid self-petrification
            {
                ISubscriber subscriber = target.GetComponent<ISubscriber>();
                if (subscriber != null)
                {
                    subscriber.ReceiveMessage("Petrified");
                }
            }
        }
    }

    private void OnDestroy()
    {
        DropLoot();
    }

    void DropLoot()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability && commonItems.Count != 0)
        {
            Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemProbability && uncommonItems.Count != 0)
        {
            Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemProbability + rareItemProbability && rareItems.Count != 0)
        {
            Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemProbability + rareItemProbability + legendaryItemProbability && legendaryItems.Count != 0)
        {
            Instantiate(legendaryItems[Random.Range(0, legendaryItems.Count)], transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
