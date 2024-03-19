using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChecker : MonoBehaviour, IDataPersistance
{
    [SerializeField] TutorialManager manager;
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
        if (this.transform.childCount == 0 )
        {
            manager.daltonNPC = true;
            Debug.Log("All obstacles are gone");
            StartCoroutine(Deactivate(2));
        }
    }

    IEnumerator Deactivate(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
