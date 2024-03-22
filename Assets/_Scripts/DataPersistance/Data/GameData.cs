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
    public List<string> InvItems_Names;
    public List<int> InvItems_xCord;
    public List<int> InvItems_yCord;
    public List<bool> InvItems_Rotated;
    public List<bool> InvItems_Equipped;
    public List<string> StashItems_Names;
    public List<int> StashItems_xCord;
    public List<int> StashItems_yCord;
    public List<bool> StashItems_Rotated;


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
        
        this.InvItems_Names = new List<string>();
        this.InvItems_xCord = new List<int>();
        this.InvItems_yCord = new List<int>();
        this.InvItems_Rotated = new List<bool>();
        this.InvItems_Equipped = new List<bool>();

        this.StashItems_Names = new List<string>();
        this.StashItems_xCord = new List<int>();
        this.StashItems_yCord = new List<int>();
        this.StashItems_Rotated = new List<bool>();

    }
}
