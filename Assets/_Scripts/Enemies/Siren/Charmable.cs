using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Charmable : MonoBehaviour, ISubscriber
{
    private string emissionChannel = "Singing";

    private int maxCharmedHP = 50;
    private int charmedHP;
    private bool isCharmed = false;
    private float ResetCooldown;
    private Color baseColor;

    private void Awake()
    {
        charmedHP = maxCharmedHP;
        baseColor = gameObject.transform.GetChild(1).GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    private void Update()
    {
        if (Time.time - ResetCooldown > 3)
        {
            charmedHP = maxCharmedHP;
        }

        //Debug.Log(charmedHP);
    }

    public void ReceiveMessage(string s)
    {
        if (s.Equals(emissionChannel))
        {
            ResetCooldown = Time.time;
            TickCharm(1);
        }
    }

    private void TickCharm(int d)
    {
        if (!isCharmed)
        {
            charmedHP = Mathf.Clamp(charmedHP - d, 0, 100);
            if (charmedHP == 0)
                Charm();
            else
            {
                ChangeColor(Color.red);
                StartCoroutine(ResetColor());
            }
        }
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeColor(baseColor);
    }

    private void ChangeColor(Color color)
    {
        gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_BaseColor", color);
    }

    private void Charm()
    {
        isCharmed = true;
    }

}
