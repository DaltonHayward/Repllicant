using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolTipUI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
    
        [SerializeField] public ItemData item;
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("On Pointer Enter: ToolTip");
            TooltipSystem.ShowToolTip(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.HideToolTip();
        }
    }
}
