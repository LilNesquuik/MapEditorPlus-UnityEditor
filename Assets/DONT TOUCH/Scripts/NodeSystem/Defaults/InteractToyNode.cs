using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditorPlus.NodeSystem.Defaults
{
    [Flags]
    public enum InteractToyType
    {
        OnSearching,
        OnSearchAborted,
        OnSearched,
        OnInteracted
    }
    
    public class InteractToyNode : INodeType, IEntryNode
    {
        public string Name => "Interact Toy";
        public string TypeId => "interact_toy";

        public IReadOnlyDictionary<string, NodeParameterDefinition> Parameters => new Dictionary<string, NodeParameterDefinition>
        {
            ["trigger_type"] = new()
            {
                Name = "Trigger Type",
                Type = typeof(InteractToyType),
                Required = true,
                DefaultValue = "OnSearched"
            }
        };
    }
}