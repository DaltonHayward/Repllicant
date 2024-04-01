using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OWNER: Spencer   
/// simple alternative to our current health bar - looks a bit nicer imo
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    public Slider healthSlider;

    public Slider easeSlider;
    //public float maxHealth = 100f;
    public float health;

    private float lerpSpeed = 0.05f;

    private void Start() 
    {
        //health = playerHealth.currentHealth;
    }

    private void Update() 
    {
        health = playerHealth.currentHealth;

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }


        if (healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, health, lerpSpeed);
        }
    }

    
}
