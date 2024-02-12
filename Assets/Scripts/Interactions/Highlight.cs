using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.yellow);
            Debug.Log("HOHo");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<Renderer>().material.SetColor("_OutlineColor", other.GetComponent<CraftingTable>().GetOriginalOutline());
        }
    }
}
