using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.Extras;
#if UNITY_EDITOR
using UnityEditor;
#endif


// can be used to provide drag and drop gameobject field, verifyng that tehre is an interface listener present (specified by the generic argument).
// really only useful for interfaces as monobehaviours dont need this

// v.02 valuesource-> target




/// <summary>
/// 
/// To override for your type
/// 
/*
/// 
/// 

// class
[System.Serializable]
public class MyTypeSelector:GenericSelector<T>

//editor

[CustomPropertyDrawer(typeof(MyTypeSelector))]
public class MyTypeSelectorDrawer : GenericSelectorPropertyDrawer{} 

*/

/// 
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]

public class GenericSelector<T>
{
    [SerializeField] GameObject _referenceGameObject;
    public GameObject referenceGameObject
    {
        get { return _referenceGameObject; }
        set
        {

            _referenceGameObject = value;
        }
    }
    protected GameObject lastGameObject;
    public T target;

    [ReadOnly]
    [SerializeField]
    string message = "Please run Check(mono) no this component";
    public T valueSource
    {
        get
        {
            if (!wasChecked)
            {
                Check(null);
            }
            return target;
        }
    }

    public Transform referenceGameObjectTransform { get { if (referenceGameObject == null) return null; return referenceGameObject.transform; } }
    public void Check(MonoBehaviour source)
    {


        if (referenceGameObject != null)
        {
            if (target == null || lastGameObject != referenceGameObject)
                target = referenceGameObject.GetComponent<T>();
            if (target == null)
            {
                referenceGameObject = null;
                lastGameObject = null;
            }
            lastGameObject = referenceGameObject;
        }

        if (target == null)
        {
            message = "[No source]";
            referenceGameObject = null;
        }
        else
            message = "OK: [" + target.GetType() + "]";


        lastGameObject = referenceGameObject;
    }
    bool wasChecked = false;
    public virtual void OnValidate(MonoBehaviour source)
    {
        wasChecked = true;
        Check(source);
        //GetValue();
    }




    // /// <summary>    
    // /// Handles null case
    // /// </summary>
    // /// <value></value>
    public string GetMessage()
    {
        string n = "[no source]";
        if (referenceGameObject != null && target != null)
            n = referenceGameObject.name + " [" + target.GetType() + "]";
        // if (overrideValue) n += " [Override]";
        return n;
    }
}
#if UNITY_EDITOR
namespace Z.Extras
{
    public class GenericSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {

            var referenceGameObject = prop.FindPropertyRelative("_referenceGameObject");
            var message = prop.FindPropertyRelative("message");
            // var objectType = prop.serializedObject.targetObject.GetType().ToString();//  prop.objectReferenceValue.GetType().ToString();

            GUILayout.Space(10);
            // GUILayout.Label(objectType);
            EditorGUILayout.PropertyField(referenceGameObject);
            EditorGUILayout.PropertyField(message);
            GUILayout.Space(10);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
}
#endif