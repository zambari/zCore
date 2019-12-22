using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
//v.0.2. symmetrical mode

//v.0.3 deactivation on assembly reload
//v.0.4 margin control
//v.0.5 public setters

namespace zUI
{
    [ExecuteInEditMode]
    public class RectAnchorHelper : MonoBehaviour
    {
        [SerializeField] bool edit;
        [SerializeField] bool _symmetricalXMode;
        [SerializeField] bool _symmetricalYMode;
        public bool symmetricalXMode { get { return _symmetricalXMode; } set { _symmetricalXMode = value;  CheckAndSet(); } }

        public bool symmetricalYMode  { get { return _symmetricalYMode; } set { _symmetricalYMode = value;  CheckAndSet(); } }
        [SerializeField] RectTransform rect;
        [Range(0, 1)]
        [SerializeField] float _xAnchorMin;
        [Range(0, 1)]
        [SerializeField] float _xAnchorMax=1;
        [Range(0, 1)]
        [SerializeField] float _yAnchorMin;
        [Range(0, 1)]
        [SerializeField] float _yAnchorMax=1;
        public float xAnchorMin { get { return _xAnchorMin; } set { _xAnchorMin = value; CheckAndSet(); } }

        public float xAnchorMax { get { return _xAnchorMax; } set { _xAnchorMax = value; CheckAndSet(); } }

        public float yAnchorMin { get { return _yAnchorMin; } set { _yAnchorMin = value; CheckAndSet(); } }


        public float yAnchorMax { get { return _yAnchorMax; } set { _yAnchorMax = value; CheckAndSet(); } }

        // [SerializeField] [HideInInspector] Vector2 offsetMin;
        // [SerializeField] [HideInInspector] Vector2 offsetMax;
        [Range(-1, 100)]
        [SerializeField] float margin = -1;

        void CheckAndSet()
        {
            if (symmetricalXMode) _xAnchorMax = 1 - _xAnchorMin;
            if (symmetricalYMode) _yAnchorMax = 1 - _yAnchorMin;
            SetValues();

        }
        void Reset()
        {
            GetValues();
        }
        void OnValidate()
        {
            //      if (Application.isPlaying) return;
            if (rect == null) rect = GetComponent<RectTransform>();
            if (symmetricalXMode) xAnchorMax = 1 - xAnchorMin;
            if (symmetricalYMode) yAnchorMax = 1 - yAnchorMin;
            if (edit)
            {
                SetValues();
            }
            else
                GetValues();
        }

        void SetValues()
        {
            rect.anchorMin = new Vector2(xAnchorMin, yAnchorMin);
            rect.anchorMax = new Vector2(xAnchorMax, yAnchorMax);
            if (margin != -1)
            {
                rect.offsetMin = new Vector2(margin, margin);
                rect.offsetMax = new Vector2(-margin, -margin);
            }
        }
        void GetValues()
        {
            if (rect == null) rect = GetComponent<RectTransform>();
            _xAnchorMin = rect.anchorMin.x;
            _xAnchorMax = rect.anchorMax.x;
            _yAnchorMin = rect.anchorMin.y;
            _yAnchorMax = rect.anchorMax.y;
            // offsetMin = rect.offsetMin;
            // offsetMax = rect.offsetMax;
        }
#if UNITY_EDITOR
        void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        }
        public void OnBeforeAssemblyReload()
        {
            edit = false;
        }
#endif
    }


}

