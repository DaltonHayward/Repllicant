using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class LightningBullet : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public float instanceTime;
    private bool isHit = false;
    private string signalMessage;
    private int signalDamage; 


    void Start()
    {
        instanceTime=1f;
    }

    void Update()
    {
        if(direction != null){
        if(!isHit){
            instanceTime -= Time.deltaTime;
            if(instanceTime <= 0){
                DestroyBulletwithoutParticle();
            }
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, direction, step);        
        }
        
    }
    private void OnCollisionStay(Collision other) {
        Debug.Log("Bullet collided with: "+other.gameObject.name);
        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet shocks enemy");
            // other.gameObject.GetComponent<ISubscriber>().ReceiveMessage(signalMessage);
            isHit = true;
            Destroy(gameObject);
        }
        
    }

    public void SetDirection(Vector3 dir, float damage, string Message){
        signalMessage=Message;
        damage = signalDamage;
        // Debug.Log("Id: "+gameObject.GetInstanceID());
        // Debug.Log("Set direction to: "+dir);
        // Debug.Log("Starting position is:"+gameObject.transform.position);
        direction = dir;
    }
    public void DestroyBulletwithoutParticle(){
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, 1);

    }
}
