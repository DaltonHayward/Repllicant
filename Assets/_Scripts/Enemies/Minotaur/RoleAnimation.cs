using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// move
    /// </summary>
    /// <param name="speed"></param> 0 idle, 0.5 walk, 1 run
    public void PlayMove(float speed)
    {
        animator.SetFloat("move", speed);
    }

    // normal
    public void PlaySkill1()
    {
        animator.SetTrigger("skill1");
    }

    // Shock wave
    public void PlaySkill2()
    {
        animator.SetTrigger("skill2");
    }

    // chase
    public void PlaySkill3()
    {
        animator.SetTrigger("skill3");
    }

    public void PlayJump()
    {
        animator.SetTrigger("jump");
    }

    public void PlayTakeDamage()
    {
        animator.SetTrigger("hurt");
    }
}
