using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Enemy enemy;
    public Slider healthSlider;
    public Slider easeSlider;
    public float maxHealth;
    public float health;

    private float lerpSpeed = 0.05f;

    private void Start() 
    {
        maxHealth = enemy.hp;
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        easeSlider.maxValue = maxHealth;
    }

    private void Update() 
    {
        health = enemy.hp;

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
