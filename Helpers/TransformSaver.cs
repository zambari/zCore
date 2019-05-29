using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

namespace Z
{

/// <summary>
/// Purpose of this class is to save outcom of physics simulation. Run your simulation and press Save, 
/// when you exit play mode, press Load and Remove Rigidbodies - you now have a baked final state of yoru obejcts
/// </summary>
    [SelectionBase]
    public class TransformSaver : MonoBehaviour
    {
        [System.Serializable]
        public class TRSContainer
        {
            public TRS[] transforms;
        }
        TRSContainer transforms;
        public string fileName = "transformSave";
        public bool removeRigidBodies=true;

        [ExposeMethodInEditor]
        public void Save()
        {
            transforms = new TRSContainer();
            transforms.transforms = new TRS[transform.childCount];
            for (int i = 0; i < transforms.transforms.Length; i++)
            {
                transforms.transforms[i] = new TRS(transform.GetChild(i));
            }
            transforms.ToJson(fileName);

        }
        [ExposeMethodInEditor]
        public void Load()
        {
            transforms = transforms.FromJson(fileName);
            if (transforms.transforms.Length != transform.childCount)
            {
                Debug.Log("invalid child count, should be "+transforms.transforms.Length+", is "+transform.childCount+"sure this is the right object?");
                return;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                Undo.RegisterCompleteObjectUndo(transform.GetChild(i),"Loaded Saved Transforms");
               transforms.transforms[i].Apply(transform.GetChild(i));

               if (removeRigidBodies)
               {
                   var rb=transform.GetChild(i).GetComponent<Rigidbody>();
                   if (rb!=null) DestroyImmediate(rb);
               }

            }
        }
    }
}
#endif