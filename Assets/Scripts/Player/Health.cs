using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float Hp = 100;
    private float _invinsibleDuration;

    // Update is called once per frame
    void Update()
    {
        if (_invinsibleDuration > 0)
        {
            _invinsibleDuration -= Time.deltaTime;
        }
    }

    public void TakeDamage(float amount) 
    {
        if (_invinsibleDuration <= 0)
        {
            Hp -= amount;
            Debug.Log("Take Damage");
        }
    }

    public void Invinsible(float delay, float invinsibleLength)
    {
        if (delay > 0)
        {
            StartCoroutine(StartInvinsible(delay, invinsibleLength));
        }
        else 
        {
            _invinsibleDuration = invinsibleLength;
        }
    }

    IEnumerator StartInvinsible(float dly, float invsLength) 
    {
        yield return new WaitForSeconds(dly);
        Debug.Log("Invinsible");
        _invinsibleDuration = invsLength;
    }
}
