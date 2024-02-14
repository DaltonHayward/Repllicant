<<<<<<< HEAD
using System.Collections;
=======
<<<<<<< HEAD
using System.Collections;
=======
﻿using System.Collections;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
>>>>>>> b8779168c47a5b519ff55258e747bba22ad93baa
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum  CowState
{
    idle,
    charge,
    attack,
    wave,
<<<<<<< HEAD
    chase,
    stone
=======
    chase
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
}
[RequireComponent(typeof(NavMeshAgent))]
public class COW : MonoBehaviour
{
<<<<<<< HEAD
    [Header("Charge knockback distance")]
    public float strokeBackDistance;
    [Header("Shockwave damage")]
    public float WaveAttack;
    [Header("Charge speed")]
    public float chargespeed;
    [Header("Charge distance")]
    public float chargedistance;

   public CowState state;
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, skillSpeed;
=======
 
    
    public float chargespeed;
    public float chargedistance;

    CowState state;
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, skillSpeed, waveDamage;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
    float lastAttackTime = -100, lastSkillTime = -100;
    Transform player;
    NavMeshAgent navMeshAgent;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
<<<<<<< HEAD
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;//The probabilities of each item must add up to 1
=======
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }


>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
    private void OnDestroy()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability)
        {
            Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability)
        {
            Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability + rareItemsProbability)
        {
            Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(legendaryItems[Random.Range(0, legendaryItems.Count)], transform.position, Quaternion.identity);
        }
    }
<<<<<<< HEAD
    public void Die()
    {
        Destroy(gameObject);
    }
    public int lookAccurate = 10;
    float subAngle;
    List<GameObject> target = new List<GameObject>();
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }
    public void LookAround()
    {
        target.Clear();

        RaycastHit hit;
        for (int i = 0; i < lookAccurate; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0) * transform.forward, out hit, attackRange + 0.1f) && !target.Contains(hit.collider.gameObject))
            {

                target.Add(hit.collider.gameObject);
            }
            if (Physics.Raycast(transform.position, Quaternion.Euler(0, subAngle * (i + 1), 0) * transform.forward, out hit, attackRange + 0.1f) && !target.Contains(hit.collider.gameObject))
            {

                target.Add(hit.collider.gameObject);
            }




        }
        for (int j = 0; j < target.Count; j++)
        {
            if (target[j].tag == "Player"|| target[j].tag == "Medu")
            {
                StartCoroutine(Wave(target[j]));

            }
          
        }
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        subAngle = 45f / lookAccurate;
    }


    Vector3 chargeDir;
    bool playerIsDamageByCharge=false;
    bool meduIsDamageByCharge = false;
