using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenGate()
    {
        Debug.Log("Player triggered gate to open.");
        anim.SetBool("PlayerEnter", true);
        GetComponentInChildren<Interactor>().gameObject.SetActive(false);
    }
}
