using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor.Rendering;

public class StatusNotif : MonoBehaviour
{

    [SerializeField] public GameObject statusText;

    [SerializeField] public GameObject statusSprite;

    [SerializeField] public Transform statusPanel;

    [SerializeField] public PlayerController playerController;

    private EffectableObject effectable;
    public StatusNotif instance;

    //List<GameObject> currentEffects = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        effectable = playerController.Effectable;
    }

    private void Update()
    {
        if (effectable.GetBaseEffects() != null)
        {
            UpdateStatusUI();
        }
    }

    // update status notifier under health bar
    public void UpdateStatusUI()
    {
        foreach (Transform child in statusPanel)
        {
            Destroy(child.gameObject);
        }

        //currentEffects.Clear();

        if (effectable != null)
        {
            List<BaseEffect> activeEffects = effectable.GetBaseEffects();


            for (int i = 0; i < activeEffects.Count; i++)
            {
                GameObject newStatus = Instantiate(statusSprite, statusPanel); //statusText previously
                //newStatus.name = activeEffects[i].name;
                //currentEffects.Add(newStatus);
                //newStatus.transform.GetComponent<TextMeshProUGUI>().text = activeEffects[i].name;
                newStatus.transform.GetComponent<Image>().sprite = activeEffects[i].sprite;
            }
        }

    }


}
