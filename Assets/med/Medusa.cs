using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Medusa : MonoBehaviour
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, skillSpeed;
    float lastAttackTime = -100, lastSkillTime = -100;
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
                lastAttackTime = Time.time;
                Attack();
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
        yield return new WaitForSeconds(1); // Skill preparation time
        Debug.Log("Using Skill");
        PetrifyTargetsInRange();
        yield return new WaitForSeconds(3); // Duration before skill effect wears off
    }

    void PetrifyTargetsInRange()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider target in targets)
        {
            if (target.gameObject != gameObject) // Avoid self-petrification
            {
                PetrifyTarget(target.gameObject);
            }
        }
    }

    void PetrifyTarget(GameObject target)
    {
        StartCoroutine(PetrifyTargetCoroutine(target));
    }

    IEnumerator PetrifyTargetCoroutine(GameObject target)
    {
        var controller = target.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            target.GetComponent<Animator>().enabled = false;
        }

        var rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(2); // Petrification duration

        // Restore target's movement capability
        if (controller != null)
        {
            controller.enabled = true;
            target.GetComponent<Animator>().enabled = true;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnDestroy()
    {
        DropLoot();
    }

    void DropLoot()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability)
        {
            Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemProbability)
        {
            Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemProbability + rareItemProbability)
        {
            Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else
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
