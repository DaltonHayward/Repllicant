using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinotaurShrine : MonoBehaviour
{
    [SerializeField] int scene = 3;

    [SerializeField] public GameObject craftingManager;

    public void MinotaurCheck()
    {
        // check player inventory for reagents
        InventoryInteraction inventory = craftingManager.GetComponent<InventoryInteraction>();

        var item1 = new ItemTypeAndCount("Wood", 1);
        var item2 = new ItemTypeAndCount("Axe", 1);

        ItemTypeAndCount[] items = { item1, item2 };
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
