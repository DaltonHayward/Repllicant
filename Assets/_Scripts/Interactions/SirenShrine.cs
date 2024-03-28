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

        var item1 = new ItemTypeAndCount("Arcane Conductor Matrix", 1);
        var item2 = new ItemTypeAndCount("Crystallized Etherforce", 1);

        ItemTypeAndCount[] items = {item1, item2};
        Debug.Log(inventory.ItemCheck(items));
        if (inventory.ItemCheck(items))
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
