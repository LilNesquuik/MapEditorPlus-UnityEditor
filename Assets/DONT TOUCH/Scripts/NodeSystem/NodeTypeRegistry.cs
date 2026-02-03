using System;
using System.Collections.Generic;
using System.Linq;
using MapEditorPlus.NodeSystem.Defaults;

namespace MapEditorPlus.NodeSystem
{
    public static class NodeTypeRegistry
    {
        private static List<INodeType> _types;

        public static IReadOnlyList<INodeType> Types
        {
            get
            {
                if (_types == null)
                {
                    InitializeRegistry();
                }
                return _types;
            }
        }

        private static void InitializeRegistry()
        {
            _types = new List<INodeType>
            {
                new EventListenerNode(),
                new DelayActionNode(),
                new AnimatorActionNode(),
                new InteractToyNode()
            };
        }

        public static INodeType GetNodeType(string typeId)
        {
            return Types.FirstOrDefault(t => t.TypeId == typeId);
        }

        public static void RegisterNodeType(INodeType nodeType)
        {
            if (_types == null)
            {
                InitializeRegistry();
            }

            if (!_types.Any(t => t.TypeId == nodeType.TypeId))
            {
                _types.Add(nodeType);
            }
        }
    }
}