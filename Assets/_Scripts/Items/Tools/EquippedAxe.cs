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
                c.TakeDamage(Damage);
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
