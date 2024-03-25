using ReplicantPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDataPersistance
{
    public float maxHealth = 100f; // Player Max health
    public float currentHealth; // Player current health

    public bool isDead;

    private float _invincibleDuration;
    public GameObject slider;
    public GameObject deathScreen;

    void Start()
    {
        StartCoroutine(RefreshHPBar(0.5f));
        isDead = false;
    }

    public void Update()
    {
        if (_invincibleDuration > 0)
        {
            _invincibleDuration -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_invincibleDuration <= 0 && !isDead) 
        {
            currentHealth -= damage; // Reduce HP when take damage
            Debug.Log("Player health is now " + currentHealth); // Print current HP
            slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die(); // Excute dealth logic when HP reach 0
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount; // Restore HP
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Make sure that the current hp wont above MAX HP
        slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
        Debug.Log("Player healed, health is now " + currentHealth); // Print current HP
    }

    void Die()
    {
        Debug.Log("Player is dead!"); // Death logic
        // 这里可以添加重启游戏或者显示游戏结束界面的逻辑
        //You can add game over scene logic here
        InventoryController.instance.PlayerDeath();
        isDead = true;
        deathScreen.SetActive(true);
        deathScreen.GetComponent<CanvasGroup>().interactable = true;
        Heal(maxHealth);
    }

    public void Invincible(float delay, float invincibleLength)
    {
        if (delay > 0)
        {
            StartCoroutine(StartInvincible(delay, invincibleLength));
        }
        else
        {
            _invincibleDuration = invincibleLength;
        }
    }

    public bool isInvincible()
    {
        return _invincibleDuration > 0;
    }

    IEnumerator StartInvincible(float dly, float invsLength)
    {
        yield return new WaitForSeconds(dly);
        Debug.Log("Invincible");
        _invincibleDuration = invsLength;
    }

    IEnumerator RefreshHPBar(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
    }

    public void LoadData(GameData gameData)
    {
        this.currentHealth = gameData.currentHealth;
        this.maxHealth = gameData.maxHealth;
        slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.currentHealth = this.currentHealth;
        gameData.maxHealth = this.maxHealth;
    }
}
