using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 5f;
    void Start()
    {
        
    }
    void Update()
    {
        if (transform != null)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w"))
            {transform.position += new Vector3(0, 0, 1) * Time.deltaTime * movementSpeed * 2.5f;}

            else if (Input.GetKey("w") && !Input.GetKey(KeyCode.LeftShift))
            {transform.position += new Vector3(0, 0, 1) * Time.deltaTime * movementSpeed;}

            else if (Input.GetKey("s"))
            {transform.position -= new Vector3(0, 0, 1) * Time.deltaTime * movementSpeed;}

            if (Input.GetKey("a") && !Input.GetKey("d"))
            {transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * movementSpeed;}

            else if (Input.GetKey("d") && !Input.GetKey("a"))
            {transform.position -= new Vector3(-1, 0, 0) * Time.deltaTime * movementSpeed;}
        }
    }
}
