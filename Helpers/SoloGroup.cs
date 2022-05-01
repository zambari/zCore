using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z;

// to be refactored further (different editor behaviour, no executeineditmode)
// but this approach should stack overflow much less

[ExecuteInEditMode]
public class SoloGroup : MonoBehaviour
{
    // SoloActivate lastActive;
    // List<SoloActivate> _activeSolos;
    // List<SoloActivate> activeSolos { get { if (_activeSolos == null) _activeSolos = new List<SoloActivate>(); return _activeSolos; } }

    public SoloActivate toActivateOnStart;
    // List<SoloActivate> toDeactivate = new List<SoloActivate>();
    SoloActivate currentSolo;
    bool stateChanged;
    void Start()
    {
        toActivateOnStart?.gameObject.SetActive(true);
    }
    public void OnSoloActivated(SoloActivate source)
    {
        currentSolo = source;
        stateChanged = true;
        // if (!activeSolos.Contains(source))
        //     activeSolos.Add(source);
        // if (enabled)
        // {
        //     if (lastActive != null && lastActive != source)
        //     {
        //         toDeactivate.Add(lastActive);
        //     }
        //     // lastActive.gameObject.Hide();
        // }
        // lastActive = source;
        // source.gameObject.Show();
    }
    public void OnSoloDeactivated(SoloActivate source)
    {

        // if (activeSolos.Contains(source)) activeSolos.Remove(source); else Debug.Log("deactivate not form lsit ", gameObject);
        // if (enabled)
        // {

        // }
        //        source.gameObject.Hide();
    }
    void Update()
    {
        if (stateChanged)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var thisSolo = transform.GetChild(i).GetComponent<SoloActivate>();
                if (thisSolo != currentSolo)
                {
                    thisSolo.gameObject.SetActive(false);
                }
            }
            stateChanged = false;
        }

        // if (toDeactivate.Count > 0)
        // {
        //     foreach (var td in toDeactivate)
        //     {

        //         td.gameObject.Hide();
        //         if (activeSolos.Contains(td)) activeSolos.Remove(td);
        //     }
        //     toDeactivate.Clear();
        // }

    }
}
