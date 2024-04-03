using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquippedAxe : EquippedTool
{
    public void OnTriggerEnter(Collider other)
    {
        Collectible c = other.GetComponent<Collectible>();

        if (c != null)
        {
            if (c.GetType() == typeof(Wood))
            {
                
                float dmg;
                if (other.gameObject.GetComponent<Wood>().isStoned)
                {
                    dmg = Damage / 10;
                }
                else
                {
                    dmg = Damage;
                }

                c.TakeDamage(dmg);
                
                // send fire signal if burning
                ISubscriber subscriber = other.GetComponent<ISubscriber>();
                if(invTool.isShocked && subscriber != null){
                c.GetComponent<Wood>().ReceiveMessage("Shocked:"+Damage+","+3);
                }
                if (isBurning && subscriber != null)
                {
                    
                    subscriber.ReceiveMessage("Burning");
                }
                
            }

            if (c.GetType() == typeof(Stone))
            {
                if(invTool.isShocked){
                c.GetComponent<Stone>().ReceiveMessage("Shocked:"+Damage / 10+","+3);
                }
                else{
                    c.TakeDamage(Damage/10);
                }
            }
        }


        _animator.CrossFade("Blend Tree", 0.07f, 0);
        _playerController.StopCoroutine(_playerController.Reset);
        _playerController.SetState(PlayerController.State.STANDING);
    }
}
