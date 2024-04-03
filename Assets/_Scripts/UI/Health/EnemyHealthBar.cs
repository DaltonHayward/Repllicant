using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OWNER: Spencer   
/// health bar for enemies - not active when undamaged or hasn't take damage for 3 seconds
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    public Enemy enemy;
    public Slider healthSlider;
    public Slider easeSlider;
    public float maxHealth;
    public float health;

    private float lerpSpeed = 0.05f;

    float timeUntilHidden = 0f;

    private void Awake() 
    {

    }
    private void Start() 
    {
        maxHealth = enemy.hp;
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        easeSlider.maxValue = maxHealth;
    }

    private void Update() 
    {
        // billboard the health bar to always face camera
        transform.LookAt(transform.position + Camera.main.transform.forward);

        // update hidden value on damage
        if (health != enemy.hp)
        {
            timeUntilHidden = 3f; // how long until the health bar will be hidden again
        }
        // update health bar to match enemies health
        health = enemy.hp;


        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }


        if (healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, health, lerpSpeed);
        }

        // calculate time until the bar is hidden from player
        timeUntilHidden -= Time.deltaTime;

        if (timeUntilHidden <= 0)
        {
            timeUntilHidden = 0;
            healthSlider.gameObject.SetActive(false);
            easeSlider.gameObject.SetActive(false);
        }
        else
        {
            if (!healthSlider.gameObject.activeInHierarchy)
            {
                healthSlider.gameObject.SetActive(true);
                easeSlider.gameObject.SetActive(true);
            }
        }
        if (healthSlider.value <= 0)
        {
            Destroy(healthSlider.gameObject);
            Destroy(easeSlider.gameObject);
        }
    }
}
