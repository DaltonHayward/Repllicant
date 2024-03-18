using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTesting : MonoBehaviour, IDataPersistance
{
    public int interactedCount = 0;
    public bool onOff = false;

    public void LoadData(GameData gameData)
    {
        this.interactedCount = gameData.testInt;
        this.onOff = gameData.testBool;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.testInt = interactedCount;
        gameData.testBool = onOff;
    }

    public void IncreaseInteractedCount()
    {
        interactedCount++;
    }

    public void TurnOnOff()
    {
        onOff = !onOff;
    }
}
