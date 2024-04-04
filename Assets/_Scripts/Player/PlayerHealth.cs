using ReplicantPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(DamageIndicator))]

public class PlayerHealth : MonoBehaviour, IDataPersistance
{
    //[SerializeField] protected UnityEvent<PlayerHealth> OnPlayerDied = new UnityEvent<PlayerHealth>();
    public float maxHealth = 500f; // Player Max health
    public float currentHealth; // Player current health

    protected float previousHealth = 0f;
    

    public bool isDead;

    private float _invincibleDuration;
    //public GameObject slider;
    public GameObject deathScreen;

    // Coupled EffectableObject script here so that effects can be applied to the player health
    protected EffectableObject Effectable;

    // for red damage taken effect
    DamageIndicator damageIndicator;

    void Start()
    {
        damageIndicator = GetComponent<DamageIndicator>();
        //StartCoroutine(RefreshHPBar(0.5f));
        isDead = false;
        previousHealth = currentHealth;
    }

    private void Awake() 
    {
        // retrieve effects
        Effectable = GetComponent<EffectableObject>();
    }
    public void Update()
    {
        if (_invincibleDuration > 0)
        {
            _invincibleDuration -= Time.deltaTime;
        }
        currentHealth = Mathf.RoundToInt(Effectable.Effect_PlayerHealth(currentHealth));

        if  (previousHealth != currentHealth)
        {
            previousHealth = currentHealth;
        }
    }

    public void TakeDamage(float damage) 
    {
        if (_invincibleDuration <= 0 && !isDead) 
        {
            //damageIndicator.Hurt();
            //currentHealth -= damage; // Reduce HP when take damage
            currentHealth = Mathf.Max(currentHealth - damage, 0f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Damage/DamageVocal", GetComponent<Transform>().position);
            

            if (currentHealth <= 0f && previousHealth > 0f)
            {
                Die(); // Excute dealth logic when HP reach 0
            }
        }
    }

    public void Heal(float amount) // could add gameobject source here
    {
        //currentHealth += amount; // Restore HP
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Make sure that the current hp wont above MAX HP
        //slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
        Debug.Log("Player healed, health is now " + currentHealth); // Print current HP
    }

    void Die()
    {
        Debug.Log("Player is dead!"); // Death logic
        // 这里可以添加重启游戏或者显示游戏结束界面的逻辑 - (Here you can add logic to restart the game or display the game end interface)
        //You can add game over scene logic here
        //InventoryController.instance.PlayerDeath();
        isDead = true;
        deathScreen.SetActive(true);
        deathScreen.GetComponent<CanvasGroup>().alpha = 0;
        FadeIn(deathScreen.GetComponent<CanvasGroup>(), 0.5f);
        //deathScreen.GetComponent<CanvasFader>().Fade();
        deathScreen.GetComponent<CanvasGroup>().interactable = true;
        Heal(maxHealth);
        //OnPlayerDied.Invoke(this);
    }
    
    // Call this method to fade in the canvas group
    public void FadeIn(CanvasGroup canvasGroup, float fadeDuration)
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1f, fadeDuration));
    }

    // Coroutine to fade the canvas group
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        cg.alpha = endAlpha;
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

    /*
    IEnumerator RefreshHPBar(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
    }*/

    public void LoadData(GameData gameData)
    {
        this.currentHealth = gameData.currentHealth;
        this.maxHealth = gameData.maxHealth;
        //slider.GetComponent<HealthBarText>().ChangeHealthSlider(currentHealth, maxHealth);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.currentHealth = this.currentHealth;
        gameData.maxHealth = this.maxHealth;
    }
}
