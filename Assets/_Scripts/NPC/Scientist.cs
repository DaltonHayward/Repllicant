using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField] ProgressManager progressManager;

    public void SendToBase()
    {
        progressManager.scientist = true;
    }
}
