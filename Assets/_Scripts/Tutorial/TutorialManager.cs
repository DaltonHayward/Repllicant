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
            StartCoroutine(TeleportPlayer(2.5f));
        }
        if (daltonNPC) { daltonCamp.SetActive(true);}
        // put player in front of tutorial dalton
        else 
        { 
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
        firstSpawn = gameData.firstSpawn;
        NPCCheck();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.daltonNPC = daltonNPC;
        gameData.techNPC = techNPC;
        gameData.avNPC = avNPC;
        gameData.scientistNPC = scientistNPC;
        gameData.firstSpawn = firstSpawn;
    }

    IEnumerator TeleportPlayer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("First spawn, teleporting to starting position");
        GameObject.FindWithTag("Player").gameObject.transform.position = new Vector3(171.800003f, 0, -103.419998f);
        yield return new WaitForSeconds(1f);
        GameObject.FindWithTag("Player").gameObject.transform.position = new Vector3(171.800003f, 0, -103.419998f);
    }
}
