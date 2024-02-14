using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
 
    public bool isAttacked=false;
    public Transform GroundCheck;
    public LayerMask ground;
    public bool isGround=true;
    [SerializeField]
    private bool isJump=false;
    Rigidbody2D rb2d;
    private Animator animator;
    private Vector2 movement;
    public float speed;
    public float jumpSpeed;
    public int jumpCount=1;
    private int remainJumpCount;

    private bool JumpPressed=false;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        remainJumpCount = jumpCount;
    }
    private void Update()
    {
        if (!isAttacked)
        {
              if(Input.GetKeyDown(KeyCode.Space)&&remainJumpCount>0)
                 {
            JumpPressed = true;
          
          
                }
        }
      
        ChangeAnime();
    }

    void ChangeAnime()
    {
  
        animator.SetFloat("running",Mathf.Abs(rb2d.velocity.x));
        if(isGround)
        {
            animator.SetBool("falling",false);
            animator.SetBool("isjumping", false);
        }
       else if(!isGround&& rb2d.velocity.y>0.01)
        {
            animator.SetBool("isjumping", true);
            animator.SetBool("falling", false);
        }
        else if(rb2d.velocity.y<0.01)
        {
            animator.SetBool("isjumping", false);
            animator.SetBool("falling", true);
        }
       


    }
    private void FixedUpdate()
    {
        //isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, ground);
        isGround = GetComponent<CircleCollider2D>().IsTouchingLayers(ground);
        if(!isAttacked)
        {
            MoveHorizontal();

        }
      Jump();
        
     
    }

    
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1,0,0);
    //    Gizmos.DrawWireSphere(GroundCheck.position, 0.2f);

    //}

    void Flip()
    {
        if(Input.GetAxisRaw("Horizontal")!=0)
        {
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"),1,1);
        }

       
    }
   void MoveHorizontal()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb2d.velocity.y);
        rb2d.velocity = movement;
        Flip();
    }
   void Jump()
    {   
      
        if(isGround)
        {
            //remainJumpCount = jumpCount;
            //isJump = false;
        }
        if(JumpPressed&&isGround)
        {
            GetComponent<PalyMisic>().PlayJump();
            isJump = true;
            rb2d.velocity=new Vector2(rb2d.velocity.x, jumpSpeed);
            remainJumpCount--;
            print("1");
            JumpPressed = false;
        }
        else if(JumpPressed&&remainJumpCount>0&&isJump)
        {
            GetComponent<PalyMisic>().PlayJump();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            remainJumpCount--;
            JumpPressed = false;
        }
    }

 

}
