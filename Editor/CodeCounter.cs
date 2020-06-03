using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// v.02 plugin exclusion as option
namespace Z
{
    public class CodeCounter : MonoBehaviour
    {
        public bool onlyInScripts;
        public bool excludePlugins = true;
        [HideInInspector]
        public bool excludeMorePlugins = true;
        [ExposeMethodInEditor]
        void CountCode()
        {
            string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            int bytes = 0;
            int fileCount = 0;
            int lines = 0;
            int skippedCount = 0;
            for (int i = 0; i < files.Length; i++)
            {
                if (excludePlugins)
                    if (files[i].Contains("plugins") || files[i].Contains("Plugins"))
                    {
                        skippedCount++;
                        continue;
                    }
                if (excludeMorePlugins)
                    if (files[i].Contains("PostProcessing ") || files[i].Contains("RainbowFolders") || files[i].Contains("TextMesh Pro)"))
                    {
                        skippedCount++;
                        continue;
                    }
                if (onlyInScripts)
                {
                    if (!files[i].Contains("scripts") && !files[i].Contains("Scripts"))
                    {
                        skippedCount++;
                        continue;

                    }
                }
                string thisFile = File.ReadAllText(files[i]);
                bytes += thisFile.Length;
                string[] lineSplit = thisFile.Split('\n');
                lines += lineSplit.Length;
                fileCount++;
            }
            Debug.Log("found " + lines + " lines " + bytes / 1024 + "k bytes in " + fileCount + " files (plugins " + (excludePlugins? "excluded": "included") + ")");
            Debug.Log("skipped " + skippedCount);
        }
    }
}