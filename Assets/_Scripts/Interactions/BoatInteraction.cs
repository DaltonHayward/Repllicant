using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatInteraction : MonoBehaviour
{
    [SerializeField] public Interactor interactor;
    [SerializeField] public DataPersistanceManager dataPersistanceManager;
    public int scene = 2;

    public void BoatUsed()
    {
        Debug.Log("Boat was used");
        SceneChange(scene);
    }

    private void SceneChange(int scene) 
    {
        dataPersistanceManager.SaveGame();
        Debug.Log("Switching scene to: scene " + scene);
        SceneManager.LoadScene(scene);
    }
}
