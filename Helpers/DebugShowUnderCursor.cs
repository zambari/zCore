using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugShowUnderCursor : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

     public List<RaycastResult> RaycastMouse(){
         
         PointerEventData pointerData = new PointerEventData (EventSystem.current)
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
        list= RaycastMouse();
        string objects="objs:\n";
        foreach ( RaycastResult result in list)
            objects+=result.gameObject.name+"\n";
        text.text = objects;
    }
}
