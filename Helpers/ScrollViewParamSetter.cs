using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
#if UNITY_EDITOR
    public class ScrollViewParamSetter : MonoBehaviour
    {
        void Reset()
        {
            var scroll = GetComponent<ScrollRect>();
            scroll.horizontal = false;
            DestroyImmediate(scroll.horizontalScrollbar.gameObject);
            var vertScroll = scroll.verticalScrollbar;
            vertScroll.handleRect.GetComponent<Image>().sprite = null;
            vertScroll.GetComponent<Image>().sprite = null;
            vertScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 0);
            vertScroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);
            var sliding = scroll.verticalScrollbar.transform.Find("Sliding Area");
            if (sliding == null) Debug.Log("no slinding area"); else sliding.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            sliding.Find("Handle").GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            var contentfitter = scroll.content.AddOrGetComponent<ContentSizeFitter>();
            contentfitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var layout = contentfitter.AddOrGetComponent<VerticalLayoutGroup>();
            layout.SetChildControl();
            UnityEditor.Selection.activeGameObject = layout.gameObject;
            scroll.content.gameObject.AddTextChild("content");
            DestroyImmediate(this);
        }
    }
#endif
}