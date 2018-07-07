using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 414
#pragma warning disable 219
//zambari 2017
// v.1.01
public class NameHelper
{
    const int maxLen = 15;
    MonoBehaviour mono;
    static char seperator = '\uFEFF'; // unicode whitespace
    const string extraSpace = "  ";
    string baseName;
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

    public void SetName(string newName=null)
    {
        if (newName!=null) 
           baseName=newName;
        

        if (baseName.Length > maxLen) baseName = baseName.Substring(0, maxLen);

        string[] s = baseName.Split('\n');
        baseName = s[0];

        if (useSecondTag)
            mono.name = tag + extraSpace + seperator + baseName + seperator + extraSpace + _tagPost;
        else
            mono.name = tag + extraSpace + seperator + baseName;
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
        if (baseName != null)
            mono.name = baseName;
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
        baseName =  GetWithoutTags(mono.name);

        
    }

}
