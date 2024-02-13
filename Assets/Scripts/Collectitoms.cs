using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Collectitoms : MonoBehaviour
{
    public float attackTime = 0.3f;
    public int cherryCount = 0;
    public Text cherry;
    float cur = 0;
    private void Start()
    {
        UPdateUI();
    }
    void UPdateUI()
    {
        cherry.text = cherryCount.ToString();
    }
    private void Update()
    {
        if (GetComponent<Move>().isAttacked == true)
        {
            Timer(attackTime);
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("cherry"))
        {
            //收集处理逻辑
            cherryCount++;
            UPdateUI();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("top"))
        {
            GetComponent<PalyMisic>().PlayHit();
            collision.transform.parent.gameObject.GetComponent<Animator>().SetBool("death", true);
            if (collision.transform.parent.gameObject.GetComponent<EnemyTwo>())
            {
                collision.transform.parent.gameObject.GetComponent<EnemyTwo>().Death = true;
            }
            else if (collision.transform.parent.gameObject.GetComponent<Frog>())
            {
                collision.transform.parent.gameObject.GetComponent<Frog>().Death = true;
            }
            collision.transform.parent.gameObject.GetComponent<Collider2D>().enabled = false;

        }
        else if (collision.CompareTag("exit"))
        {
            Gamemanager.jumpScene("2");
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            if(collision.transform.position.x>transform.position.x)
            {
                GetComponent<Move>().isAttacked = true;

                GetComponent<Rigidbody2D>().velocity = new Vector2(-6,0);
            }
            else
            {
                GetComponent<Move>().isAttacked = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(6, 0);
            }

            //GetComponent<PlayerHealth>().SubHealth();
        }
    }
   public void Timer(float time)
    {
        
        cur += Time.deltaTime;
        if(cur>time)
        {
            GetComponent<Move>().isAttacked = false;
            cur = 0;
        }
    }


}
