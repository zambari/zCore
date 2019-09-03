//zbr2017

using UnityEngine;
using UnityEngine.EventSystems;

public class zCameraController : MonoBehaviour
{
   public Transform target;
    public float panScalar = 0.01f;
    public float lookScalarX = -0.2f;
    public float lookScalarY = 0.12f;
    public float targetDistance = 0.5f;
    SphereCollider sphereCollider;
    Vector3 counterCollision;
    public float zoomSpeed=2;
    public float colliderProximity;
    void Start()
    {
        if (target==null)
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
        if (rb == null) rb=gameObject.AddComponent<Rigidbody>();
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

    }
  
    void OnTriggerStay(Collider other)
    {
        Vector3 closespoint = other.ClosestPointOnBounds(target.position);
        counterCollision = target.position - closespoint;
        colliderProximity = 1 - (counterCollision.magnitude);//- minDistane) / startPushFromDistance;
        if (colliderProximity > 0)
        {
            counterCollision.Normalize();

            target.position += counterCollision * colliderProximity * 0.1f;
            posWhenMidStarted += counterCollision * colliderProximity * 0.1f;
        }
        if (colliderProximity < 0.5f)
        {
            draggingMid = false;
        }


    }

    Vector3 dragMidStarted;
    Vector3 posWhenMidStarted;
    Vector3 dragRightStarted;

    bool draggingRight;
    bool draggingMid;
    Vector3 eulerWhenDragStarted;

    void Update()
    {    if (Input.GetMouseButtonUp(1))
            draggingRight = false;
        if (Input.GetMouseButtonUp(2))
            draggingMid = false;
          if (draggingMid)
        {
            Vector3 distance = (dragMidStarted - Input.mousePosition) * panScalar;
            target.position = posWhenMidStarted + target.right * distance.x + target.up * distance.y;//+correction;
        }
        if (draggingRight)
        {
            Vector3 distance = (dragRightStarted - Input.mousePosition);
            target.rotation = Quaternion.Euler(eulerWhenDragStarted + new Vector3(distance.y * lookScalarX, distance.x * lookScalarY, 0));
        }
        if ( !EventSystem.current.IsPointerOverGameObject())
        {

            if (Input.GetMouseButtonDown(1))
                {
                    dragRightStarted = Input.mousePosition;
                    eulerWhenDragStarted = target.rotation.eulerAngles;
                    draggingRight = true;
                }

                if (Input.GetMouseButtonDown(2))
                {
                    dragMidStarted = Input.mousePosition;
                    posWhenMidStarted = target.position;
                    draggingMid = true;
                }
            

            
                float scrolled = Input.GetAxis("Mouse ScrollWheel");

                if (scrolled != 0)
                    target.position += target.forward * scrolled*zoomSpeed;
            }

    }
}
