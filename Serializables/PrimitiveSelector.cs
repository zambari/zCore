using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Z
{
#pragma warning disable 414 
    [System.Serializable]
    public class PrimitiveSelector
    {
       
        [SerializeField] GameObject addByDraggingHere;
        [SerializeField] GameObject removeByDroppingHere;
        [Header("Drag to edit list")]

        [ReadOnly] [SerializeField] int itemsCount;
        public List<GameObject> selectedGameObjects = new List<GameObject>();
        public void OnValidate(MonoBehaviour source)
        {
            // this.source = source;
            if (addByDraggingHere != null && selectedGameObjects != null)
            {
                if (selectedGameObjects.Contains(addByDraggingHere)) Debug.Log("Already has this, sorry");
                else
                {
                    selectedGameObjects.Add(addByDraggingHere);
                    Debug.Log("added " + addByDraggingHere.name, addByDraggingHere);
                }
            }
            addByDraggingHere = null;
            if (removeByDroppingHere != null && selectedGameObjects != null)
                if (selectedGameObjects.Contains(removeByDroppingHere)) selectedGameObjects.Remove(removeByDroppingHere); else Debug.Log("does not contain");
            removeByDroppingHere = null;
            for (int i = 0; i < selectedGameObjects.Count; i++)
            {
                if (selectedGameObjects[i] == null)
                {
                    selectedGameObjects.RemoveAt(i);
                    i--;
                }
            }
            itemsCount = selectedGameObjects.Count;
        }
    }
}