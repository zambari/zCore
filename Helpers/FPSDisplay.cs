using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

// v.0.2 show resolutoni
// v.0.3 resoluton, ip adress, fps combination (source: stackoverflow)

namespace ZUI
{
    [RequireComponent(typeof(Text))]
    public class FPSDisplay : MonoBehaviour
    {
        float started;
        int countBeforeDisplaying = 60;
        public enum DisplayMode { FPS, FPSAndResolution, FPSAndResolutionInNewLine, Resolution, IPAddress, Mac }
        public DisplayMode displayMode;
        Text _text;
        Text text { get { if (_text == null) _text = GetComponent<Text>(); return _text; } }
        void OnValidate()
        {
            name = "Display: " + displayMode;
            text.text = GetLabel();
        }

        string averageFpstString;
        void Reset()
        {
            text.text = GetLabel();
            text.raycastTarget = false;
            text.color = Color.white;
            OnValidate();

        }

        string GetIPAddress()
        {
            if (Application.isPlaying)
            {
                string strHostName = Dns.GetHostName();
                IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
                foreach (IPAddress ipaddress in iphostentry.AddressList)
                    if (ipaddress.GetAddressBytes().Length == 4)

                        return ipaddress.ToString();
            }
            return "x.x.x.x";
        }



        string GetMac()
        {
            if (Application.isPlaying)
            {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                // no android support sorry
                try
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        //                    var properties = adapter.GetIPProperties();
                        var tmp = adapter.GetPhysicalAddress();
                        if (tmp != null)
                        {
                            var regex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
                            var replace = "$1:$2:$3:$4:$5:$6";
                            string mac = Regex.Replace(tmp.ToString().ToLower(), regex, replace);
                            return mac;
                        }
                    }
                }
                catch { }
#endif

            }
            return "xx:xx:xx:xx";

        }
        string GetFPSString()
        {
            if (Application.isPlaying)
            {
                float time = Time.unscaledTime - started;
                return ((float)countBeforeDisplaying / time).ToString("F") + " FPS";
            }
            return "XX FPS";
        }
        string GetResolutionString()
        {
            if (Application.isPlaying)
                return "(" + Screen.width + "x" + Screen.height + ")";
            return "(0000 x 0000)";
        }
        string GetLabel()
        {
            switch (displayMode)
            {

                case DisplayMode.FPS:
                    return GetFPSString();
                case DisplayMode.FPSAndResolution:
                    return GetFPSString() + " " + GetResolutionString();
                case DisplayMode.FPSAndResolutionInNewLine:
                    return GetFPSString() + "\r\n" + GetResolutionString();
                case DisplayMode.Resolution:
                    return GetResolutionString();
                case DisplayMode.IPAddress:
                    return GetIPAddress();

                case DisplayMode.Mac:
                    return GetMac();
                default:
                    return "unknown";

            }
        }

        IEnumerator Start()
        {
            Text text = GetComponent<Text>();
            started = Time.unscaledTime;
            if (displayMode == DisplayMode.FPS ||
            displayMode == DisplayMode.FPSAndResolution ||
            displayMode == DisplayMode.FPSAndResolutionInNewLine)
            {
                while (true)
                {   countBeforeDisplaying = 0;
                    while (countBeforeDisplaying < 50)
                    {
                        countBeforeDisplaying++;
                        yield return null;
                    }
                    averageFpstString = "";
                    text.text = GetLabel();
                }
            }
            else
            {
                text.text = GetLabel();

            }
        }
    }
}