using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChecker : MonoBehaviour, IDataPersistance
{
    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(ref GameData gameData)
    {
        if (this.transform.childCount == 0) { gameData.daltonNPC = true; }
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.childCount == 0 )
        {
            Debug.Log("All obstacles are gone");
            gameObject.SetActive(false);
        }
    }
}