=======
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
    void Update()
    {
        switch (state)
        {
            case CowState.idle:
<<<<<<< HEAD

                playerIsDamageByCharge = false;
                if (Vector3.Distance(player.position, transform.position) < chaseRange)
                {
                    state = CowState.chase;
                    return;
                }
               
                break;
=======
                if (Vector3.Distance(player.position, transform.position) < chaseRange)
                {
                    state = CowState.chase;
                }
                break; 

>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
            case CowState.chase:
                if (Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = CowState.attack;
<<<<<<< HEAD
                    navMeshAgent.enabled=false;
                    return;
                }
                if(Vector3.Distance(player.position, transform.position) > chaseRange)
                {
                    state = CowState.idle;
                    navMeshAgent.enabled = false;
                    return;
                }
                navMeshAgent.enabled = true;
                Vector3 targetPos = player.position - (player.position - transform.position).normalized * (attackRange-1);
                targetPos.y = transform.position.y;
                navMeshAgent.SetDestination( targetPos  );
            
                break;
            case CowState.attack:
                if (Vector3.Distance(player.position, transform.position) > attackRange)
                {
                    state = CowState.chase;
                    return;
                }
                transform.LookAt( new Vector3(player.position.x,transform.position.y, player.position.z)  );
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Debug.Log($"Player taken damage from minotaur{attack}");
                    player.gameObject.GetComponent<PlayerHealth>().currentHealth -= attack;
=======
                    navMeshAgent.enabled = false;
                }
                if (Vector3.Distance(player.position, transform.position) > chaseRange)
                {
                    state = CowState.idle;
                    navMeshAgent.enabled = false;
                }
                navMeshAgent.enabled = true;
                Vector3 targetPos = player.position - (player.position - transform.position).normalized * (attackRange - 1);
                targetPos.y = transform.position.y;
                navMeshAgent.SetDestination(targetPos);
                break;
                
            case CowState.attack:

                if (SKillIsdoing == true)
                {
                    break;
                }
                if (Vector3.Distance(player.position, transform.position) > attackRange)
                {
                    state = CowState.chase;
                    break;
                }

                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Debug.Log("attack");
                    MeleeAttack();
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
                    lastAttackTime = Time.time;
                }
                if (Time.time - lastSkillTime > skillSpeed)
                {
<<<<<<< HEAD
                    int i = Random.Range(0,2);
                    if(i==0)
                    {
                        state = CowState.charge;
                        chargeDir = new  Vector3((player.position - transform.position).normalized.x,0, (player.position - transform.position).normalized.z)  ;
               
                       
                    }
                    else if (i == 1)
                    {
                        //Shock wave
                        LookAround();
                    
=======
                    int i = Random.Range(0, 1);
                    if (i == 0)
                    {
                        state = CowState.charge;
                        chargeDir = new Vector3((player.position - transform.position).normalized.x, 0, (player.position - transform.position).normalized.z);


                    }
                    else if (i == 1)
                    {
                        StartCoroutine(SKill());
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
                    }

                    lastSkillTime = Time.time;
                }
                break;
            case CowState.charge:
<<<<<<< HEAD
            
                if (Vector3.Distance(transform.position,player.position)> chargedistance)
                {
                    state = CowState.idle;
              
                    return;
                }
                Collider[] target ;
                transform.Translate(chargeDir* chargespeed*Time.deltaTime,Space.World);
                target = Physics.OverlapSphere(transform.position, 0.6f);
                for (int i = 0; i < target.Length; i++)
                {
                    if(target[i].tag== "Player"&& playerIsDamageByCharge==false)
                    {
                        Debug.Log($"Player taken charge damage{attack}");
                        target[i].gameObject.GetComponent<PlayerHealth>().currentHealth -= attack;
                        target[i].gameObject.GetComponent<PlayerController>().SetState(PlayerController.State.)
                        target[i].gameObject.GetComponent<PlayerController>().strokeBackTargetPosition = target[i].transform.position+ new Vector3(   (Quaternion.Euler(0,30f,0)* (target[i].transform.position-transform.position).normalized* strokeBackDistance).x, 0, (Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized* strokeBackDistance).z);
                        playerIsDamageByCharge = true;
                    }
                    if (target[i].tag == "Medu" && meduIsDamageByCharge == false)
                    {
                        Debug.Log($"Medusa taken charge damage{attack}");
                        target[i].gameObject.GetComponent<Medusa>().hp -= attack;
                        target[i].gameObject.GetComponent<Medusa>().isStrokeBack =true;
                        target[i].gameObject.GetComponent<Medusa>().strokeBackTargetPosition = target[i].transform.position + new Vector3((Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized * strokeBackDistance).x, 0, (Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized * strokeBackDistance).z);
                        meduIsDamageByCharge = true;
                    }
                }
                break;
            case CowState.stone:
=======

                if (Vector3.Distance(transform.position, player.position) > chargedistance)
                {
                    state = CowState.idle;
                    GetComponentInChildren<ProjectOfCOW>().enabled = false;
                    break;
                }
                GetComponentInChildren<ProjectOfCOW>().enabled = true;
                GetComponentInChildren<ProjectOfCOW>().isCharge = true;
                transform.Translate(chargeDir * chargespeed * Time.deltaTime, Space.World);
                Debug.Log(1);
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
                break;
            default:
                break;
        }
    }

<<<<<<< HEAD
    public IEnumerator Wave(GameObject player)
    {

   

        yield return new WaitForSeconds(1);//Delay before cast 1 sec
        Debug.Log($"The shock wave causes {WaveAttack} damage and slows down by 30%");
        if(player.tag=="Medu")
        {
            player.GetComponent<NavMeshAgent>().speed *= 0.7f;
            player.GetComponent<Medusa>().hp -= WaveAttack;
            yield return new WaitForSeconds(3);//The projectile will be removed after 3 seconds, so the skillspeed must be greater than 3 seconds, otherwise it will keep firing.
            player.GetComponent<NavMeshAgent>().speed *= 10f / 7;
        }
        else if(player.tag == "Player")
        {
            player.GetComponent<PlayerController>().MoveSpeed *= 0.7f;
            player.GetComponent<PlayerHealth>().currentHealth -= WaveAttack;
            yield return new WaitForSeconds(3);//The projectile will be removed after 3 seconds, so the skillspeed must be greater than 3 seconds, otherwise it will keep firing.
            player.GetComponent<PlayerController>().MoveSpeed *= 10f / 7;
        }
      
      
=======
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
    void MeleeAttack()
    {
        // Detect targets within the range
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Player"));
        foreach (var hitPlayer in hitPlayers)
        {
            Debug.Log("Hit " + hitPlayer.name);
            hitPlayer.GetComponent<PlayerHealth>().TakeDamage(attack);
        }
    }
    bool SKillIsdoing=false;

    Vector3 chargeDir;

    public IEnumerator SKill()
    {

        SKillIsdoing = true;
        GetComponentInChildren<ProjectOfCOW>().isCharge = false;
        yield return new WaitForSeconds(1);
        Debug.Log("Skilling");
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Player"));
        foreach (var hitEnemy in hitEnemies)
        {
            // Applies damage and slowing effects to each enemy hit
            Debug.Log("Shockwave hits " + hitEnemy.name);
            hitEnemy.GetComponent<PlayerHealth>().TakeDamage(waveDamage);
        }


        GetComponentInChildren<ProjectOfCOW>().enabled = true;
         yield return new WaitForSeconds(3);
        GetComponentInChildren<ProjectOfCOW>().enabled = true;
        SKillIsdoing = false;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,chaseRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
