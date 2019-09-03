using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z
{
    public class GridSpacer : MonoBehaviour
    {

        // v.0.1 grid creator
        // Start is called before the first frame update
        [Range(0, 5)]
        public int SizeX = 3;
        [Range(0, 5)]
        public int SizeY = 0;
        [Range(0, 5)]
        public int SizeZ = 3;
        [Range(1, 5)]
        public float spacing = 1.5f;
        public Vector3 bounds = Vector3.one;
        public bool autoSpace;

        int GetItemCount()
        {
            return (2 * SizeX + 1) * (2 * SizeY + 1) * (2 * SizeZ + 1);
        }

        void OnDrawGizmosSelected()
        {
            if (!isActiveAndEnabled) return;
            Gizmos.color = Color.white * 0.7f;
            Vector3 pos = transform.position;
            for (int i = -SizeX; i <= SizeX; i++)
                for (int j = -SizeY; j <= SizeY; j++)
                    for (int k = -SizeZ; k <= SizeZ; k++)
                    {
                        Vector3 thisOffset = new Vector3(i * bounds.x * spacing, j * bounds.y * spacing, k * bounds.z * spacing);
                        Gizmos.DrawWireCube(transform.TransformPoint(thisOffset), bounds);
                    }
        }
        void Start()
        {

        }
        void OnValidate()
        {
            if (autoSpace && enabled)
                SpaceChldren();

        }
        [ExposeMethodInEditor]
        void SpaceChldren()
        {
            int childIndex = 0;
            for (int i = -SizeX; i <= SizeX; i++)
                for (int j = -SizeY; j <= SizeY; j++)
                    for (int k = -SizeZ; k <= SizeZ; k++)
                    {
                        if (childIndex < transform.childCount)
                        {
                            var thistransf = transform.GetChild(childIndex);
                            childIndex++;
#if UNITY_EDITOR
                            Undo.RegisterCompleteObjectUndo(thistransf, "Grid");
#endif
                            Vector3 thisOffset = new Vector3(i * bounds.x * spacing, j * bounds.y * spacing, k * bounds.z * spacing);
                            thistransf.position = transform.TransformPoint(thisOffset);
                        }
                        else return;
                    }
        }
    }
}