using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float Damage;
    private bool BypassInvinsible;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>()) 
        {
            other.GetComponent<PlayerHealth>().TakeDamage(Damage);
        }
    }
}
