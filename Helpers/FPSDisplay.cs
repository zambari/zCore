using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]

public class FPSDisplay : MonoBehaviour
{
    int fpscount;
    float timeStrated;
    void OnValidate()
    {
        if (!name.Contains("FPS")) name += "FPS";
    }

    string averageFpstString;

    IEnumerator Start()
    {
         Text text = GetComponent<Text>();
        while (true)
        {

            fpscount=0;
            timeStrated = Time.time;
            while (fpscount < 50)
            {
                fpscount++;
                yield return null;

            }

            float time = Time.time - timeStrated;
            //            if (fpscount >= 100)
            //          {
            //fpscount = 0;
            //float time = Time.time - timeStrated;
            //timeStrated = Time.time;
            averageFpstString = ((float)fpscount / time).ToShortString();
            text.text = averageFpstString+" FPS";
            /*    }
                stringBuilder = new StringBuilder();

                stringBuilder.Append(s ? ">" : " ");
                stringBuilder.Append(averageFpstString);

                s = !s;
                try
                {
                    oculusfpsString = OVRPlugin.GetAppFramerate().ToShortString() + (" FPS");
                    stringBuilder.Append(" fps\n");
                    stringBuilder.Append(oculusfpsString);
                }
                catch { };
                text.text = stringBuilder.ToString();


                yield return new WaitForSeconds(.4f);*/
        }
    }
}
