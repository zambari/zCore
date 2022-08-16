using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{

    public interface ISelectableUI
    {
         object objectReference { get; set; }
        bool isSelected { get; set; }
        GameObject gameObject { get; }
        string name { get; }
        Transform transform { get; }
        void Toggle(bool val);
    }

    public interface ISelectableUIController
    {
        void HandleSelection(ISelectableUI source, bool value);
        void HandleDestroy(ISelectableUI source); //?
        GameObject gameObject { get; }
        string name { get; }
        Transform transform { get; }
    }
}