//zbr2017 +2019

using UnityEngine;
using UnityEngine.EventSystems;
// 
namespace Z
{

    //v.0.2 control mapping change, but collisions removed

    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float panScalar = 0.01f;
        public float lookScalarX = -0.2f;
        public float lookScalarY = 0.12f;
        public float targetDistance = 0.5f;
        float fovChangeScalar = 9f;
        SphereCollider sphereCollider;
        public float zoomSpeed = 2;
        Quaternion startRotation;
        Vector3 startPosition;

        Vector3 dragStarted;
        Vector3 posWhenMidStarted;

        bool isDragging;
        Vector3 eulerWhenDragStarted;
        float timeOfMidClick;
        Camera controlledCamera;
        public Vector2 fovRange = new Vector2(30, 90);
        void Start()
        {
            if (target == null)
            {
                GameObject g = new GameObject("CAMERA MOVEMENT");
                target = g.transform;
                target.parent = transform.parent;
                target.position = transform.position + new Vector3(0, 0, -targetDistance);
                target.rotation = transform.rotation;
                transform.parent = target;
            }

            //  if (sphereCollider == null) sphereCollider = gameObject.AddComponent<SphereCollider>();
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            sphereCollider = GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                //sphereCollider = gameObject.AddComponent<SphereCollider>();
            }
            if (sphereCollider != null)
            {

                sphereCollider.isTrigger = true;
                sphereCollider.radius = 0.3f;
            }
            startRotation = target.rotation;
            startPosition = target.position;
            controlledCamera = target.GetComponentInChildren<Camera>();
        }

        void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(2))
                {
                    if (Time.unscaledTime - timeOfMidClick < 0.3f)
                    {
                        Debug.Log("cam reset");
                        target.rotation = startRotation;
                        target.position = startPosition;
                    }

                    timeOfMidClick = Time.unscaledTime;
                }

                if (Input.GetMouseButtonDown(2) || // mmb OR lmb + alt + shift
                   (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt)))
                {
                    dragStarted = Input.mousePosition;
                    posWhenMidStarted = target.position;
                    // ??
                    eulerWhenDragStarted = target.rotation.eulerAngles;
                    //? 
                    isDragging = true;
                }
                float scrolled = Input.GetAxis("Mouse ScrollWheel");
                if (scrolled != 0)
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                        target.position += target.forward * scrolled * zoomSpeed;
                    else
                    {
                        if (controlledCamera != null)
                        {
                            float newFov = controlledCamera.fieldOfView - scrolled * fovChangeScalar;
                            controlledCamera.fieldOfView = fovRange.Clamp(newFov);
                        }
                    }
                }

            }
            if (isDragging)
            {
                if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftControl))
                {
                    isDragging = false;
                    return;
                }

                if (Input.GetMouseButton(2) ||
                    (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift)))
                {
                    Vector3 distance = (dragStarted - Input.mousePosition) * panScalar;
                    target.position = posWhenMidStarted + target.right * distance.x + target.up * distance.y;//+correction;
                }
                if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftControl))
                {
                    Vector3 distance = (Input.mousePosition - dragStarted);
                    target.rotation = Quaternion.Euler(eulerWhenDragStarted + new Vector3(distance.y * lookScalarX, distance.x * lookScalarY, 0));
                }

            }

        }
    }
}