using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // values to save are listed here
    public int testInt;
    public bool testBool;
    public float currentHealth;
    public float maxHealth;
    // npcs
    public bool daltonNPC;
    public bool techNPC;
    public bool avNPC;
    public bool scientistNPC;
    // Bosses
    public bool sirenDefeated;

    // the values defined in this constructor will be the default values when no data to load
    public GameData()
    {
        // default values
        this.testInt = 0;
        this.testBool = false;
        this.currentHealth = 500;
        this.maxHealth = 500;
        this.daltonNPC = false;
        this.techNPC = false;
        this.avNPC = false;
        this.scientistNPC = false;
        this.sirenDefeated = false;
    }
}
