using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquippedSword : EquippedTool
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit weapon");
            ISubscriber subscriber = other.GetComponent<ISubscriber>();
            if (subscriber != null)
            {
                if (invTool.isShocked)
                {
                    var Jumps = 3;
                    Debug.Log("Sword Sending Shocked Message"+Damage);
                    subscriber.ReceiveMessage("Shocked:" + Damage + "," + Jumps);
                }
                else{
                    Debug.Log("Sword Sending Attacked Message"+Damage);
                subscriber.ReceiveMessage("Attacked:" + Damage);
                }
            }
        }
    }
}
