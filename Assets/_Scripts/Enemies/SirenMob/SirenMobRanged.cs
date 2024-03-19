using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class SirenMobRanged : Enemy
{
    public float skillSpeed;
    public GameObject projectilePrefab; // Assign this in the Inspector with your projectile prefab
    public Transform projectileSpawnPoint;

    // reference to cone attack
    public GameObject coneAttack;
    public MeshRenderer coneMesh;

    // for turn red when dmg taken effect
    private Color _originalMaterialColor;

    public void Awake()
    {
        _originalMaterialColor = GetComponentInChildren<SkinnedMeshRenderer>().materials[0].GetColor("_BaseColor");
        coneMesh = coneAttack.GetComponent<MeshRenderer>();
    }

    // controls Medusa movement
    public override void Update()
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
            }

            if (Time.time - lastSkillTime > skillSpeed)
            {
                //StartCoroutine(Skill());
                lastSkillTime = Time.time;
            }
            return;
        }

        if (distanceToPlayer < chaseRange)
        {
            navMeshAgent.SetDestination(player.position - (player.position - transform.position).normalized * attackRange);
        }
    }

    // changes medusa color to indicate she got damaged
    public override void Hurt()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
        StartCoroutine(HurtTimeout(0.1f));
    }

    IEnumerator HurtTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", _originalMaterialColor);
    }

    // medusa spawns a snake that moves forward
    void Attack()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            // Instantiate the projectile at the spawn point
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            projectile.transform.LookAt(player);
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

    }
