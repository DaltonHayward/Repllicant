using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IDataPersistance
{
    // the camps
    [SerializeField] GameObject daltonCamp;
    [SerializeField] GameObject tutorialDalton;
    [SerializeField] GameObject techCamp;
    [SerializeField] GameObject aviatorCamp;
    [SerializeField] GameObject scientistCamp;
    [SerializeField] GameObject tutorialObjects;
    // NPC bools
    public bool daltonNPC = false;
    public bool techNPC = false;
    public bool avNPC = false;
    public bool scientistNPC = false;
    public bool firstSpawn = false;

    // enables each camp
    public void NPCCheck()
    {
        if (!firstSpawn)
        {
            firstSpawn = true;
            GameObject.FindWithTag("Player").gameObject.transform.position = new Vector3(173.259995f, 0f, -98.6600037f);
        }
        if (daltonNPC) { daltonCamp.SetActive(true);}
        else 
        { 
            //GameObject.FindWithTag("Player").transform.position = new Vector3(186.630005f, 0, -103.230003f); 
            tutorialDalton.SetActive(true);
        }
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
