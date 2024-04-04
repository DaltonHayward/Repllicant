using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialMessages : MonoBehaviour
{
    public static TutorialMessages instance;

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
    public void ChangeTutorialMessage(string message)
    {
        Debug.Log(message);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }
}
