using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Medusa : MonoBehaviour
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, skillSpeed;
<<<<<<< HEAD
    float lastAttackTime = -100, lastSkillTime = -100;
=======
    float lastAttackTime, lastSkillTime;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
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
<<<<<<< HEAD
        navMeshAgent.speed = speed;
=======
        navMeshAgent.speed = speed; 
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
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
<<<<<<< HEAD
                lastAttackTime = Time.time;
                Attack();
=======
                Attack();
                lastAttackTime = Time.time;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
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
<<<<<<< HEAD
        yield return new WaitForSeconds(1); // Skill preparation time
        Debug.Log("Using Skill");
        PetrifyTargetsInRange();
        yield return new WaitForSeconds(3); // Duration before skill effect wears off
=======
        Debug.Log("Using Skill");
        PetrifyTargetsInRange();
        yield return new WaitForSeconds(skillSpeed); // cooldown
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
    }

    void PetrifyTargetsInRange()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider target in targets)
        {
            if (target.gameObject != gameObject) // Avoid self-petrification
            {
<<<<<<< HEAD
                PetrifyTarget(target.gameObject);
=======
                ISubscriber subscriber = target.GetComponent<ISubscriber>();
                if (subscriber != null)
                {
                    subscriber.ReceiveMessage("Petrified");
                }
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
            }
        }
    }

<<<<<<< HEAD
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
            GetComponent<Animator>().enabled = false;
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
        }

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

=======
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
    private void OnDestroy()
    {
        DropLoot();
    }

    void DropLoot()
    {
        float randomValue = Random.value;
<<<<<<< HEAD
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
=======
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
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
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
