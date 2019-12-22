using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Z
{

    // v.0.2 spotlight creation
    public class SpotController : MonoBehaviour
    {

        [SerializeField] new Light light;
        public enum MyLightType { spot, directional }
        public MyLightType type;
        public Color color = Color.white;
        public float distance = 20;
        public LightShadows shadows;
        public float overShoot = 5;
        [Range(-180, 180)]
        public float HorizontalAngle = 30;
        [Range(-180, 180)]
        public float verticalAngle = 45;
        public float radius = 2;
        public float intensity = 1;
        public float a;


        void OnDrawGizmosSelected()
        {
            if (type == MyLightType.spot)
                Gizmos.DrawWireSphere(transform.position, radius);
        }

        readonly static string objectName = "SpotlightController";
#if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Light/Spotlight Controller")]
        static void CreateNewGameObject()
        {
            GameObject g = new GameObject(objectName);
            if (UnityEditor.Selection.activeGameObject != null)
            {
                g.transform.SetParent(UnityEditor.Selection.activeGameObject.transform);
            }
            g.AddComponent<SpotController>();
            UnityEditor.Selection.activeGameObject = g;
        }
#endif
        void Reset()
        {
            if (name == "GameObject") name = objectName;

            light = GetComponentInChildren<Light>();
            if (light != null)
            {
                GameObject g = new GameObject();
                g.transform.SetParent(transform.parent);
                g.transform.SetPositionAndRotation(transform.position, transform.rotation);
                g.AddComponent<SpotController>();
                transform.SetParent(g.transform);
                DestroyImmediate(this);
                return;
            }
            else
            {
                GameObject g = new GameObject("SpotLight");
                g.transform.SetParent(transform);
                g.transform.SetPositionAndRotation(transform.position, transform.rotation);
                light = g.AddComponent<Light>();
                light.type = LightType.Spot;
            }
            light = GetComponentInChildren<Light>();
            OnValidate();
        }
        void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            if (light == null) light = GetComponentInChildren<Light>();
            if (light == null) return;
            if (radius <= 0) radius = 0.01f;
            if (distance <= 0) distance = 0.1f;
            light.type = type == MyLightType.spot ? LightType.Spot : LightType.Directional;
            light.color = color;
            light.transform.localPosition = new Vector3(0, distance, 0);
            light.range = distance + overShoot;
            light.transform.localEulerAngles = new Vector3(90, 0, 0);
            light.shadows = shadows;
            transform.localEulerAngles = new Vector3(verticalAngle, HorizontalAngle, 0);
            light.intensity = intensity;
            if (type == MyLightType.spot)
            {
                a = Mathf.Atan2(radius, distance) * Mathf.Rad2Deg * 2;
                light.spotAngle = a;
            }
        }

    }
}