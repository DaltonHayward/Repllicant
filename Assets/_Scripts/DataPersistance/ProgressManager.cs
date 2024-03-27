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
    [SerializeField] GameObject boat2;
    // bools
    public bool tech = false;
    public bool aviator = false;
    public bool scientist = false;
    public bool sirenDefeated = false;

    public bool isBossFight = false;

    public static ProgressManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one ProgressManager in the scene");
        }
        instance = this;
    }

    // enables 
    public void ProgressCheck()
    {
        if (techNPC != null) { techNPC.SetActive(!tech); }
        if (aviatorNPC != null) {  aviatorNPC.SetActive(!aviator); } 
        if (scientistNPC != null) { scientistNPC.SetActive(!scientist); }
        if (sirenDefeated && !isBossFight) 
        { 
            GameObject.FindWithTag("Player").transform.position = new Vector3(149.089996f, 0, 132.199997f); 
            if (boat2 != null) { boat2.SetActive(true); }
        }
        if (gate != null)
        {
            gate.GetComponentInChildren<Interactor>().interactable = sirenDefeated;
        }
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
