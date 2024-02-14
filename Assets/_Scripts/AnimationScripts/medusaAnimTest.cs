using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class medusaAnimTest : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetTrigger("walk");
        }
    }
}
