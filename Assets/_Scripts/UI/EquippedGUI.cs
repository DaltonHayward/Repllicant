using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquippedGUI : MonoBehaviour
{

    [SerializeField] public GameObject equippedSprite;

    [SerializeField] public Transform equippedPanel;

    [SerializeField] public PlayerController playerController;

    private string equipped;

    private enum Equipment { WEAPON, PICKAXE, AXE };

    // Start is called before the first frame update
    void Start()
    {
        equipped = playerController.GetCurrentEquipment();
        UpdateEquippedUI();

    }

    // Update is called once per frame
    void Update()
    {
        string newEquipped = playerController.GetCurrentEquipment();
        
        if (newEquipped != equipped)
        {
            equipped = newEquipped;
            UpdateEquippedUI();
        }
    }

    public void UpdateEquippedUI()
    {
        foreach (Transform child in equippedPanel)
        {
            Destroy(child.gameObject);
        }
        //Debug.Log(playerController._tools[playerController._currentTool] != null);
        int currentTool = playerController.GetCurrentTool();
        EquippedTool[] tools = playerController.GetTools();
        if (equipped != null && tools[currentTool] != null)
        {
            GameObject newEquipped = Instantiate(equippedSprite, equippedPanel);
            if (equipped == "WEAPON")
            {
                ItemData item;
                if (!InventoryController.instance.itemDataDictionary.TryGetValue("Sword", out item))
                {
                    return;
                }
                newEquipped.transform.GetComponent<Image>().sprite = item.sprites[0].sprite;
            }
            if (equipped == "PICKAXE")
            {
                ItemData item;
                if (!InventoryController.instance.itemDataDictionary.TryGetValue("Pickaxe", out item))
                {
                    return;
                }
                newEquipped.transform.GetComponent<Image>().sprite = item.sprites[0].sprite;
            }
            if (equipped == "AXE")
            {
                ItemData item;
                if (!InventoryController.instance.itemDataDictionary.TryGetValue("Axe", out item))
                {
                    return;
                }
                newEquipped.transform.GetComponent<Image>().sprite = item.sprites[0].sprite;
            }
            else
            {
                return;
            }
            

        }
    }
}
