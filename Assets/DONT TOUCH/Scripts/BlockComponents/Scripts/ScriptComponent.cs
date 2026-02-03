using System.Collections.Generic;
using UnityEngine;

public class ScriptComponent : MonoBehaviour
{
    [Tooltip("The name of the script.")]
    public string ScriptName;
    
    [Tooltip("The properties of the script.")]
    [SerializeField]
    public List<ScriptProperty> properties = new();
    
    public IReadOnlyDictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> dict = new();
            foreach (ScriptProperty p in properties)
                dict[p.Key] = p.Value;

            return dict;
        }
    }
}

[System.Serializable]
public class ScriptProperty
{
    public string Key;
    public string Value;
}
