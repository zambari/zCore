using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MaterialParameterDrawer
{
    public List<MaterialParameterDetails> parameters = new List<MaterialParameterDetails>();
    public ShaderParser shaderParser = new ShaderParser();
    public void OnValidate()
    {
        shaderParser.OnValidate(parameters);

        for (int i = 0; i < parameters.Count; i++)
        {
            if (parameters[i].hash == 0) parameters[i].hash = Shader.PropertyToID(parameters[i].name);
        }
        for (int i = parameters.Count - 1; i >= 0; i--)
        {
            if (parameters[i].remove) parameters.RemoveAt(i);
        }
    }
    public void ApplyChangesTo(Material material)
    {

        if (material == null) return;
        for (int i = 0; i < parameters.Count; i++)
        {
            parameters[i].ApplyTo(material);
        }
    }
}

[System.Serializable]
public class ShaderParser
{
    [TextArea(2, 3)]
    public string shaderBody;
    public bool TryParsing;
    public bool clear;
    public void OnValidate(List<MaterialParameterDetails> parameters)
    {
        if (clear)
        {
            parameters.Clear();
            clear = false;
        }
        if (TryParsing)
        {
            TryParsing = false;
            var lines = shaderBody.Split('\n');
            Debug.Log("lines " + lines.Length);
            string rangeString = "Range";
            foreach (var line in lines)
            {
                if (!line.Contains("Float") && !line.Contains(rangeString)) continue;
                if (line.Contains("Stencil")) continue;
                var thisLine = line.Trim();
                if (thisLine.StartsWith("//"))
                {
                    Debug.Log("commented out");
                    continue;
                }
                var splittwo = line.Split('(');
                var thisName = splittwo[0];
                MaterialParameterDetails newDetail = new MaterialParameterDetails();
                newDetail.name = thisName.Trim();
                if (line.Contains(rangeString))
                {

                    string rangeSegment = line.Substring(line.IndexOf(rangeString) + rangeString.Length + 1);
                    rangeSegment = rangeSegment.Substring(0, rangeSegment.IndexOf(')'));
                    var rangeSplit = rangeSegment.Split(',');
                    float min;
                    float max;
                    if (System.Single.TryParse(rangeSplit[0], out  min))
                        if (System.Single.TryParse(rangeSplit[1], out  max))
                        {
                            newDetail.paramRange = new Vector2(min, max);
                        }

                }
                int equalspotition = thisLine.IndexOf("=");
                if (equalspotition > 0)
                {
                    var valstr = thisLine.Substring(equalspotition + 1);
                    float val;
                    if (System.Single.TryParse(valstr, out  val))
                    {
                        newDetail.value = val;
                    }
                    else Debug.Log("failed parsing "+valstr);
                }
                parameters.Add(newDetail);
                //  Debug.Log(line);

            }

        }




    }

}
[System.Serializable]
public class MaterialParameterDetails
{
    public string name;
    public int hash;
    [Range(0, 1)]
    public float value;

    public Vector2 paramRange = Vector2.right;
    [HideInInspector] public float lastValue;
    public bool remove;
    public void ApplyTo(Material material)
    {
        if (value != lastValue)
        {
            material.SetFloat(hash, value);
            lastValue = value;
        }
    }

}