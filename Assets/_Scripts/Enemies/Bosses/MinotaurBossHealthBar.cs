using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinotaurBossHealthBar : MonoBehaviour
{
    public GameObject siren;
    public Slider healthSlider;
    public Slider easeSlider;
    public GameObject text;
    public float maxHealth;
    public float health;

    private float lerpSpeed = 0.05f;


    private void Awake()
    {

    }
    private void Start()
    {
        maxHealth = siren.GetComponent<COW>().hp;
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        easeSlider.maxValue = maxHealth;
    }

    private void Update()
    {
        // billboard the health bar to always face camera
        transform.LookAt(transform.position + Camera.main.transform.forward);

        // update health bar to match enemies health
        health = siren.GetComponent<COW>().hp;


        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }


        if (healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, health, lerpSpeed);
        }

        if (healthSlider != null)
        {
            if (!healthSlider.gameObject.activeInHierarchy)
            {
                healthSlider.gameObject.SetActive(true);
                easeSlider.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
            }
            if (healthSlider.value <= 0)
            {
                Destroy(healthSlider.gameObject);
                Destroy(easeSlider.gameObject);
                Destroy(text.gameObject);
            }
        }

    }
}
