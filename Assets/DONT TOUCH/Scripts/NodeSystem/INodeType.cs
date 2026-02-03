using System;
using System.Collections.Generic;

namespace MapEditorPlus.NodeSystem
{
    public interface INodeType
    {
        string Name { get; }
        string TypeId { get; }
        IReadOnlyDictionary<string, NodeParameterDefinition> Parameters { get; }
    }

    public class NodeParameterDefinition
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
    }
}