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
    chase
}
[RequireComponent(typeof(NavMeshAgent))]
public class COW : MonoBehaviour
{
 
    [Header("����ٶ�")]
    public float chargespeed;
    [Header("��̾���")]
    public float chargedistance;

    CowState state;
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed, skillSpeed;
    float lastAttackTime = -100, lastSkillTime = -100;
    Transform player;
    NavMeshAgent navMeshAgent;
    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;//ÿ����Ʒ���ʣ�������Ҫ����1
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
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }
    bool SKillIsdoing=false;

    Vector3 chargeDir;
    void Update()
    {
        switch (state)
        {
            case CowState.idle:
                
                if (Vector3.Distance(player.position, transform.position) < chaseRange)
                {
                    state = CowState.chase;
                    return;
                }
               
                break;
            case CowState.chase:
                if (Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = CowState.attack;
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
                if(SKillIsdoing==true)
                {
                    return;
                }
                if (Vector3.Distance(player.position, transform.position) > attackRange)
                {
                    state = CowState.chase;
                    return;
                }
                transform.LookAt( new Vector3(player.position.x,transform.position.y, player.position.z)  );
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Debug.Log("attack");
                    lastAttackTime = Time.time;
                }
                if (Time.time - lastSkillTime > skillSpeed)
                {
                    int i = Random.Range(0,1);
                    if(i==0)
                    {
                        state = CowState.charge;
                        chargeDir = new  Vector3((player.position - transform.position).normalized.x,0, (player.position - transform.position).normalized.z)  ;
               
                       
                    }
                    else if (i == 1)
                    {
                        StartCoroutine(SKill());
                    }

                    lastSkillTime = Time.time;
                }
                break;
            case CowState.charge:
                if(Vector3.Distance(transform.position,player.position)> chargedistance)
                {
                    state = CowState.idle;
                    GetComponentInChildren<ProjectOfCOW>().enabled = false;
                    return;
                }
                GetComponentInChildren<    ProjectOfCOW>().enabled = true;
                GetComponentInChildren<ProjectOfCOW>().isCharge = true;
                transform.Translate(chargeDir* chargespeed*Time.deltaTime,Space.World);
                Debug.Log(1);
                break;
            default:
                break;
        }
    }

    public IEnumerator SKill()
    {

        SKillIsdoing = true;
        GetComponentInChildren<ProjectOfCOW>().isCharge = false;
        yield return new WaitForSeconds(1);//ǰҡһ��
        Debug.Log("Skilling");
        GetComponentInChildren<ProjectOfCOW>().enabled = true;
        yield return new WaitForSeconds(3);//3���ȡ��Ͷ����,����skillspeedҪ����3�룬��Ȼ��һֱ��
        GetComponentInChildren<ProjectOfCOW>().enabled = true;
        SKillIsdoing = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,chaseRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
