using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SirenBossKill : MonoBehaviour
{
    [SerializeField] int scene = 0;

    public void SirenDeath()
    {
        ProgressManager.instance.sirenDefeated  = true;
        SceneChange(scene);
    }

    private void SceneChange(int scene)
    {
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Switching scene to: scene " + scene);
        SceneManager.LoadScene(scene);
    }
}
