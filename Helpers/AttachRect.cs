using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Z
{
    [ExecuteInEditMode]
    public class AttachRect : MonoBehaviour, IAttach
    { //
        RectTransform canvasRectTransform;          // uses its own canvas ref
        public Transform followTransform;
        public Transform targetTransform { get { return followTransform; } set { followTransform = value; } }
        public Vector3 targetPoint;
        [Header("can be used as offset in 3d space")]
        public Vector3 relativeAttachmentPoint;
        Vector3 _attachmentPoint;
        public void Attach(GameObject g)
        {
            followTransform = (g == null ? null : g.transform);
        }
        void OnEnable()
        {
            canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }
        protected void LateUpdate()
        {
            if (canvasRectTransform == null) return;
            if (followTransform != null)
            {
                _attachmentPoint = followTransform.position + relativeAttachmentPoint;

            }
            else
            {
                _attachmentPoint = targetPoint + relativeAttachmentPoint;
            }
            Vector3 screenPosHud = Camera.main.WorldToViewportPoint(_attachmentPoint);
            screenPosHud.x *= Screen.width;
            screenPosHud.y *= Screen.height;
            //screenPosHud.x *= canvasRectTransform.rect.width;
            //screenPosHud.y *= canvasRectTransform.rect.height;
            transform.position = screenPosHud;// + offset*currentScale;
                                              //		Debug.Log(screenPosHud.z, gameObject);
        }

    }
}