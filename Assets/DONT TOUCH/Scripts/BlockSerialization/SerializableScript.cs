using System.Collections.Generic;
using System.Linq;

public class SerializableScript
{
    public string ScriptName { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    
    public SerializableScript()
    {
    }

    public SerializableScript(ScriptComponent scriptComponent)
    {
        ScriptName = scriptComponent.ScriptName;
        Properties = scriptComponent.Properties.ToDictionary(x => x.Key, x => x.Value);
    }
}
