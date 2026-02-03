using System.Collections.Generic;

namespace MapEditorPlus.NodeSystem.Defaults
{
    public class DelayActionNode : INodeType
    {
        public string Name => "Delay Action";
        public string TypeId => "delay_action";

        public IReadOnlyDictionary<string, NodeParameterDefinition> Parameters => new Dictionary<string, NodeParameterDefinition>
        {
            ["duration"] = new()
            {
                Name = "Duration",
                Type = typeof(float),
                Required = true,
                DefaultValue = "1"
            }
        };
    }
}