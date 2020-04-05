using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace zUI
{

    public class DebugShowUnderCursor : MonoBehaviour
    {
        Text text;
        void Start()
        {
            text = GetComponent<Text>();
        }
        void Reset()
        {
            text = GetComponent<Text>();
            text.text = "EventSystem.current.RaycastAl()\n\n\n";
            text.raycastTarget = false;
            text.color = Color.white;
            var cnt = text.gameObject.AddOrGetComponent<ContentSizeFitter>();
            cnt.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            cnt.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        public List<RaycastResult> RaycastMouse()
        {

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return results;
        }
        List<RaycastResult> list;
        void Update()
        {
            list = RaycastMouse();
            string objects = "objs:\n";
            foreach (RaycastResult result in list)
                objects += result.gameObject.name + "\n";
            text.text = objects;
        }
    }

}