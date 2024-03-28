using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public int baseIsland = 2;
    public int mainMenu = 0;

    public void Respawn()
    {
        SceneChange(baseIsland);
    }

    public void MainMenu()
    {
        SceneChange(mainMenu);
    }

    public void Exit() 
    {
        Application.Quit();
    }

    private void SceneChange(int scene)
    {
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Switching scene to: scene " + scene);
        SceneManager.LoadScene(scene);
    }
}
