using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapEditorPlus.NodeSystem.Editor
{
    public class CustomGraphEditorWindow : EditorWindow
    {
        private CustomGraphView _graphView;
        private VisualElement _leftPanel;
        private string _currentFilePath;

        [MenuItem("MapEditorPlus/Action Graph Editor")]
        public static void Open()
        {
            CustomGraphEditorWindow window = GetWindow<CustomGraphEditorWindow>();
            window.titleContent = new GUIContent("Action Graph Editor");
            window.minSize = new Vector2(800, 600);
            window.Show();
        }

        private void OnEnable()
        {
            CreateGraphView();
            CreateLeftPanel();
            CreateToolbar();
            SetupLayout();
        }

        private void CreateGraphView()
        {
            _graphView = new CustomGraphView
            {
                name = "CustomGraphView",
                style =
                {
                    flexGrow = 1
                }
            };
        }

        private void CreateLeftPanel()
        {
            _leftPanel = new VisualElement
            {
                name = "LeftPanel",
                style =
                {
                    width = 200,
                    flexShrink = 0,
                    backgroundColor = new Color(0.15f, 0.15f, 0.15f, 1f),
                    paddingTop = 10,
                    paddingBottom = 10
                }
            };

            Label title = new Label("Node Types")
            {
                style =
                {
                    fontSize = 16,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    marginBottom = 10
                }
            };
            _leftPanel.Add(title);

            foreach (INodeType nodeType in NodeTypeRegistry.Types)
            {
                Button button = CreateNodeButton(nodeType);
                _leftPanel.Add(button);
            }
        }

        private Button CreateNodeButton(INodeType nodeType)
        {
            Button button = new Button(() => AddNodeToGraph(nodeType))
            {
                text = nodeType.Name,
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    height = 35,
                    marginBottom = 5,
                    marginLeft = 10,
                    marginRight = 10,
                    borderTopLeftRadius = 5,
                    borderTopRightRadius = 5,
                    borderBottomLeftRadius = 5,
                    borderBottomRightRadius = 5,
                    backgroundColor = new Color(0.25f, 0.25f, 0.25f, 1f),
                    color = Color.white,
                    fontSize = 13,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };

            SetupButtonHoverEffects(button);
            return button;
        }

        private void SetupButtonHoverEffects(Button button)
        {
            Color normalColor = new Color(0.25f, 0.25f, 0.25f, 1f);
            Color hoverColor = new Color(0.35f, 0.35f, 0.35f, 1f);
            Color pressColor = new Color(0.15f, 0.15f, 0.15f, 1f);

            button.RegisterCallback<MouseEnterEvent>(evt => button.style.backgroundColor = hoverColor);
            button.RegisterCallback<MouseLeaveEvent>(evt => button.style.backgroundColor = normalColor);
            button.RegisterCallback<MouseDownEvent>(evt => button.style.backgroundColor = pressColor);
            button.RegisterCallback<MouseUpEvent>(evt => button.style.backgroundColor = hoverColor);
        }

        private void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            ToolbarButton saveButton = new ToolbarButton(SaveGraph) { text = "Save" };
            ToolbarButton loadButton = new ToolbarButton(LoadGraph) { text = "Load" };
            ToolbarButton newButton = new ToolbarButton(NewGraph) { text = "New" };

            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(newButton);

            rootVisualElement.Add(toolbar);
        }

        private void SetupLayout()
        {
            VisualElement rootContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1
                }
            };

            rootContainer.Add(_leftPanel);
            rootContainer.Add(_graphView);

            rootVisualElement.Add(rootContainer);
        }

        private void AddNodeToGraph(INodeType nodeType)
        {
            Vector2 spawnPosition = new Vector2(300, 300);
            _graphView.AddNode(nodeType, spawnPosition);
        }

        private void SaveGraph()
        {
            string path = EditorUtility.SaveFilePanel(
                "Save Graph",
                Application.dataPath,
                "NewGraph",
                "json"
            );

            if (string.IsNullOrEmpty(path))
                return;

            GraphData graphData = _graphView.GetGraphData();
            string json = JsonUtility.ToJson(graphData, true);
            File.WriteAllText(path, json);

            _currentFilePath = path;
            Debug.Log($"Graph saved to: {path}");
        }

        private void LoadGraph()
        {
            string path = EditorUtility.OpenFilePanel(
                "Load Graph",
                Application.dataPath,
                "json"
            );

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;

            string json = File.ReadAllText(path);
            GraphData graphData = JsonUtility.FromJson<GraphData>(json);

            _graphView.LoadGraphData(graphData);
            _currentFilePath = path;

            Debug.Log($"Graph loaded from: {path}");
        }

        private void NewGraph()
        {
            if (EditorUtility.DisplayDialog(
                "New Graph",
                "Are you sure you want to create a new graph? Unsaved changes will be lost.",
                "Yes",
                "Cancel"))
            {
                _graphView.LoadGraphData(new GraphData());
                _currentFilePath = null;
            }
        }

        private void OnDisable()
        {
            if (_graphView != null)
            {
                rootVisualElement.Remove(_graphView);
            }
        }
    }
}