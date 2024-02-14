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
        _isLuring = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
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
        // calculate the distance bewteen the enemy and the player and move towards them
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= _lureRange)
        {
            transform.LookAt(player.transform.position);
        }
        if (distanceToPlayer < chaseRange)
        {
            navMeshAgent.SetDestination((player.transform.position - transform.position).normalized * _lureRange);
        }
    }
    
    private void HandleLure()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);

        foreach (Collider c in targets)
        {
            // send signals to the player and all enemies telling them that they're in range of the siren
            if (c.CompareTag("Player") || c.CompareTag("Enemy"))
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) <= _lureRange)
                {
                    subscriber.ReceiveMessage("Frequency");
                    _isLuring = true;
                    //break;
                }
                else if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) > _lureRange)
                {
                    subscriber.ReceiveMessage("Quiet");
                    _isLuring = false;
                    //break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        float randomValue = Random.value;
        if (randomValue < commonItemProbability)
        {
            if (commonItems != null)
                Instantiate(commonItems[Random.Range(0, commonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability)
        {
            if (uncommonItems != null)
                Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], transform.position, Quaternion.identity);
        }
        else if (randomValue < commonItemProbability + uncommonItemsProbability + rareItemsProbability)
        {
            if (rareItems != null)
                Instantiate(rareItems[Random.Range(0, rareItems.Count)], transform.position, Quaternion.identity);
        }
        else
        {
            if (legendaryItems != null)
                Instantiate(legendaryItems[Random.Range(0, legendaryItems.Count)], transform.position, Quaternion.identity);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator GiveDamageCoroutine()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);
        while (Vector3.Distance(player.transform.position, transform.position) <= _lureRange)
        {
            foreach (Collider c in targets)
            {
                if (c.CompareTag("Player"))
                {
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
                    if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) <= _lureRange)
                    {
                        // Damages player more as they get closer to the siren
                        player.GetComponent<PlayerHealth>().TakeDamage(_lureRange / Vector3.Distance(player.transform.position, transform.position));
                        yield return new WaitForSeconds(5);
                        Debug.Log(player.GetComponent<PlayerHealth>().CurrentHealth);
                    }
                }
            }
        }
    }

    private void Attract() 
    {
        // player should gravitate toward siren when in range
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);
        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player"))
            {
                CharacterController cc = player.GetComponent<CharacterController>();
                Vector3 direction = (transform.position - player.transform.position).normalized;
                cc.Move(direction * attractionForce * Time.deltaTime);
            }
        }
    }


    public void ReceiveMessage(string channel)
    {
        // split message into parts
        string[] parts = channel.Split(':');

        // handles attack message
        if (channel.StartsWith("Attacked"))
        {
            // apply damage from message
            float damage;
            if (float.TryParse(parts[1].Trim(), out damage))
            {
                TakeDamage(damage);
                
            }
        }
    }
}
