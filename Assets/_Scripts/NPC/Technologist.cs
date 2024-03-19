using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Technologist : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;

    public void SendToBase()
    {
        progressManager.tech = true;
    }
}
