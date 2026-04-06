using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class SchematicObjectDataList
{
    public string CompilerVersion { get; set; } = Schematic.CompilerVersion;
    
    public int RootObjectId { get; set; }

    public List<SchematicBlockData> Blocks { get; set; } = new();
}
