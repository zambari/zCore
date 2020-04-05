using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IApply 
{
    string name {get;}
    void Apply(float f);
    GameObject gameObject {get;}
  
}
