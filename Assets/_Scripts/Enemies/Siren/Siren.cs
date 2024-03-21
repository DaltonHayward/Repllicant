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
    public float hp, chaseRange, speed, attractionForce;

    NavMeshAgent navMeshAgent;

    [SerializeField] private float _songRange;

    public List<GameObject> commonItems, uncommonItems, rareItems, legendaryItems;
    public float commonItemProbability, uncommonItemsProbability, rareItemsProbability, legendaryItemsProbability;
    public float damageRate;
    private Color baseColor;

    public GameObject player;
    public GameObject timeManager;
    private Event e;

    private IEnumerator damageCoroutine;

    void Awake()
    {
        transform.rotation = Quaternion.identity;
        player = GameObject.FindGameObjectWithTag("Player");
        timeManager = GameObject.FindGameObjectWithTag("TimeManager");

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;

        damageCoroutine = GiveDamageCoroutine();
        StartCoroutine(damageCoroutine);

        SirenSong ss = GetComponent<SirenSong>();
        ss.SetParameters(damageRate, _songRange, "Singing");

        baseColor = gameObject.transform.GetChild(7).GetComponent<Renderer>().material.GetColor("_BaseColor");
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

        transform.LookAt(player.transform.position);
        if (distanceToPlayer < chaseRange && distanceToPlayer > 2f)
        {
            speed = 0.5f;
            navMeshAgent.SetDestination(player.transform.position - (player.transform.position - transform.position).normalized * (chaseRange - 1));
        }
        else
        {
            speed = 0.1f;
        }
    }

    private void OnDestroy()
    {
        if (legendaryItems != null)
            Instantiate(legendaryItems[0], transform.position, Quaternion.identity);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator GiveDamageCoroutine()
    {
        while (true)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, _songRange);
            foreach (Collider c in targets)
            {
                ISubscriber[] subs = c.GetComponents<ISubscriber>();
                if (subs != null)
                {
                    foreach (ISubscriber sub in subs)
                    {
                        if (c.gameObject.GetComponent<PlayerHealth>() != null)
                            if (timeManager.GetComponent<TimeManager>().CurrentTime >= 21f || timeManager.GetComponent<TimeManager>().CurrentTime <= 6f)
                            {
                                c.gameObject.GetComponent<PlayerHealth>().TakeDamage(1.5f * _songRange / Vector3.Distance(c.gameObject.transform.position, transform.position));
                                damageRate = 2f;
                            }
                            else
                            {
                                c.gameObject.GetComponent<PlayerHealth>().TakeDamage(_songRange / Vector3.Distance(c.gameObject.transform.position, transform.position));
                            }
                    }
                }
            }
            yield return new WaitForSeconds(damageRate);
        }
    }

    void OnGUI()
    {
        e = Event.current;
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
                    if (timeManager.GetComponent<TimeManager>().CurrentTime >= 21f || timeManager.GetComponent<TimeManager>().CurrentTime <= 6f)
                    {
                        attractionForce = (_songRange / dist) * 1.5f;
                    }
                    else
                    {
                        attractionForce = _songRange / dist;
                    }
                    cc.Move(direction.normalized * (attractionForce * 0.8f) * Time.deltaTime);
                }
                else if (angleToSiren <= -90 || angleToSiren >= 90 && dist < _songRange && dist > 3 || dist <= 2)
                {
                    // sometimes Input.anyKey || e.isKey works, sometimes only e.isKey does
                    if (Input.anyKey || e.isKey)
                    {
                        // Weaken effect if player is trying to move away so player can escape if they want
                        cc.Move(direction.normalized * (-dist / _songRange) * Time.deltaTime);
                    }
                    else
                    {
                        // resume attract as normal otherwise
                        cc.Move(direction.normalized * (attractionForce * 0.2f) * Time.deltaTime);
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
        // siren body flashes red when hit
        gameObject.transform.GetChild(7).GetComponent<Renderer>().material.SetColor("_BaseColor", color);
    }
}
