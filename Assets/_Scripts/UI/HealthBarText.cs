using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarText : MonoBehaviour
{
    [SerializeField] private GameObject healthText;
    [SerializeField] private Slider healthSlider;

    private float currentHealth;
    private float maxHealth;

    public void HealthBarTextChanged(float newHealth)
    {
        currentHealth = newHealth;
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthText.GetComponent<TextMeshProUGUI>().text = currentHealth + "";
    }

    public void ChangeHealthSlider(float currentHP, float maxHP)
    {
        currentHealth = currentHP;
        maxHealth = maxHP;
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthText.GetComponent<TextMeshProUGUI>().text = currentHealth + "";
    }
}
