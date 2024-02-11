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
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            animator.SetTrigger("basicAtk");
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetTrigger("heavyAtk");
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetTrigger("comboAtk");
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetTrigger("warcry");
        }
    }
}
