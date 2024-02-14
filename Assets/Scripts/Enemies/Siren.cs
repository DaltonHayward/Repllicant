using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Siren : MonoBehaviour
{
    [SerializeField]
    public float hp, attack, chaseRange, speed;
    [SerializeField]
    public float attractionForce = 20f;

    NavMeshAgent navMeshAgent;

    [SerializeField] private float _lureRange = 30f;

    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;

    private bool _isLuring;
    public GameObject player;

    private IEnumerator damageCoroutine;

    void Awake()
    {
        damageCoroutine = GiveDamageCoroutine();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        _isLuring = false;
        StartCoroutine(damageCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        HandleLure();
        Attract();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }

    private void Movement() 
    {
        // calculate the distance bewteen the enemy and the player and 
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= _lureRange)
        {
            transform.LookAt(player.transform.position);
            return;
        }
        if (distanceToPlayer < chaseRange)
        {
            navMeshAgent.SetDestination(player.transform.position - (player.transform.position - transform.position).normalized * _lureRange);
        }
    }

    private void HandleLure()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);

        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player") || c.CompareTag("Enemy"))
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
        //Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);
        while (true)
        {
            /*foreach (Collider c in targets)
            {
                if (c.CompareTag("Player"))
                {
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
                    if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) < _lureRange)
                    {*/
                        // Damages player more as they get closer to the siren
                        player.GetComponent<Health>().TakeDamage(_lureRange / Vector3.Distance(player.transform.position, transform.position));
                        yield return new WaitForSeconds(5);
                        Debug.Log(player.GetComponent<Health>().Hp);
           /*         }
                }
            }*/
        }
    }

    private void Attract() 
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);
        foreach (Collider c in targets)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            Vector3 direction = (transform.position - c.gameObject.transform.position).normalized;
            //Debug.Log(direction);
            //rb.AddForce(direction * attractionForce * Time.deltaTime);
        }
    }

    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Attack"))
        {
            TakeDamage(Damage);
        }
    }
}
