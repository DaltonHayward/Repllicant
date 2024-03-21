using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour, IDataPersistance
{
    // references
    [SerializeField] GameObject techNPC;
    [SerializeField] GameObject aviatorNPC;
    [SerializeField] GameObject scientistNPC;
    [SerializeField] GameObject gate;
    // bools
    public bool tech = false;
    public bool aviator = false;
    public bool scientist = false;
    public bool sirenDefeated = false;
    
    // enables 
    public void ProgressCheck()
    {
        techNPC.SetActive(!tech); 
        aviatorNPC.SetActive(!aviator); 
        scientistNPC.SetActive(!scientist);
        gate.GetComponentInChildren<Interactor>().interactable = sirenDefeated; 
    }

    public void LoadData(GameData gameData)
    {
        tech = gameData.techNPC;
        aviator = gameData.avNPC;
        scientist = gameData.scientistNPC;
        sirenDefeated = gameData.sirenDefeated;
       
        ProgressCheck();
    }

    public void SaveData(ref GameData gameData)
    {;
        gameData.techNPC = tech;
        gameData.avNPC = aviator;
        gameData.scientistNPC = scientist;
        gameData.sirenDefeated = sirenDefeated;
        
    }
}
