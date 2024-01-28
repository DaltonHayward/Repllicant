using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNotification : MonoBehaviour
{
    public GameObject notifText;
    public GameObject craftingMenu;

    public void NotifyPlayer()
    {
        notifText.SetActive(true);
    }

    public void DenotifyPlayer()
    {
        notifText.SetActive(false);
        craftingMenu.SetActive(false);

    }
}
