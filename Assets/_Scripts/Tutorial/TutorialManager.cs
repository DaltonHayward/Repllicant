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
    [SerializeField] GameObject boat;
    [SerializeField] GameObject compass;
    // NPC bools
    public bool daltonNPC = false;
    public bool techNPC = false;
    public bool avNPC = false;
    public bool scientistNPC = false;
    public bool firstSpawn = false;
    public int tProgress = 0;

    // tutorial messages
    [SerializeField] GameObject tutorialMessages;

    public static TutorialManager instance;

    private void Awake()
    {
        // singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // updates tutorial progress
    public void UpdateTutorialProgress(int progress)
    {
        string message = "";
        tProgress = progress;
        switch (progress)
        {
            case 0:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Talk to the strange man";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                break;
            case 1:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Clear the area for Dalton's camp. Open your inventory by pressing 'TAB', then equip both tools by right-clicking on each of them and selecting equip. Use the scroll wheel to change tool/weapon";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                break;
            case 2:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Return to Dalton";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                // logic for changing dalton ink here
                tutorialDalton.GetComponent<TutorialDalton>().ChangeDialogue(1);
                break;
            case 3:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Craft a torch by opening the crafting menu in the top right. Select the torch from the menu and craft";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                break;
            case 4:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Talk to Dalton once more";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                // logic for changing dalton ink here
                tutorialDalton.GetComponent<TutorialDalton>().ChangeDialogue(2);
                break;
            case 5:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Find the boat and use it to reach the main island so you can rescue your other crewmates";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                boat.GetComponentInChildren<Interactor>().interactable = true;
                compass.SetActive(true);
                daltonNPC = true;
                tProgress = 999;
                break;
            default:
                Debug.Log("Changing to next tutorial step:" + progress);
                // Default case
                tutorialMessages.SetActive(false);
                break;
        }
    }

    // enables each camp
    public void NPCCheck()
    {
        // if the player has just started a new game, tp them to starting location
        if (!firstSpawn) 
        {
            firstSpawn = true;
            StartCoroutine(TeleportPlayer(2.5f));
        }
        if (daltonNPC) 
        { 
            daltonCamp.SetActive(true);
            boat.GetComponentInChildren<Interactor>().interactable = true;
            compass.SetActive(true);
        }
        // put player in front of tutorial dalton
        else 
        { 
            tutorialDalton.SetActive(true);
            if (tutorialMessages == null) 
            {
                Debug.Log("Reference to tutorial messages not set");
                return;
            }
            tutorialMessages.SetActive(true);
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
        tProgress = gameData.tProgress;
        StartCoroutine(NPCCheckCR(0.5f));
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.daltonNPC = daltonNPC;
        gameData.techNPC = techNPC;
        gameData.avNPC = avNPC;
        gameData.scientistNPC = scientistNPC;
        gameData.firstSpawn = firstSpawn;
        gameData.tProgress = tProgress;
    }

    IEnumerator NPCCheckCR(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NPCCheck();
        UpdateTutorialProgress(tProgress);
    }

    IEnumerator TeleportPlayer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("First spawn, teleporting player to starting position");
        GameObject.FindWithTag("Player").gameObject.transform.position = new Vector3(171.800003f, 0, -103.419998f);
        yield return new WaitForSeconds(1f);
        GameObject.FindWithTag("Player").gameObject.transform.position = new Vector3(171.800003f, 0, -103.419998f);
    }
}
