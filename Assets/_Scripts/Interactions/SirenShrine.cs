using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SirenShrine : MonoBehaviour
{
    [SerializeField] int scene = 3;

    public void SirenCheck()
    {
        // TODO: check player inv for harpy feathers
        SceneChange(scene);
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
