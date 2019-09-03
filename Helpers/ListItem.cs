using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

// simplified ver

//[ExecuteInEditMode]
namespace Z
{
    public class ListItem : MonoBehaviour//, IPointerClickHandler
    {
        public Text textLabel;
        public int id;
        public string label;
        protected ListPopulator _listPopulator;
        protected ListPopulator listPopulator { get { if (_listPopulator == null) _listPopulator = GetComponentInParent<ListPopulator>(); return _listPopulator; } }
        public void SetID(int i)
        {
            id = i;
        }
        public void SetLabel(string s)
        {
            label = s;
            name = /*(id == 0 ? "" : id + ".") + */" item " + label;
            if (textLabel != null)
                textLabel.text = s;
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
        protected void SelectController()
        {
            if (listPopulator == null) Debug.Log("sorry, this must be a rouge item", gameObject);
            else
                Selection.activeGameObject = listPopulator.gameObject;

        }
#endif
    }
}