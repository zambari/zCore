using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Z{
	[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class UniversalTextShow : MonoBehaviour
{
    public string currentValue;
    public static BindingFlags bindingLibrealFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
    public enum SourceTypes { Field, Property };
    public enum SourceValueTypes { Float, String };
    [ClickableEnum]
    public SourceValueTypes sourceValueType;
    FieldInfo fieldInfo;
    PropertyInfo propertyInfo;
    public bool showPrivate;
    [SerializeField]
        float b;
    [SerializeField]
    [ClickableEnum]
    SourceTypes _sourceType;
    public bool showVariableName = true;
    public bool newLineAfterVarName = true;
    public int varNameSize = 10;

    public SourceTypes sourceType
    {
        get { return _sourceType; }
        set
        {
            if (_sourceType == value) return;
            _sourceType = value;
            getSource();
        }

    }

    [SerializeField]
    [HideInInspector]
    GameObject _targetGameObject;
    [SerializeField]
    [HideInInspector]
    Component _targetComponent;

    [SerializeField]
   // [HideInInspector]
    string _sourceName;
    public string sourceName
    {
        get { return _sourceName; }
        set
        {
            //if (_sourceName==value )
            _sourceName = value;

            getSource();
            GetCurrentValue();
        }
    }
    Text _text;
    Text text
    {
        get { if (_text == null) _text = GetComponent<Text>(); return _text; }
    }
void OnEnable()
{
          getSource();
          GetCurrentValue();
}
    void Update()
    {
        //		if (sourceType==SourcType)
        //currentValue
        GetCurrentValue();
    }
    public void getSource()
    {
        if (sourceComponent != null)
        {
            if (sourceType == SourceTypes.Field)
            {
                propertyInfo = null;
                if (showPrivate)
                    fieldInfo = sourceComponent.GetType().GetField(sourceName, bindingLibrealFlag);
                else
                    fieldInfo = sourceComponent.GetType().GetField(sourceName);
            }
            if (sourceType == SourceTypes.Property)
            {
                fieldInfo = null;
                if (showPrivate)
                    propertyInfo = sourceComponent.GetType().GetProperty(sourceName, bindingLibrealFlag);
                else
                    propertyInfo = sourceComponent.GetType().GetProperty(sourceName);
            }
        }
        else
        {
            propertyInfo = null;
            fieldInfo = null;
        }
    }
    public string GetCurrentValue()
    {
        string val = "";
        bool valOk = false;
        if (sourceComponent != null)
        {
            if (propertyInfo != null)
            {

                val = propertyInfo.GetValue(sourceComponent, null).ToString();
                valOk = true;
            }
            else
             if (fieldInfo != null)
            {

                val = fieldInfo.GetValue(sourceComponent).ToString();;
                valOk = true;
            }
        }
        if (!valOk)
        {
            text.text = "invalid";
        }
        else
        {
            if (showVariableName)
            {
                text.text = "<size=" + varNameSize + ">" + sourceName + "</size> ";
                if (newLineAfterVarName) text.text += '\n';

                text.text += val.ToString();
            }
            else
                text.text = val.ToString();

        }
        currentValue = val;
      //  text.text = val.ToString();
        return val;

    }
    public GameObject targetGameObject
    {
        get
        { return _targetGameObject; }
        set
        {
            if (_targetGameObject == value) return;

            _targetGameObject = value;
            getSource();
        }
    }


    public Component sourceComponent
    {
        get { return _targetComponent; }
        set
        {
            if (_targetComponent == value) return;
            _targetComponent = value;
            getSource();
        }


    }
}


}

