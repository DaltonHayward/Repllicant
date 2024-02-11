using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Medusa : MonoBehaviour
{
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed,skillSpeed;
    float lastAttackTime=-100,lastSkillTime=-100;
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
    void Update()
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
    }
    public IEnumerator SKill()
    {
        yield return new WaitForSeconds(1);//ǰҡһ��
        Debug.Log("Skilling");
        MeshCollider meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        meshCollider.isTrigger=true;
        yield return new WaitForSeconds(3);//3���ȡ��Ͷ����,����skillspeedҪ����3�룬��Ȼ��һֱ��
        meshCollider.isTrigger = false;
    }
}