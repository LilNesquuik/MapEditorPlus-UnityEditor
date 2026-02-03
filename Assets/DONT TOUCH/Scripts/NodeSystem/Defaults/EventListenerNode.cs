using System;
using System.Collections.Generic;

namespace MapEditorPlus.NodeSystem.Defaults
{
    public class EventListenerNode : INodeType, IEntryNode
    {
        public string Name => "Event Listener";
        public string TypeId => "event_listener";
        public Type SupportedType => typeof(AnimatorActionNode);

        public IReadOnlyDictionary<string, NodeParameterDefinition> Parameters => new Dictionary<string, NodeParameterDefinition>
        {
            ["event_id"] = new()
            {
                Name = "Event ID",
                Type = typeof(int),
                Required = true,
                DefaultValue = "0"
            }
        };
    }
}