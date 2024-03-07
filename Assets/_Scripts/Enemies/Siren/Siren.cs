using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
[RequireComponent(typeof(NavMeshAgent))]

public class Siren : Enemy
{
    [SerializeField]
    public float attractionForce = 20f;

    [SerializeField] private float _songRange = 30f;

    private IEnumerator damageCoroutine;

    void Awake()
    {
        Debug.Log("Siren spawned");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        damageCoroutine = GiveDamageCoroutine();
        StartCoroutine(damageCoroutine);

        SirenSong ss = GetComponent<SirenSong>();
        ss.SetParameters(0.5f, _songRange, "Singing");
    }

    // Update is called once per frame
    public override void Update()
    {
        Attract();
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
