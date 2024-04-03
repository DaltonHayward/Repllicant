using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialMessages : MonoBehaviour
{ 
    public void ChangeTutorialMessage(string message)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }
}
