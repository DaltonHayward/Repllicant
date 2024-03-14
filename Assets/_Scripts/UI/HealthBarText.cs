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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthSlider.value;
        maxHealth = healthSlider.maxValue;
        healthText.GetComponent<TextMeshProUGUI>().text = currentHealth + "";
    }

    public void HealthBarTextChanged(float newHealth)
    {
            currentHealth = newHealth;
            healthText.GetComponent<TextMeshProUGUI>().text = currentHealth + "";
    }
}
