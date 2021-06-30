using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z;


    public class SoloGroup : MonoBehaviour
    {
        SoloActivate lastActive;
        List<SoloActivate> _activeSolos;
        List<SoloActivate> activeSolos { get { if (_activeSolos == null) _activeSolos = new List<SoloActivate>(); return _activeSolos; } }

        public void OnSoloActivated(SoloActivate source)
        {
            if (!activeSolos.Contains(source)) activeSolos.Add(source);
            if (enabled)
            {
                if (lastActive != null && lastActive != source) lastActive.gameObject.Hide();
                lastActive = source;
                source.gameObject.Show();
            }
        }
        public void OnSoloDeactivated(SoloActivate source)
        {

            if (activeSolos.Contains(source)) activeSolos.Remove(source); else Debug.Log("deactivate not form lsit ", gameObject);
            if (enabled)
                source.gameObject.Hide();
        }

        void Start() { } // do not remove, needed for enabler
    }
