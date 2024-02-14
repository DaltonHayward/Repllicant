using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{

    Animator animator;
    [SerializeField] float moveSpeed = 5.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveTo = new Vector3 (horizontalInput, 0, verticalInput);
        moveTo.Normalize();

        transform.Translate (moveTo * moveSpeed * Time.deltaTime, Space.World);

        if (moveTo != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            transform.forward = moveTo;
        }
        if (moveTo == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
        }
    }
}
