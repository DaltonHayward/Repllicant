using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true); ;
            if (Input.GetKey("f"))
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
