using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform player;
    public float speed=1;
    public float attackDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ReceiveMessage("DamageOnPlayer:" + attackDamage.ToString());
        }
    }
    void Update()
    {
        transform.LookAt(player);
        transform.Translate(Vector3.MoveTowards(transform.position, player.position, speed));
    }
}
