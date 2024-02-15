using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
[RequireComponent(typeof(NavMeshAgent))]

public class Siren : MonoBehaviour
{
    [SerializeField]
    public float hp, attack, chaseRange, speed;
    [SerializeField]
    public float attractionForce = 20f;

    NavMeshAgent navMeshAgent;

    [SerializeField] private float _songRange = 30f;

    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;

    private bool _isLuring;
    public GameObject player;

    private IEnumerator damageCoroutine;

    void Awake()
    {
        _isLuring = false;
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        //damageCoroutine = GiveDamageCoroutine();
        //StartCoroutine(damageCoroutine);

        SirenSong ss = GetComponent<SirenSong>();
        ss.SetParameters(0.5f, _songRange, "Singing");
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        //HandleLure();
        //Attract();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }

    public void Movement()
    {
       
        // only check for new position every 5 seconds
        // calc distance to player
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < chaseRange && distanceToPlayer > 1.0f)
        {
            transform.LookAt(player.transform.position);
            navMeshAgent.SetDestination((player.transform.position - transform.position).normalized * distanceToPlayer);
        }
        
    }

    /*private void HandleLure()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _songRange);

        foreach (Collider c in targets)
        {
            // send signals to the player and all enemies telling them that they're in range of the siren
            if (c.CompareTag("Player") || c.CompareTag("Enemy"))
            {
                ISubscriber subscriber = c.GetComponent<ISubscriber>();
                if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) <= _songRange)
                {
                    subscriber.ReceiveMessage("Frequency");
                    _isLuring = true;
                }
                else if (subscriber != null && Vector3.Distance(c.gameObject.transform.position, transform.position) > _songRange)
                {
                    subscriber.ReceiveMessage("Quiet");
                    _isLuring = false;
                }
            }
        }
    }*/

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

    /*private IEnumerator GiveDamageCoroutine()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _songRange);
        while (Vector3.Distance(player.transform.position, transform.position) <= _songRange)
        {
            foreach (Collider c in targets)
            {
                if (c.CompareTag("Player"))
                {
                    ISubscriber subscriber = c.GetComponent<ISubscriber>();
                    if (subscriber != null && Vector3.Distance(player.transform.position, transform.position) <= _songRange)
                    {
                        // Damages player more as they get closer to the siren
                        player.GetComponent<PlayerHealth>().TakeDamage(_songRange / Vector3.Distance(player.transform.position, transform.position));
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
        Collider[] targets = Physics.OverlapSphere(transform.position, _songRange);
        foreach (Collider c in targets)
        {
            if (c.CompareTag("Player"))
            {
                // get the direction we are facing 
                Vector3 direction = (transform.position - player.transform.position);

                // returns the angle (from, to)
                float angleToSiren = Vector3.Angle(direction, player.transform.forward);

                // when the angle is at -90 or +90, then it is in view (180º FOV)
                CharacterController cc = player.GetComponent<CharacterController>();
                if (angleToSiren >= -90 && angleToSiren <= 90)
                {
                    Debug.Log("looking at siren");
                    cc.Move(direction.normalized * (attractionForce * Time.deltaTime));
                }
                else
                {
                    Debug.Log("looking away from siren");
                    cc.Move(direction.normalized * (attractionForce + player.GetComponent<PlayerController>().Speed * Time.deltaTime));
                }

            }
        }
    }*/


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
