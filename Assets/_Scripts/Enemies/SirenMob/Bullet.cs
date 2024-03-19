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
            player.gameObject.GetComponent<PlayerController>().ReceiveMessage("DamageOnPlayer:" + attackDamage.ToString());
            player.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed);
    }
}
