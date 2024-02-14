using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Medusa : Enemy
{
    public float skillSpeed;
    public GameObject projectilePrefab; // Assign this in the Inspector with your projectile prefab
    public Transform projectileSpawnPoint;

    // for turn red when dmg taken effect
    private Color _originalMaterialColor;

    public void Awake()
    {
        _originalMaterialColor = GetComponentInChildren<SkinnedMeshRenderer>().materials[0].GetColor("_BaseColor");
    }


    public override void UpdateLogic()
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

    public override void Hurt()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
        StartCoroutine(Timeout(0.1f));
    }


    IEnumerator Timeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", _originalMaterialColor);
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

    /*public IEnumerator SKill()
    {
        yield return new WaitForSeconds(1);//前摇一秒
        Debug.Log("Skilling");
        MeshCollider meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        meshCollider.isTrigger = true;
        yield return new WaitForSeconds(3);//3秒后取消投射物,所以skillspeed要大于3秒，不然会一直放
        meshCollider.isTrigger = false;
    }*/

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


    //public float hp, attack, chaseRange, attackRange, speed, attackSpeed,skillSpeed;
    //float lastAttackTime=-100,lastSkillTime=-100;
    //Transform player;
    //NavMeshAgent navMeshAgent;
    //public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    //public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;//每种物品概率，加起来要等于1

    
    
    /*void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }*/
    /*void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            transform.LookAt(player.position);
            if (Time.time - lastAttackTime > attackSpeed)
            {
                Debug.Log("attack");
                lastAttackTime = Time.time;
            }
            if (Time.time - lastSkillTime > skillSpeed)
            {
                StartCoroutine(SKill());
                lastSkillTime = Time.time;
            }
            return;
        }
        if (Vector3.Distance(player.position, transform.position) < chaseRange)
        {
            Debug.DrawLine(transform.position, player.position - (player.position - transform.position).normalized * attackRange);
            navMeshAgent.SetDestination(player.position - (player.position - transform.position).normalized*attackRange);
        }
    }*/
}
