using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragRotator : MonoBehaviour, IDragHandler
{
    public Transform cameraPivot;
    public Vector2 verticalRange = new Vector2(-10, 90);
    public float dragScalarX = 90;
    public float dragScalarY = 90;
    float currentX;
    Vector3 startRot;
    // public float lastDragX;
    void Start()
    {
        startRot = cameraPivot.rotation.eulerAngles;
    }
    public void ResetRot()
    {
        cameraPivot.rotation = Quaternion.Euler(startRot);
    }
    public void OnDrag(PointerEventData eventData)
    {

        var currentRot = cameraPivot.rotation.eulerAngles;
        currentRot.y += eventData.delta.x * dragScalarX / Screen.width;
        currentX = currentRot.x;
        currentX -= eventData.delta.y * dragScalarY / Screen.height;
        if (currentX > 180) currentX -= 360;
        if (currentX > verticalRange.y) currentX = verticalRange.y;
        if (currentX < verticalRange.x) currentX = verticalRange.x;
        currentRot.x = currentX;
        cameraPivot.rotation = Quaternion.Euler(currentRot);
    }
}
