using ReplicantPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DamageIndicator))]

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    public float maxHealth = 250f; // Boss Max health
    public float currentHealth; // Boss current health

    public GameObject slider;
    public GameObject boss;


    void Start()
    {
        StartCoroutine(RefreshHPBar(0.5f));
    }

    public void Update()
    {
        if (boss.GetComponent<Siren>() != null)
        {
            currentHealth = Mathf.RoundToInt(boss.GetComponent<Siren>().hp);
            slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
        }
        
        if (boss.GetComponent<COW>() != null)
        {
            currentHealth = Mathf.RoundToInt(boss.GetComponent<COW>().hp);
            slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
        }
    }

    IEnumerator RefreshHPBar(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
    }
}
