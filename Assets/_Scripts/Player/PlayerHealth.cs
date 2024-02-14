using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Player Max health
    public float CurrentHealth; // Player current health
    private float _invinsibleDuration;
    public Slider hpSlider;

    void Start()
    {
        CurrentHealth = maxHealth; // At start, full health

        if (_invinsibleDuration > 0)
        {
            _invinsibleDuration -= Time.deltaTime;
        }
        hpSlider.maxValue = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage; // Reduce HP when take damage
        Debug.Log("Player health is now " + CurrentHealth); // Print current HP
        hpSlider.value = CurrentHealth;

        if (CurrentHealth <= 0)
        {
            Die(); // Excute dealth logic when HP reach 0
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount; // Restore HP
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth); // Make sure that the current hp wont above MAX HP
        Debug.Log("Player healed, health is now " + CurrentHealth); // Print current HP
    }

    void Die()
    {
        Debug.Log("Player is dead!"); // Death logic
        // 这里可以添加重启游戏或者显示游戏结束界面的逻辑
        //You can add game over scene logic here
    }

    public void Invinsible(float delay, float invinsibleLength)
    {
        if (delay > 0)
        {
            StartCoroutine(StartInvinsible(delay, invinsibleLength));
        }
        else
        {
            _invinsibleDuration = invinsibleLength;
        }
    }

    IEnumerator StartInvinsible(float dly, float invsLength)
    {
        yield return new WaitForSeconds(dly);
        Debug.Log("Invinsible");
        _invinsibleDuration = invsLength;
    }
}
