using System.Collections;
using System.Collections.Generic;
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
                if (isBurning && subscriber != null)
                {
                    subscriber.ReceiveMessage("Burning");
                }
            }

            if (c.GetType() == typeof(Stone))
            {
                c.TakeDamage(Damage / 10);
            }
        }


        _animator.CrossFade("Blend Tree", 0.07f, 0);
        _playerController.StopCoroutine(_playerController.Reset);
        _playerController.SetState(PlayerController.State.STANDING);
    }
}
