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
using Z;
public class ListItem : MonoBehaviour//, IPointerClickHandler
{
    public Text textLabel;
    public int id;
    public string label;
    protected ListPopulator _listPopulator;
    protected ListPopulator listPopulator { get { if (_listPopulator == null) _listPopulator = GetComponentInParent<ListPopulator>(); return _listPopulator; } }
    public Button button;
    public void SetID(int i)
    {
        id = i;
    }
    void Reset()
    {
        textLabel = GetComponentInChildren<Text>();
    }
    protected virtual void OnValidate()
    {
        if (textLabel == null) textLabel = GetComponentInChildren<Text>();
        if (button == null) button = GetComponentInChildren<Button>();

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