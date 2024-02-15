using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Charmable : MonoBehaviour, ISubscriber
{
    private string emissionChannel = "Singing";

    [SerializeField] private int maxCharmedHP = 50;
    private int charmedHP;
    private bool isCharmed = false;
    private float ResetCooldown;
    private Color baseColor;
    public float charmedCooldown = 5;
    public Transform Siren;
    private PlayerController _playerController;

    private void Awake()
    {
        charmedHP = maxCharmedHP;
        baseColor = gameObject.transform.GetChild(1).GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    private void Update()
    {
        if (isCharmed && _playerController != null)
        {
            StartCoroutine(CharmedCooldown());
            _playerController.MoveTowardsTarget(Siren.position);

        }
        else
        {

        }

        if (Time.time - ResetCooldown > 3)
        {
            charmedHP = maxCharmedHP;
        }

        //Debug.Log(charmedHP);
    }

    IEnumerator CharmedCooldown()
    {
        yield return new WaitForSeconds(charmedCooldown);
        ResetCharm();
    }


    public void ReceiveMessage(string s)
    {
        // handles attack message
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
        _playerController = GetComponent<PlayerController>();

        if (_playerController != null)
        {
            _playerController.SetState(PlayerController.State.CHARMED);
        }
        else
        {

        }
    }

    public void ResetCharm()
    {
        isCharmed = false;
        charmedHP = maxCharmedHP;
        _playerController.SetState(PlayerController.State.STANDING);
    }

}
