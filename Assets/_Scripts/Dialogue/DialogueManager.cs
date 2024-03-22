using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// OWNER: Spencer Martin
/// Manager class for dialogue system
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Paramas")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    public static DialogueManager instance;

    private Coroutine displayLineCoroutine;

    private bool canContinueToNextLine = false;

    //private PlayerController _playerController;

    private InkExternalFunctions inkExternalFunctions;

    private DialogueVariables dialogueVariables;

    private InventoryInteraction inventoryInteraction;

    private String _currentLine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
            Destroy(this);
        }

        inkExternalFunctions = new InkExternalFunctions();
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        // handle continuing to next line in the dialogue when submit is pressed
        if (canContinueToNextLine
            && currentStory.currentChoices.Count == 0
            && InputManager.instance.InteractInput)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
      
        //dialogueVariables.StartListening(currentStory);
        inkExternalFunctions.Bind(currentStory);
       

        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        StopCoroutine(displayLineCoroutine);

        //dialogueVariables.StopListening(currentStory);
        inkExternalFunctions.Unbind(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (GameObject.FindWithTag("Player").GetComponent<PlayerController>()._playerState == PlayerController.State.DIALOG)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetState(PlayerController.State.STANDING);
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        _currentLine = line;
        // empty the dialogue text
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        // hide items while text is typing
        continueText.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // if the submit button is pressed, finish up displaying the line right away
            //if (InputManager.instance.InteractInput) 
            //{
             //   dialogueText.maxVisibleCharacters = line.Length;
              //  break;
           // }

            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;

                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }

            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }

        }

        // actions to take after the entire line has finished displaying
        continueText.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive programming
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }


    private IEnumerator SelectFirstChoice()
    {
        // Event system fuckery
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }


    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }


    }

    public Ink.Runtime.Object GetVariableState(string variableName) 
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null) 
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    // This method will get called anytime the application exits.
    // Depending on your game, you may want to save variable state in other places.
    public void OnApplicationQuit() 
    {
        dialogueVariables.SaveVariables();
    }
}
