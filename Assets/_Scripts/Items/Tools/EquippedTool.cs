using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedTool : MonoBehaviour
{
    public float BaseDamage;
    public float BurningDamage;
    [HideInInspector]
    public float Damage;
    [SerializeField]
    private ParticleSystem fireSystem;
    public InvTool invTool;

    protected Animator _animator;
    protected PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Damage = BaseDamage;
    }

    public void Update()
    {
        if (invTool.isBurning)
        {
            // set dmg
            Damage = BurningDamage;
            if (!fireSystem.isPlaying)
            {
                fireSystem.Play();
            }
        }

        else if (!invTool.isBurning)
        {
            // reset dmg
            Damage = BaseDamage;
            if (fireSystem.isPlaying)
            {
                fireSystem.Stop();
            }
        }
        if (invTool.isShocked)
        {
            Damage = BaseDamage;
        }
        else if (!invTool.isShocked)
        {
            Damage = BaseDamage;
        }
    }

    public void SetInvTool(InvTool tool)
    {
        invTool = tool;
    }

    #region - Colliders -
    public void BeginCollision()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void EndCollision()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }
    #endregion

    #region - Trail -
    public void BeginTrail()
    {
        GetComponentInChildren<TrailRenderer>().enabled = true;
    }

    public void EndTrail()
    {
        GetComponentInChildren<TrailRenderer>().enabled = false;
    }
    #endregion

}
