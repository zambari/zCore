using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexturePreviewAttribute : PropertyAttribute
{
    public readonly int height;
    public TexturePreviewAttribute(int height = 40)
    {
        this.height = 40;
    }

}

