using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MaterialSetBasic : MonoBehaviour {
    public Material materialToSet;
    // Use this for initialization
    public bool setNow;
     
    private void OnValidate()
    {
        if (setNow)
        {
            setNow = false;
            setm();

        }

    }
    //public materialPrese
    MeshRenderer[] mrends;
    void setm()
    {
        if (materialToSet == null) return;
        if (mrends==null)
            mrends = GetComponentsInChildren<MeshRenderer>();

        Material[] oneMat = new Material[1];
        Material[] twoMats = new Material[2];
        oneMat[0] = materialToSet;
        twoMats[0] = materialToSet;
        twoMats[1] = materialToSet;

        for (int i = 0; i < mrends.Length; i++)
            {


          //  if (setShared)
            {
                if (mrends[i].sharedMaterials.Length == 1)

                    mrends[i].sharedMaterials = oneMat;
                else
                    mrends[i].sharedMaterials = twoMats ;
            }
           /* else 
                   
            }
            else
            {
                mrends[i].material = materialToSet;
            }
            */
            }

        
            
    }


    void Start () {

        //   mrends = GetComponentsInChildren<MeshRenderer>();
        setm();
      
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
