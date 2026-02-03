using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditorPlus.NodeSystem
{
    [Serializable]
    public class NodeData
    {
        public string nodeId;
        public string typeId;
        public Vector2 position;
        public Dictionary<string, string> parameterValues = new();
        
        public NodeData(string nodeId, string typeId, Vector2 position)
        {
            this.nodeId = nodeId;
            this.typeId = typeId;
            this.position = position;
        }
    }

    [Serializable]
    public class ConnectionData
    {
        public string outputNodeId;
        public string inputNodeId;
        public string outputPortName;
        public string inputPortName;

        public ConnectionData(string outputNodeId, string inputNodeId, string outputPortName = "Output", string inputPortName = "Input")
        {
            this.outputNodeId = outputNodeId;
            this.inputNodeId = inputNodeId;
            this.outputPortName = outputPortName;
            this.inputPortName = inputPortName;
        }
    }

    [Serializable]
    public class GraphData
    {
        public List<NodeData> nodes = new List<NodeData>();
        public List<ConnectionData> connections = new List<ConnectionData>();
    }
}