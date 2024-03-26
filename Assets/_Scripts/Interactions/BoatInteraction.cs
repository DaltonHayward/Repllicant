using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatInteraction : MonoBehaviour
{
    [SerializeField] public Interactor interactor;
    public int scene = 2;

    public void BoatUsed()
    {
        Debug.Log("Boat was used");
        SceneChange(scene);
    }

    private void SceneChange(int scene) 
    {
        UIManager.instance.loadingScreen.alpha = 1f;
        UIManager.instance.FadeAccent();
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Switching scene to: scene " + scene);
        SceneManager.LoadScene(scene);
    }
}
