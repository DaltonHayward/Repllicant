using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minotaurTest : MonoBehaviour
{ 
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            animator.SetTrigger("basicAtk");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetTrigger("heavyAtk");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            animator.SetTrigger("comboAtk");
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            animator.SetTrigger("warcry");
        }
    }
}
