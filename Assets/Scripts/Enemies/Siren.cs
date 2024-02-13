using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siren : MonoBehaviour
{
    [SerializeField]
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed;
    [SerializeField]
    float lastAttackTime;

    UnityEngine.AI.NavMeshAgent navMeshAgent;

    public GameObject player;
    public bool closePlayer = false;
    [SerializeField] private float _lureRange = 30f;

    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;

    private enum State { EMITTING, NOT_EMITTING };
    private State _playerState;
    private bool _isLuring;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        /*navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = speed;*/
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            if (Time.time - lastAttackTime > attackSpeed)
            {
                Debug.Log("attack");
                lastAttackTime = Time.time;
            }
            return;
        }
        HandleLure();
        //navMeshAgent.SetDestination(player.transform.position - (player.transform.position - transform.position).normalized);

    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = false;
    }

    private void HandleLure()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);

        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player"))
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) < chaseRange)
                {
                    subscriber.ReceiveMessage("Frequency");
                    _isLuring = true;
                    break;
                }
                else if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) >= chaseRange) {
                    subscriber.ReceiveMessage("Quiet");
                    _isLuring = false;
                    break;
                }
            }
            if (c.CompareTag("Enemy")) {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) < chaseRange)
                {
                    subscriber.ReceiveMessage("Frequency");
                    _isLuring = true;
                    break;
                }
                else if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) >= chaseRange)
                {
                    subscriber.ReceiveMessage("Quiet");
                    _isLuring = false;
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        float randomValue = Random.value;
        /*if (randomValue < commonItemProbability)
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
        }*/
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
