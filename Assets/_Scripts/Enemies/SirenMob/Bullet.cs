using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform player;
    public float speed=1;
    public float attackDamage;
    public float lifeTime = 15f;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        transform.LookAt(new Vector3(player.position.x, 1, player.position.z));
    }

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
        if (lifeTime > 0)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
