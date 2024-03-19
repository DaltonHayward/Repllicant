using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IDataPersistance
{
    // the camps
    [SerializeField] GameObject daltonCamp;
    [SerializeField] GameObject techCamp;
    [SerializeField] GameObject aviatorCamp;
    [SerializeField] GameObject scientistCamp;
    [SerializeField] GameObject tutorialObjects;
    // NPC bools
    public bool daltonNPC = false;
    public bool techNPC = false;
    public bool avNPC = false;
    public bool scientistNPC = false;

    // enables each camp
    public void NPCCheck()
    {
        if (daltonNPC) { daltonCamp.SetActive(true); }
        if (daltonNPC) { tutorialObjects.SetActive(false); }
        if (techNPC) { techCamp.SetActive(true); }
        if (avNPC) {  aviatorCamp.SetActive(true); }
        if (scientistNPC) {  scientistCamp.SetActive(true);}
    }
    public void LoadData(GameData gameData)
    {
        daltonNPC = gameData.daltonNPC;
        techNPC = gameData.techNPC;
        avNPC = gameData.avNPC;
        scientistNPC= gameData.scientistNPC;
        NPCCheck();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.daltonNPC = daltonNPC;
        gameData.techNPC = techNPC;
        gameData.avNPC = avNPC;
        gameData.scientistNPC = scientistNPC;
    }
}
