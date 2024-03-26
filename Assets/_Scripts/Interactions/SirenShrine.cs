using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SirenShrine : MonoBehaviour
{
    [SerializeField] int scene = 3;

    [SerializeField] GameObject gameManager;

    public void SirenCheck()
    {
        // TODO: check player inv for harpy feathers
        for (int i = 0; i < gameManager.GetComponent<InventoryController>().items.length(); i++)
        { 
            
        }

        SceneChange(scene);
    }

    private void SceneChange(int scene)
    {
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Switching scene to: scene " + scene);
        SceneManager.LoadScene(scene);
    }
}
