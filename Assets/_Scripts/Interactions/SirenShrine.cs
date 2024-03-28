using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SirenShrine : MonoBehaviour
{
    [SerializeField] int scene = 3;

    [SerializeField] public GameObject gameManager;

    public void SirenCheck()
    {
        // check player inventory for reagents
        Dictionary<string, ItemData> inventory = gameManager.GetComponent<InventoryController>().itemDataDictionary;
        if (inventory.ContainsKey("CrystallizedEtherforce") && inventory.ContainsKey("ArcaneConductorMatrix"))
        {
            SceneChange(scene);
        }
    }

    private void SceneChange(int scene)
    {
        UIManager.instance.loadingScreen.gameObject.SetActive(true);
        UIManager.instance.loadingScreen.alpha = 1f;
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Switching scene to: scene " + scene);
        SceneManager.LoadScene(scene);
    }
}
