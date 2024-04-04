using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SirenShrine : MonoBehaviour
{
    [SerializeField] int scene = 3;

    [SerializeField] public GameObject craftingManager;

    public void SirenCheck()
    {
        // check player inventory for reagents
        InventoryInteraction inventory = craftingManager.GetComponent<InventoryInteraction>();

        var item1 = new ItemTypeAndCount("ArcaneConductorMatrix", 1);
        var item2 = new ItemTypeAndCount("CrystallizedEtherforce", 1);
        var item3 = new ItemTypeAndCount("AerodynamicEssence", 1);
        Debug.Log("test");

        ItemTypeAndCount[] items = {item1, item2, item3};
        Debug.Log(inventory.ItemCheck(items));
        if (inventory.ItemCheck(items))
        {
            SceneChange(scene);
        }
        else
        {
            GetComponentInChildren<Interactor>().FailedCheck();
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
