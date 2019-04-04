using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CodeCounter : MonoBehaviour
{

    [ExposeMethodInEditor]
    void CountCode()
    {
        string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        int bytes = 0;
        int fileCount = 0;
        int lines = 0;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Contains("plugins") || files[i].Contains("Plugins")) continue;
            string thisFile = File.ReadAllText(files[i]);
            bytes += thisFile.Length;
            string[] lineSplit = thisFile.Split('\n');
            lines += lineSplit.Length;
            fileCount++;
        }
        Debug.Log("found " + lines + " lines " + bytes/1024 + "k bytes in " + fileCount + " files (plugins excluded)");
    }
}
