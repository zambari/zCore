using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{

    public enum TransformType { none, TransformPoint, TransformVector, TransformDirection, InverseTransformPoint, InverseTransformVector, InverseTransformDirection }
    public enum TransformAxis { none, X, Y, Z }

    public static class TransformationHelper
    {
        public static Vector3 Mirror(this Vector3 source, TransformAxis axis)
        {
            switch (axis)
            {
                case TransformAxis.X: return new Vector3(-source.x, source.y, source.z);
                case TransformAxis.Y: return new Vector3(source.x, -source.y, source.z);
                case TransformAxis.Z: return new Vector3(source.x, source.y, -source.z);
                default: return source;
            }
        }
        public static TransformType Inverse(this TransformType transformType)
        {
            switch (transformType)
            {
                case TransformType.TransformPoint:
                    return TransformType.InverseTransformPoint;
                case TransformType.TransformVector:
                    return TransformType.InverseTransformVector;
                case TransformType.TransformDirection:
                    return TransformType.InverseTransformDirection;
                case TransformType.InverseTransformPoint:
                    return TransformType.TransformPoint;
                case TransformType.InverseTransformVector:
                    return TransformType.TransformVector;
                case TransformType.InverseTransformDirection:
                    return TransformType.TransformDirection;
                default:
                    return TransformType.none;
            }
        }
        public static Vector3 GetTransformation(this Transform transform, TransformType type, Vector3 source)
        {

            switch (type)
            {
                case TransformType.TransformPoint:
                    return transform.TransformPoint(source);
                case TransformType.TransformVector:
                    return transform.TransformVector(source);
                case TransformType.TransformDirection:
                    return transform.TransformDirection(source);
                case TransformType.InverseTransformPoint:
                    return transform.InverseTransformPoint(source);
                case TransformType.InverseTransformVector:
                    return transform.InverseTransformVector(source);
                case TransformType.InverseTransformDirection:
                    return transform.InverseTransformDirection(source);
                default:
                    return source;

            }
        }
    }
}
