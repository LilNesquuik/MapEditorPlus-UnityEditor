using System;
using System.Collections.Generic;

namespace MapEditorPlus.NodeSystem.Defaults
{
    public enum AnimatorActionType
    {
        SetFloat,
        SetInt,
        SetBool,
        SetTrigger
    }

    public class AnimatorActionNode : INodeType, IEndNode
    {
        public string Name => "Animator Action";
        public string TypeId => "animator_action";

        public IReadOnlyDictionary<string, NodeParameterDefinition> Parameters => new Dictionary<string, NodeParameterDefinition>
        {
            ["parameter_name"] = new()
            {
                Name = "Parameter Name",
                Type = typeof(string),
                Required = true,
                DefaultValue = ""
            },
            ["action_type"] = new()
            {
                Name = "Action Type",
                Type = typeof(AnimatorActionType),
                Required = true,
                DefaultValue = "SetBool"
            },
            ["value"] = new()
            {
                Name = "Value",
                Type = typeof(string),
                Required = false,
                DefaultValue = "true"
            }
        };
    }
}