using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtr : MonoBehaviour
{

    public float JumpPower;
    public float speed;
    Rigidbody2D rb2;

    Animator an;


    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();      
        AnimChange();

    }


    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb2.velocity = new Vector2(rb2.velocity.x,JumpPower);
        }
    }
    void Move()
    {
        rb2.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb2.velocity.y);
        Flip();
    }
    void Flip()
    {
        if(Input.GetAxisRaw("Horizontal")==1)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if(Input.GetAxisRaw("Horizontal") == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
    void AnimChange()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //Íæ¼ÒÔÚÅÜ
            an.SetBool("isRunning", true);
        }
        else
        {
            //Íæ¼Ò¾²Ö¹
            an.SetBool("isRunning", false);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            an.SetBool("isJumping", true);
        }

        an.SetFloat("VelocityY", rb2.velocity.y);


    }
}
