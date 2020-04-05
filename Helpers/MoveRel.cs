﻿//v0.2
//v.4 setget v2

using UnityEngine;

using UnityEngine.UI;
using Z;
namespace Z
{


    public class MoveRel : MonoBehaviour
    {
        [Header("MoveRel will Set positon relative to parent")]
        [Header("1 = same as parent")]
        public bool previewnPositionX = true;
        public bool previewnPositionY = false;
        public bool previewSizeX = false;
        public bool previewSizeY = false;


        [Range(0, 1)]
        public float previewSettingValue;

        public bool setAnchors;
        [Header("Use public Set methods in runtime")]
        [ReadOnly]
        public int recievedEventCount;
        void Reset()
        {
            parentRect.pivot = Vector2.zero;
            //rect.anchorMin=Vector2.zero;
            rect.pivot = Vector2.zero;
            //   rect.anchorMax=Vector2.zero;
        }

        void Start()
        {
            previewnPositionX = false;
            previewnPositionY = false;
            previewSizeX = false;
            previewSizeY = false;

        }
        protected void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            if (setAnchors)
                rect.SetAnchorsX(0, 0);


            if (transform.parent == null) Debug.LogError("Please only use Move Rel on objects that have a Parent");
            if (rect == null) Debug.LogError("Please only use Move Rel on objects that have a Rect Transfrom ");
            if (parentRect == null) Debug.LogError("Please only use Move Rel on objects which parent have a Rect Transfrom ");
            if (previewSizeY)
            {
                previewnPositionX = false;
                previewnPositionY = false;
                previewSizeX = false;
                SetRelativeSizeY(previewSettingValue);

            }
            else
               if (previewSizeX)
            {
                previewnPositionX = false;
                previewnPositionY = false;
                previewSizeY = false;
                SetRelativeSizeX(previewSettingValue);
            }
            else
               if (previewnPositionX)
            {

                previewnPositionY = false;
                previewSizeX = false;
                previewSizeY = false;
                SetRelativePosX(previewSettingValue);
            }
            else
             if (previewnPositionY)
            {
                previewnPositionX = false;
                previewSizeX = false;
                previewSizeY = false;
                SetRelativePosY(previewSettingValue);
            }

            recievedEventCount = 0;

        }


        public void SetRelativePosX(float f)
        {
            recievedEventCount++;
            previewSettingValue = f;
            rect.SetRelativeLocalX(parentRect, f);
        }
        public void SetSizeX(float f)
        {
            recievedEventCount++;
            previewSettingValue = f;
            rect.SetSizeX(f);
            transform.hasChanged = true;
        }
        public void SetRelativeSizeX(float f)
        {
            recievedEventCount++;
            previewSettingValue = f;
            rect.SetRelativeSizeX(parentRect, f);

        }
        float parentWidth { get { return parentRect.rect.width; } }
        float parentHeight { get { return parentRect.rect.height; } }
        public float normalizedX
        {
            get
            {
                if (parentWidth == 0) return -1;
                return rect.anchoredPosition.x / parentWidth;
            }
            set
            {
                rect.anchoredPosition = new Vector2(value * parentWidth, rect.anchoredPosition.y);
            }
        }

        public float normalizedWidth
        {
            get
            {
                if (parentWidth == 0) return -1;
                return rect.rect.width / parentWidth;
            }
            set
            {
                rect.sizeDelta = new Vector2(value * parentWidth, rect.sizeDelta.y);
            }
        }

        public float normalizedY
        {
            get
            {
                return rect.anchoredPosition.y / parentWidth;
            }
            set
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, value * parentHeight);
            }
        }

        public float normalizedHeight
        {
            get
            {
                return rect.rect.height / parentHeight;
            }
            set
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, value * parentHeight);
            }
        }


        public void SetRelativePosY(float f)
        {
            recievedEventCount++;
            previewSettingValue = f;
            rect.SetRelativeLocalY(parentRect, f);
            transform.hasChanged = true;
        }
        public void SetSizeY(float f)
        {
            recievedEventCount++;
            previewSettingValue = f;
            rect.SetSizeY(f);
            transform.hasChanged = true;
        }
        public void SetRelativeSizeY(float f)
        {
            recievedEventCount++;
            previewSettingValue = f;
            transform.hasChanged = true;
            rect.SetRelativeSizeY(parentRect, f);
        }

        public RectTransform rect //get component will be called only if reference is requested
        {
            get
            {
                if (_rect == null) _rect = GetComponent<RectTransform>();
                if (_rect == null) _rect = gameObject.AddComponent<RectTransform>();
                return _rect;
            }
            set
            {
                _rect = value;
                Debug.Log("setting is now automatic, upadte API ", gameObject);
            }
        }
        public RectTransform parentRect //get component will be called only if reference is requested
        {
            get
            {
                if (_parentRect == null)
                {
                    if (transform.parent == null)
                    {
                        Debug.Log("no parent !", gameObject);
                        return null;
                    }
                    _parentRect = transform.parent.GetComponent<RectTransform>();
                    if (_parentRect == null) _parentRect = transform.parent.gameObject.AddComponent<RectTransform>();

                }
                return _parentRect;
            }
            set
            {
                _parentRect = value;
            }
        }
        private RectTransform _parentRect;
        private RectTransform _rect;
    }
}