using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    public abstract class GenericSubsribe<T>
    {
        public List<T> listeners = new List<T>();
        [SerializeField] [ReadOnly] int listenerCount;
        static GenericSubsribe<T> instance;
        public int Count { get { return listenerCount; } }
        public List<MonoBehaviour> listenerComponents;
        public void StartSingleton()
        {
            if (instance != null && instance != this)
            {
                Debug.Log("GenericSubsribe failed starting singleton, there is already an instance !");
                return;
            }
            instance = this;
        }

        public T this[int i]
        {
            get
            {
                if (listeners == null) return default(T);
                if (i < 0)
                {
                    Debug.Log("negative index");
                    return default(T);
                }
                if (i >= Count)
                {
                    Debug.Log("requested index is too high " + i + " vs " + Count);
                    return default(T);
                }
                return listeners[i];
            }

        }
        public void RegisterListener(T listener)
        {
            if (listeners == null) listeners = new List<T>();
            if (!listeners.Contains(listener)) listeners.Add(listener);
            else
                Debug.Log("Duplicate listener");
            instance.listenerCount = listeners.Count;
            // Debug.Log("adding listener (has " + listeners.Count + ")");
            var asMono = listener as MonoBehaviour;
            if (asMono != null)
            {
                if (instance.listenerComponents == null) instance.listenerComponents = new List<MonoBehaviour>();
                if (!instance.listenerComponents.Contains(asMono)) instance.listenerComponents.Add(asMono);
            }
        }

        public void UnRegisterListener(T listener)
        {
            if (listeners == null) listeners = new List<T>();
            if (listeners.Contains(listener)) listeners.Remove(listener);

            listenerCount = listeners.Count;
            var asMono = listener as MonoBehaviour;
            if (listenerComponents == null) listenerComponents = new List<MonoBehaviour>();
            if (listenerComponents.Contains(asMono)) listenerComponents.Remove(asMono);

        }
        public static void RegisterListenerStatic(T listener)
        {
            if (instance != null)
                instance.RegisterListener(listener);
            else
                Debug.Log("no instance (run MakeSingleton() " + typeof(T).ToString() + " )");
        }

        public static void UnRegisterListenerStatic(T listener)
        {
            if (instance != null)
                instance.UnRegisterListener(listener);
            else
                Debug.Log("no instance (run MakeSingleton() " + typeof(T).ToString() + " )");
        }
    }

}