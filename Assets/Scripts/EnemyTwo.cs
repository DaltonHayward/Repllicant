using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    public Transform left;
    public Transform right;
    private Vector2 leftPostion;
    private Vector2 rightPosition;
    public bool isfaceleft = true;
    Rigidbody2D rb;
    public bool Death = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftPostion = left.position;
        rightPosition = right.position;

    }
    private void Update()
    {
        if (Death)
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }

        if (isfaceleft)
        {
            rb.velocity = new Vector2(-5, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(5, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (transform.position.x < leftPostion.x)
        {
            isfaceleft = false;
        }
        else if (transform.position.x > rightPosition.x)
        {
            isfaceleft = true;
        }



    }


}