using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyInBuild : MonoBehaviour
{
    // Start is called before the first frame update
#if UNITY_EDITOR
#else
    void Awake()
    {
        Destroy(gameObject);
    }
#endif

}
