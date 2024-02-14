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
    public float attractionForce = 20f;

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

        //damageCoroutine = GiveDamageCoroutine();

        //StartCoroutine(damageCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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

    private void Movement() {
        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            transform.LookAt(player.transform.position);
        }
        if (Vector3.Distance(player.transform.position, transform.position) < chaseRange)
        {
            Debug.DrawLine(transform.position, player.transform.position - (player.transform.position - transform.position).normalized * attackRange);
            navMeshAgent.SetDestination(player.transform.position - (player.transform.position - transform.position).normalized * attackRange);
        }
    }

    private void HandleLure()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _lureRange);

        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player"))
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) < _lureRange)
                {
                    subscriber.ReceiveMessage("Frequency");
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
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if ((c.CompareTag("Player") || c.CompareTag("Enemy")) && rb != null)
            {
                Vector3 direction = (transform.position - c.gameObject.transform.position).normalized;
                //rb.AddForce(direction * attractionForce * Time.deltaTime);
                //c.transform.LookAt(this.transform);
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
        while (true)
        {
            foreach (Collider c in targets)
            {
                if (c.CompareTag("Player"))
                {
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
                    if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) < _lureRange)
                    {
                        // Damages player more as they get closer to the siren
                        c.GetComponent<Health>().TakeDamage(_lureRange / Vector3.Distance(player.transform.position, transform.position));
                        yield return new WaitForSeconds(5);
                        Debug.Log(c.GetComponent<Health>().Hp);
                    }
                }
            }
        }
    }
}
