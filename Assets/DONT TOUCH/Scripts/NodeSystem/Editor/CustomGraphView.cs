using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditorPlus.NodeSystem.Editor
{
    public class CustomGraphView : GraphView
    {
        private readonly Dictionary<string, Node> _nodeMap = new();
        private readonly Dictionary<Node, NodeData> _nodeDataMap = new();

        public CustomGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (Edge edge in change.edgesToCreate)
                {
                    // Handle edge creation if needed for serialization
                }
            }

            if (change.elementsToRemove != null)
            {
                foreach (GraphElement element in change.elementsToRemove)
                {
                    if (element is Node node && _nodeDataMap.ContainsKey(node))
                    {
                        NodeData nodeData = _nodeDataMap[node];
                        _nodeMap.Remove(nodeData.nodeId);
                        _nodeDataMap.Remove(node);
                    }
                }
            }

            return change;
        }

        public Node AddNode(INodeType nodeType, Vector2 position)
        {
            string nodeId = Guid.NewGuid().ToString();
            NodeData nodeData = new NodeData(nodeId, nodeType.TypeId, position);

            Node node = CreateNodeUI(nodeType, nodeData);
            
            _nodeMap[nodeId] = node;
            _nodeDataMap[node] = nodeData;

            AddElement(node);
            return node;
        }

        private Node CreateNodeUI(INodeType nodeType, NodeData nodeData)
        {
            Node node = new Node
            {
                title = nodeType.Name,
                viewDataKey = nodeData.nodeId
            };
            
            if (nodeType is not IEntryNode)
            {
                Port inputPort = CreatePort(node, Direction.Input, "Input");
                node.inputContainer.Add(inputPort);
            }
            
            if (nodeType is not IEndNode)
            {
                Port executionPort = CreatePort(node, Direction.Output, "Execute");
                node.outputContainer.Add(executionPort);
            }

            // Parameters
            foreach (KeyValuePair<string, NodeParameterDefinition> param in nodeType.Parameters)
            {
                string paramKey = param.Key;
                NodeParameterDefinition paramDef = param.Value;

                if (!nodeData.parameterValues.ContainsKey(paramKey))
                {
                    nodeData.parameterValues[paramKey] = paramDef.DefaultValue;
                }

                VisualElement field = CreateParameterField(paramDef, nodeData, paramKey);
                if (field != null)
                {
                    node.extensionContainer.Add(field);
                }
            }

            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(nodeData.position, new Vector2(200, 150)));

            return node;
        }

        private Port CreatePort(Node node, Direction direction, string portName)
        {
            Port port = node.InstantiatePort(
                Orientation.Horizontal,
                direction,
                Port.Capacity.Multi,
                typeof(bool)
            );
            port.portName = portName;
            return port;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port =>
                port != startPort &&
                port.node != startPort.node &&
                port.direction != startPort.direction
            ).ToList();
        }

        private VisualElement CreateParameterField(NodeParameterDefinition paramDef, NodeData nodeData, string paramKey)
        {
            if (paramDef.Type == typeof(string))
            {
                TextField textField = new TextField(paramDef.Name)
                {
                    value = nodeData.parameterValues[paramKey]
                };
                textField.RegisterValueChangedCallback(evt =>
                {
                    nodeData.parameterValues[paramKey] = evt.newValue;
                });
                return textField;
            }

            if (paramDef.Type == typeof(int))
            {
                IntegerField intField = new IntegerField(paramDef.Name)
                {
                    value = int.TryParse(nodeData.parameterValues[paramKey], out int val) ? val : 0
                };
                intField.RegisterValueChangedCallback(evt =>
                {
                    nodeData.parameterValues[paramKey] = evt.newValue.ToString();
                });
                return intField;
            }

            if (paramDef.Type == typeof(float))
            {
                FloatField floatField = new FloatField(paramDef.Name)
                {
                    value = float.TryParse(nodeData.parameterValues[paramKey], out float val) ? val : 0f
                };
                floatField.RegisterValueChangedCallback(evt =>
                {
                    nodeData.parameterValues[paramKey] = evt.newValue.ToString(CultureInfo.InvariantCulture);
                });
                return floatField;
            }

            if (paramDef.Type.IsEnum)
            {
                EnumField enumField = new EnumField(paramDef.Name, (Enum)Enum.Parse(paramDef.Type, nodeData.parameterValues[paramKey]));
                enumField.RegisterValueChangedCallback(evt =>
                {
                    nodeData.parameterValues[paramKey] = evt.newValue.ToString();
                });
                return enumField;
            }

            return null;
        }

        public GraphData GetGraphData()
        {
            GraphData graphData = new GraphData();

            foreach (var kvp in _nodeDataMap)
            {
                graphData.nodes.Add(kvp.Value);
            }

            foreach (Edge edge in edges)
            {
                if (edge.output?.node != null && edge.input?.node != null)
                {
                    Node outputNode = edge.output.node;
                    Node inputNode = edge.input.node;

                    if (_nodeDataMap.ContainsKey(outputNode) && _nodeDataMap.ContainsKey(inputNode))
                    {
                        ConnectionData connection = new ConnectionData(
                            _nodeDataMap[outputNode].nodeId,
                            _nodeDataMap[inputNode].nodeId,
                            edge.output.portName,
                            edge.input.portName
                        );
                        graphData.connections.Add(connection);
                    }
                }
            }

            return graphData;
        }

        public void LoadGraphData(GraphData graphData)
        {
            ClearGraph();

            Dictionary<string, Node> loadedNodes = new Dictionary<string, Node>();

            foreach (NodeData nodeData in graphData.nodes)
            {
                INodeType nodeType = NodeTypeRegistry.GetNodeType(nodeData.typeId);
                if (nodeType != null)
                {
                    Node node = CreateNodeUI(nodeType, nodeData);
                    _nodeMap[nodeData.nodeId] = node;
                    _nodeDataMap[node] = nodeData;
                    loadedNodes[nodeData.nodeId] = node;
                    AddElement(node);
                }
            }

            foreach (ConnectionData connection in graphData.connections)
            {
                if (loadedNodes.TryGetValue(connection.outputNodeId, out Node outputNode) &&
                    loadedNodes.TryGetValue(connection.inputNodeId, out Node inputNode))
                {
                    Port outputPort = outputNode.outputContainer.Q<Port>(connection.outputPortName);
                    Port inputPort = inputNode.inputContainer.Q<Port>(connection.inputPortName);

                    if (outputPort != null && inputPort != null)
                    {
                        Edge edge = outputPort.ConnectTo(inputPort);
                        AddElement(edge);
                    }
                }
            }
        }

        private void ClearGraph()
        {
            foreach (Node node in _nodeMap.Values)
            {
                RemoveElement(node);
            }

            _nodeMap.Clear();
            _nodeDataMap.Clear();
        }
    }
}