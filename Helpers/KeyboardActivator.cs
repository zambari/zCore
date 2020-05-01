using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardActivator : MonoBehaviour
{
    public List<KeycodeGameObjectPair> objectMappings = new List<KeycodeGameObjectPair>();
    public Text helpTextObject;
    [ExposeMethodInEditor]
    void Insert()
    {
        objectMappings.Insert(0, new KeycodeGameObjectPair());
    }
    void Start()
    {
        if (helpTextObject != null)
        {
            string help = "Mapped keys:\r\n";
            for (int i = 0; i < objectMappings.Count; i++)
            {
                if (objectMappings[i].gameObject != null)
                {
                    help += $"[{objectMappings[i].keycode}] ➔ {objectMappings[i].gameObject.name}\r\n";
                }
            }
            helpTextObject.text = help;
        }
    }
    void Update()
    {
        for (int i = 0; i < objectMappings.Count; i++)
        {
            if (objectMappings[i] != null && objectMappings[i].gameObject != null)

                if (Input.GetKeyDown(objectMappings[i].keycode))
                {
                    bool newState = !objectMappings[i].gameObject.activeSelf;
                    if (newState)
                        objectMappings[i].gameObject.Show();
                    else
                        objectMappings[i].gameObject.Hide();
                }
        }
    }

}
[System.Serializable]
public class KeycodeGameObjectPair
{
    public KeyCode keycode;
    public GameObject gameObject;
}