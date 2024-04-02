using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ToolTipUI
{
    public class TooltipSystem : MonoBehaviour
    {

        [SerializeField] private GameObject popupCanvasObject;
        [SerializeField] private RectTransform popupObject;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float padding;

        private Canvas popupCanvas;
        private static TooltipSystem current;



        private void Awake()
        {
            current = this;
            popupCanvas = popupCanvasObject.GetComponent<Canvas>();
            popupCanvasObject.SetActive(false);

        }

        private void Update()
        {
            FollowCursor();
        }

        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector3 newPos = Input.mousePosition + offset;
            newPos.z = 0f;
            float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
            if (rightEdgeToScreenEdgeDistance < 0)
            {
                newPos.x += rightEdgeToScreenEdgeDistance;
            }
            float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
            if (leftEdgeToScreenEdgeDistance > 0)
            {
                newPos.x += leftEdgeToScreenEdgeDistance;
            }
            float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
            if (topEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += topEdgeToScreenEdgeDistance;
            }
            popupObject.transform.position = newPos;
        }

        public static void ShowToolTip(ItemData item) 
        {
            current.popupCanvasObject.SetActive(true);
            //current.GetComponent<CanvasFader>().Fade();
            current.header.transform.GetComponent<TextMeshProUGUI>().text = item.name;
            LayoutRebuilder.ForceRebuildLayoutImmediate(current.popupObject);
        }

        public static void HideToolTip()
        {
            
            current.popupCanvasObject.SetActive(false);
            //current.GetComponent<CanvasFader>().Fade();
        }

    }
}
