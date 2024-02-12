using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : MonoBehaviour, ISubscriber
{
    private Color _originalOutlineColor;

    public void Awake()
    {
        _originalOutlineColor = GetComponent<Renderer>().material.GetColor("_BaseColor");
    }
    public void ReceiveMessage(string channel)
    {
        if (channel.Equals("Attacked"))
        {
            GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
            StartCoroutine(Timeout(0.1f));
        }
    }

    IEnumerator Timeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<Renderer>().material.SetColor("_BaseColor", _originalOutlineColor);
    }

}
