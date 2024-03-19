using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aviator : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;

    public void SendToBase()
    {
        progressManager.aviator = true;
    }
}
