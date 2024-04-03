using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedPickaxe : EquippedTool
{
    public void OnTriggerEnter(Collider other)
    {
        Collectible c = other.GetComponent<Collectible>();

        if (c != null)
        {
            if (c.GetType() == typeof(Wood))
            {
                if(invTool.isShocked){
                c.GetComponent<Wood>().ReceiveMessage("Shocked:"+Damage/2+","+3);
                }
                else{
                    c.TakeDamage(Damage/2);
                }
            }

            if (c.GetType() == typeof(Stone))
            {
                if(invTool.isShocked){
                c.GetComponent<Stone>().ReceiveMessage("Shocked:"+Damage+","+3);
                }
                else{
                    c.TakeDamage(Damage);
                }
            }
        }

        _animator.CrossFade("Blend Tree", 0.07f, 0);
        _playerController.StopCoroutine(_playerController.Reset);
        _playerController.SetState(PlayerController.State.STANDING);
    }
}