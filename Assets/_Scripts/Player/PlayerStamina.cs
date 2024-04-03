using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    //[SerializeField] protected UnityEvent<float, float> OnStaminaChanged = new UnityEvent<float, float> ();
    public float MaxStamina = 100f;
    public float StaminaCost_Rolling = 10f;
    public float StaminaCost_Running = 4f;
    public float StaminaRecoveryRate = 10f;
    public float StaminaRecoveryDelay = 0.5f;

    protected float PreviousStamina = 0f;
    protected float StaminaRecoveryDelayRemaining = 0f;

    //public bool CanCurrentlyJump => CanJump && CurrentStamina >= StaminaCost_Rolling;
    //public bool CanCurrentlyRun => CanRun && CurrentStamina > 0f;
    public float CurrentStamina { get; protected set; } = 0f;
    /*
    protected void UpdateStamina()
    {
        // if we're running consume stamina
            /*if (IsRunning)
            {
                ConsumeStamina(StaminaCost_Running * Time.deltaTime);
            }

        else if (CurrentStamina < MaxStamina) // if we're able to recover
        {
            if (StaminaRecoveryDelayRemaining > 0f)
                StaminaRecoveryDelayRemaining -= Time.deltaTime;

            if (StaminaRecoveryDelayRemaining <= 0f)
                CurrentStamina = Mathf.Min(CurrentStamina + StaminaRecoveryRate * Time.deltaTime,
                                           MaxStamina);
        }
    }
    protected void ConsumeStamina(float amount)
    {
        CurrentStamina = Mathf.Max(CurrentStamina - amount, 0f);
        StaminaRecoveryDelayRemaining = StaminaRecoveryDelay;
    }*/
}
