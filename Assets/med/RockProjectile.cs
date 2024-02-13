using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float projTimeout = 3;
    public float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timeout(projTimeout));
    }

    IEnumerator Timeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

