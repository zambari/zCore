using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectAnchorHelper : MonoBehaviour {



public bool edit;

RectTransform rect;
[Range(0,1)]
public float xAnchorMin;
[Range(0,1)]
public float xAnchorMax;
//public bool showSymmetrical {get { return _showSymmetrical}}
//[SerializeField] bool _showSymmetrical;
[Range(0,1)]
public float yAnchorMin;
[Range(0,1)]
public float yAnchorMax;
void OnValidate()

{
    
    if (rect==null) rect=GetComponent<RectTransform>();
   // showSymmetrical=_showSymmetrical;
     if (edit)
     {
         SetValues();
     } else  GetValues();
}

void SetValues()
{
    rect.anchorMin=new Vector2(xAnchorMin,yAnchorMin);
    rect.anchorMax=new Vector2(xAnchorMax,yAnchorMax);
}
void GetValues()
{
    xAnchorMin=rect.anchorMin.x;
    xAnchorMax=rect.anchorMax.x;
    yAnchorMin=rect.anchorMin.y;
    yAnchorMax=rect.anchorMax.y;
}

}
