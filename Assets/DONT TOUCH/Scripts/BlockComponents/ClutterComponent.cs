using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClutterComponent : SchematicBlock
{
    [Tooltip("The type of connector.")]
    public SpawnableRoomConnectorType connectorType = SpawnableRoomConnectorType.ClutterSimpleBoxes;

    public override BlockType BlockType => BlockType.Clutter;

    public override void Compile(SchematicBlockData block)
    {
        block.Properties = new Dictionary<string, object>
        {
            { "ConnectorType", connectorType }
        };

        base.Compile(block);
    }

    public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
    {
        ClutterComponent clutterComponent = Create<ClutterComponent>("Assets/Resources/Blocks/Clutter.prefab");
        gameObject = clutterComponent.gameObject;
        
        clutterComponent.connectorType = (SpawnableRoomConnectorType)Convert.ToInt32(block.Properties["ConnectorType"]);

        base.Decompile(ref gameObject, block, parent);
    }
    
    
    private SpawnableRoomConnectorType _prevConnectorType = SpawnableRoomConnectorType.None;

    private void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    private void Update()
    {
        if (_prevConnectorType == connectorType)
            return;
        
        VisibleModel.SetActive(false);
        
        _prevConnectorType = connectorType;
        
        VisibleModel.SetActive(true);
    }

    
    public GameObject VisibleModel
    {
        get
        {
            Transform visibleModel = transform.Find(_prevConnectorType.ToString());
            return visibleModel == null ? transform.Find("None").gameObject : visibleModel.gameObject;
        }
    }
}
