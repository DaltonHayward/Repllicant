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
    // tutorial messages
    [SerializeField] GameObject tutorialMessages;
    // bools
    public bool tech = false;
    public bool aviator = false;
    public bool scientist = false;
    public bool sirenDefeated = false;


    public bool isBossFight = false;
    public int tProgress = 0;

    public static ProgressManager instance { get; private set; }

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
            case 10:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Find your three other crewmates.";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                break;
            case 11:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Bring the three reagents to your crewmates to obtain the necessary items to challenge the Siren.";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                break;
            case 12:
                Debug.Log("Changing to next tutorial step:" + progress);
                message = "Proceed through the gate to continue exploring...";
                tutorialMessages.GetComponent<TutorialMessages>().ChangeTutorialMessage(message);
                break;
            default:
                Debug.Log("Changing to next tutorial step:" + progress);
                // Default case
                tutorialMessages.SetActive(false);
                break;
        }
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
            if (tProgress < 12) { UpdateTutorialProgress(12); }
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
        tProgress = gameData.tProgress;
       
        StartCoroutine(Checks(1));
    }

    IEnumerator Checks(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ProgressCheck();
        UpdateTutorialProgress(tProgress);
    }

    public void SaveData(ref GameData gameData)
    {;
        gameData.techNPC = tech;
        gameData.avNPC = aviator;
        gameData.scientistNPC = scientist;
        gameData.sirenDefeated = sirenDefeated;
        gameData.tProgress = tProgress;
        
    }
}
