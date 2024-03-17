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
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            if (Time.time - lastAttackTime > attackSpeed)
            {
                Debug.Log("Attacking");
                Attack();
                lastAttackTime = Time.time;
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

    public IEnumerator Skill()
    {
        Debug.Log("Using Skill");
        StartCoroutine(SkillTimer(2));
        yield return new WaitForSeconds(skillSpeed); // cooldown
    }

    void PetrifyTargetsInRange()
    {
        // grab references to all targets in range
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider target in targets)
        {
            if (target.gameObject != gameObject) // Avoid self-petrification
            {
                ISubscriber subscriber = target.GetComponent<ISubscriber>();
                if (subscriber != null)
                {
                    // get the direction we are facing 
                    Vector3 direction = (transform.position - player.transform.position);

                    // returns the angle (from, to)
                    float angleToMedusa = Vector3.Angle(direction, player.transform.forward);

                    // when the angle is at -90 or +90, then it is in view (180º FOV)
                    CharacterController cc = player.GetComponent<CharacterController>();
                    if (target.tag == "Player")
                    {
                        // if player is facing medusa, petrify them
                        if (angleToMedusa >= -90 && angleToMedusa <= 90)
                        {
                            Debug.Log("looking at medusa");
                            subscriber.ReceiveMessage("Petrified");
                        }
                        Debug.Log("not looking at medusa");
                    }
                    else
                    {
                        subscriber.ReceiveMessage("Petrified");
                    }
                }
            }
        }
    }

    // enables visual cone for attack, then 2 seconds later petrifies targets in range
    public IEnumerator SkillTimer(float seconds)
    {
        coneMesh.enabled = true;
        yield return new WaitForSeconds(seconds);
        PetrifyTargetsInRange();
        coneMesh.enabled = false;
    }
    }
