using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropdownInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryController.instance.DropdownHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryController.instance.DropdownHovered = false;
    }
}
