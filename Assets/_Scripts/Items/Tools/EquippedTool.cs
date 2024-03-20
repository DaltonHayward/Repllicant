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
    private InvTool invTool;
    private bool isBurning;
    private IEnumerator BurnTimerCoroutine;

    protected Animator _animator;
    protected PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        BurnTimerCoroutine = BurnTimer();
        Damage = BaseDamage;
    }

    public void Update()
    {
        if (invTool.isBurning && !isBurning)
        {
            StartCoroutine(BurnTimerCoroutine);
        }

        if (!invTool.isBurning && isBurning)
        {
            StopCoroutine(BurnTimerCoroutine);
            isBurning = false;
            fireSystem.Stop();
            // reset dmg
            Damage = BaseDamage;
        }
    }

    IEnumerator BurnTimer()
    {
        isBurning = true;
        fireSystem.Play();
        // Add dmg
        Damage = BurningDamage;

        yield return new WaitForSeconds(invTool.burnTime);

        isBurning = false;
        fireSystem.Stop();
        // reset dmg
        Damage = BaseDamage;
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
