using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
[RequireComponent(typeof(NavMeshAgent))]

public class Siren : MonoBehaviour, ISubscriber
{
    [SerializeField]
    public float hp, attack, chaseRange, speed, attractionForce;

    NavMeshAgent navMeshAgent;

    [SerializeField] private float _songRange;

    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;
    private Color baseColor;

    public GameObject player;

    private IEnumerator damageCoroutine;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        damageCoroutine = GiveDamageCoroutine();
        StartCoroutine(damageCoroutine);

        SirenSong ss = GetComponent<SirenSong>();
        ss.SetParameters(0.5f, _songRange, "Singing");

        //for (int i = 1; i < gameObject.transform.childCount; i++) 
        //{
            baseColor = gameObject.transform.GetChild(7).GetComponent<Renderer>().material.GetColor("_BaseColor");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attract();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < -0)
            Die();
    }

    public void Movement()
    {
        // calc distance to player
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < chaseRange && distanceToPlayer > 2f)
        {
            speed = 0.5f;
            transform.LookAt(player.transform.position);
            navMeshAgent.SetDestination((player.transform.position - transform.position).normalized * distanceToPlayer);
        }
        else 
        {
            speed = 0f;
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

                // get the distance between the player and the siren
                float dist = Vector3.Distance(player.transform.position, transform.position);

                if (angleToSiren >= -90 && angleToSiren <= 90 && dist < _songRange && dist > 3)
                {
                    attractionForce = _songRange / dist;
                    cc.Move(direction.normalized * (attractionForce * 0.8f) * Time.deltaTime);
                }
                else if (angleToSiren <= -90 || angleToSiren >= 90 && dist < _songRange && dist > 3)
                {
                    if (Input.anyKey)
                    {
                        // Weaken effect if player is trying to move away so player can escape if they want 
                        cc.Move(direction.normalized * (-dist / _songRange) * Time.deltaTime);
                    }
                    else
                    {
                        // resume attract as normal otherwise
                        cc.Move(direction.normalized * (attractionForce * 0.4f) * Time.deltaTime);
                    }
                }
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
                ChangeColor(Color.red);
                StartCoroutine(ResetColor());
            }
        }
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeColor(baseColor);
    }

    private void ChangeColor(Color color)
    {
        //for (int i = 1; i < gameObject.transform.childCount; i++)
        //{
            gameObject.transform.GetChild(7).GetComponent<Renderer>().material.SetColor("_BaseColor", color);
        //}
    }
}
