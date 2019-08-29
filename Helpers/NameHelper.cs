using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 414
#pragma warning disable 219
//zambari 2017
// v.1.01
// v.1.02 extra space when no tag
[System.Serializable]
public class NameHelper
{
    const int maxLen = 45;
    MonoBehaviour mono;

    public static char seperator = '\uFEFF'; // unicode whitespace
                                             //static char seperator = '@'; // debug
    const string extraSpace = "  ";
    public string baseName;
    bool isMangledCached;
    bool useSecondTag = false;
    string _tag = "";
    string _tagPost = "";
    public bool useDecorators;
    string decoA = "[";
    string decoB = "]";
    string decoratedTag;

    public bool isMangled
    {
        get
        {
            return (mono.name.Contains(seperator.ToString()));
        }
    }

    public string tag
    {
        get { return _tag; }
        set
        {
            if (useDecorators)
                _tag = decoA + value + decoB;
            else
                _tag = value;
            //   SetName();
        }
    }
    public string tagPost
    {
        get { return _tagPost; }
        set
        {
            if (value == null || value.Length == 0) useSecondTag = false; else useSecondTag = true;
            _tagPost = value;
            //   SetName();
        }
    }

    public void SetName(string newName = null)
    {
        if (newName != null)
            baseName = GetWithoutTags(newName);
        else
        {
            baseName = GetWithoutTags(mono.name);
            if (baseName.Length<3) Debug.Log("cos zle z naszym name");
        }
        string[] s = baseName.Split('\n');
        baseName = s[0];

        if (baseName.Length > maxLen) baseName = baseName.Substring(0, maxLen);


        string thisNewName = tag;
        if (!useSecondTag)
        {
            thisNewName += extraSpace;
         
        }
           thisNewName += seperator;
        thisNewName += baseName;

        if (useSecondTag)
            thisNewName += seperator + extraSpace + _tagPost;

        mono.name = thisNewName;
    }
    public void SetTag(string t)
    {
        _tag = t;
        SetName();
    }
    public void SetTagPost(string t)
    {
        useSecondTag = true;
        _tagPost = t;
        SetName();
    }
    public void RemoveTag()
    {
        //if (baseName != null)
        mono.name = GetWithoutTags(mono.name);// baseName;
    }
    public string removeTag
    {
        get { RemoveTag(); return null; }
        set { RemoveTag(); }
    }
    public static string GetWithoutTags(string s)
    {
        string[] split = s.Split(seperator);
        if (split.Length < 2) return s;
        return split[1];
    }

    //string baseName;
    /*    public string unMangled(string b)
        {
            string[] split = b.Split(seperator);
            if (split.Length < 2) return b;
            return split[1];
        }*/
    public NameHelper(MonoBehaviour source)
    {
        mono = source;
        baseName = GetWithoutTags(mono.name);


    }

}
