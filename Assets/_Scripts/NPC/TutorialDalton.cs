using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDalton : MonoBehaviour
{
    // holds the Ink dialogue files
    [Header("Ink JSON's")]
    [SerializeField] public TextAsset[] inkJSON;

    public void ChangeDialogue(int dialogue)
    {
        GetComponentInChildren<DialogueTrigger>().inkJSON = inkJSON[dialogue];
    }
}
