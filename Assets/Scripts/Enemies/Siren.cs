using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Siren : MonoBehaviour
{
    [SerializeField]
    public float hp, attack, chaseRange, attackRange, speed, attackSpeed;
    [SerializeField]
    float lastAttackTime;

    NavMeshAgent navMeshAgent;



    public GameObject player;
    public bool closePlayer = false;
    [SerializeField] private float _lureRange = 30f;

    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;

    private enum State { EMITTING, NOT_EMITTING };
    private State _playerState;
    private bool _isLuring;

    private IEnumerator damageCoroutine;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        _isLuring = false;

        damageCoroutine = GiveDamageCoroutine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < chaseRange) 
        {
            Debug.DrawLine(transform.position, player.transform.position - (player.transform.position - transform.position).normalized);
            navMeshAgent.SetDestination(player.transform.position);
        }
        HandleLure();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }

/*    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            closePlayer = false;
    }*/

    private void HandleLure()
    {
        StartCoroutine(damageCoroutine);
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);

        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player"))
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) < _lureRange)
                {
                    subscriber.ReceiveMessage("Frequency");
                    StartCoroutine(damageCoroutine);
                    _isLuring = true;
                    break;
                }
                else if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) >= _lureRange) 
                {
                    subscriber.ReceiveMessage("Quiet");
                    _isLuring = false;
                    break;
                }
            }
            if (c.CompareTag("Enemy")) 
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) < _lureRange)
                {
                    subscriber.ReceiveMessage("Frequency");
                    _isLuring = true;
                    break;
                }
                else if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) >= _lureRange)
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

    private IEnumerator GiveDamageCoroutine()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);
        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player"))
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) < _lureRange)
                {
                    Debug.Log(c.GetComponent<Health>().Hp);
                    // Damages player more as they get closer to the siren
                    c.GetComponent<Health>().TakeDamage(30 / Vector3.Distance(player.transform.position, transform.position));
                    yield return new WaitForSeconds(5);
                    //GiveDamage();
                    //Debug.Log(c.GetComponent<Health>().Hp);
                }
            }
        }
    }
}
