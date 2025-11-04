using System;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class SchematicObjectDataList
{
    public int RootObjectId { get; set; }
    
    public SerializableVector CullingPosition { get; set; }
    
    public SerializableVector CullingSize { get; set; }

    public List<SchematicBlockData> Blocks { get; set; } = new();
}
