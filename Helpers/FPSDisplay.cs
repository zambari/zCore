using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// v.0.2 show resolutoni

namespace ZUI
{
    [RequireComponent(typeof(Text))]
    public class FPSDisplay : MonoBehaviour
    {
        int fpscount;
        float started;
        int countBeforeDisplaying = 60;
        public bool showResolution;
        public bool splitWithNewLine;
        void OnValidate()
        {
            if (!name.Contains("FPS")) name += "FPS";
        }

        string averageFpstString;
        void Reset()
        {
            Text text = GetComponent<Text>();
            text.text = "00 FPS";

            text.raycastTarget = false;
            text.color = Color.white;
        }

        IEnumerator Start()
        {
            Text text = GetComponent<Text>();
            while (true)
            {

                countBeforeDisplaying = 0;
                started = Time.unscaledTime;
                while (countBeforeDisplaying < 50)
                {
                    countBeforeDisplaying++;
                    yield return null;
                }

                float time = Time.unscaledTime - started;
                averageFpstString = ((float)countBeforeDisplaying / time).ToShortString() + " FPS";
                if (showResolution)
                {
                    averageFpstString += splitWithNewLine ? "\n(" : " (" + Screen.width + "x" + Screen.height + ")";
                }
                text.text = averageFpstString;

            }
        }
    }
}