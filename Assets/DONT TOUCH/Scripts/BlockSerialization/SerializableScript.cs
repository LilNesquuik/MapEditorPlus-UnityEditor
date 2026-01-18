using System.Collections.Generic;
using System.Linq;

public class SerializableScript
{
    public int ScriptId { get; set; }
    public Dictionary<string, string> Properties { get; set; }
    
    public SerializableScript()
    {
    }

    public SerializableScript(ScriptComponent scriptComponent)
    {
        ScriptId = scriptComponent.ScriptId;
        Properties = scriptComponent.Properties.ToDictionary(x => x.Key, x => x.Value);
    }
}
