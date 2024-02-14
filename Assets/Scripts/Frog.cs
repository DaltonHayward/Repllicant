using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [HideInInspector]
    public bool isdie = false;
    public float during = 1.5f;
    private float currnetTime = 0;
    private bool isleft=true ;
    public float JumpVelocity;
    public float HorizontalVelocity;
    private Rigidbody2D rb;
    public LayerMask targetLayer;
    public bool isGround;
    private bool isJump=false;
    private bool isFall =false;
    public Transform left;
    public Transform right;
    private Vector2 leftPosition;
    private Vector2 rightPosition;
    private Animator an;
    public bool Death = false;


    GameObject a;
    private void OnMouseDown()
    {
        if(a.activeSelf)
        {
            a.SetActive(false);
        }
        else
        {
            a.SetActive(true);
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftPosition = left.position;
        rightPosition = right.position;
        an = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        isGround = GetComponent<CircleCollider2D>().IsTouchingLayers(targetLayer);
        if(rb.velocity.y<-0.01f)
        {
            isFall = true;
            isJump = false;
        }
        Jump();
    }
    private void Update()
    {
        if(Death)
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }
        Flip();
        DirectionCheck();
        if(isGround)
        {
              currnetTime += Time.deltaTime;
        }
       
        if(currnetTime>=during)
        {
            currnetTime = 0;
            //执行时间到后得函数
            isJump = true;
            isGround = false;
        }
        if(isFall&&isGround)
        {
            isFall = false;
            rb.velocity = Vector2.zero;
        }
        ChangeAnim();
    }


    void Flip()
    {
        if(isGround)
        {
            if(isleft)
            {
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    void Jump()
    {
        if (isJump)
        { 
            if (isleft)
            {
                rb.velocity = new Vector2(-HorizontalVelocity, JumpVelocity);
            }
            else
            {
                rb.velocity = new Vector2(HorizontalVelocity, JumpVelocity);
            }
            isJump = false;
        }
    }
    void  DirectionCheck()
    {
        if(transform.position.x<leftPosition.x)
        {
            isleft = false;
        }
        else if(transform.position.x>rightPosition.x)
        {
            isleft=true;
        }
    }

    void ChangeAnim()
    {
        an.SetBool("isjumpup",isJump);
        an.SetBool("isfall",isFall);
    }
}
