using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum  CowState
{
    idle,
    charge,
    attack,
    wave,
    chase,
    stone
}
[RequireComponent(typeof(NavMeshAgent))]
public class COW : MonoBehaviour
{
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
    float lastAttackTime = -100, lastSkillTime = -100;
    Transform player;
    NavMeshAgent navMeshAgent;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;//The probabilities of each item must add up to 1
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
            if (target[j].tag == "Player"|| target[j].tag == "Enemy")
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
    void Update()
    {
        switch (state)
        {
            case CowState.idle:

                playerIsDamageByCharge = false;
                if (Vector3.Distance(player.position, transform.position) < chaseRange)
                {
                    state = CowState.chase;
                    
                    break;
                }
                break;
            case CowState.chase:
                if (Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = CowState.attack;
                    navMeshAgent.enabled=false;

                    break ;
                }
                if(Vector3.Distance(player.position, transform.position) > chaseRange)
                {
                    state = CowState.idle;
                    navMeshAgent.enabled = false;
                    
                    break;
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
                    
                    break;
                }
                transform.LookAt( new Vector3(player.position.x,transform.position.y, player.position.z)  );
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Debug.Log($"Player taken damage from minotaur{attack}");

                    player.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= attack;
                    lastAttackTime = Time.time;
                }
                if (Time.time - lastSkillTime > skillSpeed)
                {
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
                    
                    }

                    lastSkillTime = Time.time;
                }
                break;
            case CowState.charge:
            
                if (Vector3.Distance(transform.position,player.position)> chargedistance)
                {
                    state = CowState.idle;
              
                    break;
                }
                Collider[] target ;
                transform.Translate(chargeDir* chargespeed*Time.deltaTime,Space.World);
                target = Physics.OverlapSphere(transform.position, 0.6f);
                for (int i = 0; i < target.Length; i++)
                {
                    if(target[i].tag== "Player"&& playerIsDamageByCharge==false)
                    {
                        Debug.Log($"Player taken charge damage{attack}");

                        target[i].gameObject.GetComponent<PlayerController>().SetState(PlayerController.State.strokeBack);
                        target[i].gameObject.GetComponent<PlayerController>().strokeBackTargetPosition = target[i].transform.position+ new Vector3(   (Quaternion.Euler(0,30f,0)* (target[i].transform.position-transform.position).normalized* strokeBackDistance).x, 0, (Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized* strokeBackDistance).z);
                        target[i].gameObject.GetComponent<PlayerHealth>().CurrentHealth -= attack;
                        
                        playerIsDamageByCharge = true;
                    }
                    //if (target[i].tag == "Enemy" && meduIsDamageByCharge == false)
                    //{
                    //    Debug.Log($"Medusa taken charge damage{attack}");
                    //    target[i].gameObject.GetComponent<Medusa>().hp -= attack;
                    //    target[i].gameObject.GetComponent<Medusa>().isStrokeBack =true;
                    //    target[i].gameObject.GetComponent<Medusa>().strokeBackTargetPosition = target[i].transform.position + new Vector3((Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized * strokeBackDistance).x, 0, (Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized * strokeBackDistance).z);
                    //    meduIsDamageByCharge = true;
                    //}
                }
                break;
            case CowState.stone:
                break;
            default:
                break;
        }
    }

    public IEnumerator Wave(GameObject player)
    {

   

        yield return new WaitForSeconds(1);//Delay before cast 1 sec
        Debug.Log($"The shock wave causes {WaveAttack} damage and slows down by 30%");
        if(player.tag=="Enemy")
        {
            player.GetComponent<NavMeshAgent>().speed *= 0.7f;
            player.GetComponent<Medusa>().hp -= WaveAttack;
            yield return new WaitForSeconds(3);//The projectile will be removed after 3 seconds, so the skillspeed must be greater than 3 seconds, otherwise it will keep firing.
            player.GetComponent<NavMeshAgent>().speed *= 10f / 7;
        }
        else if(player.tag == "Player")
        {
            player.GetComponent<PlayerController>().MoveSpeed *= 0.7f;
            player.GetComponent<PlayerHealth>().CurrentHealth -= WaveAttack;
            yield return new WaitForSeconds(3);//The projectile will be removed after 3 seconds, so the skillspeed must be greater than 3 seconds, otherwise it will keep firing.
            player.GetComponent<PlayerController>().MoveSpeed *= 10f / 7;
        }
      
      
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,chaseRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
