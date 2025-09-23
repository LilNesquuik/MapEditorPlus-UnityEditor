using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointComponent : SchematicBlock, IAdminToy
{
    public override BlockType BlockType => BlockType.Waypoint;

    public Vector3 Bounds = Vector3.one;
    public float Priority = byte.MaxValue;
    
    private Transform _child;
    
    public override void Compile(SchematicBlockData block)
    {
        block.Properties = new Dictionary<string, object>
        {
            { "Bounds", (SerializableVector)Bounds },
            { "Priority", Priority }
        };
		
        base.Compile(block);
    } 

    public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
    {
        WaypointComponent waypoint = Create<WaypointComponent>("Assets/Resources/Blocks/Waypoint.prefab");
        waypoint.Bounds = block.Properties["Bounds"] as SerializableVector;
        waypoint.Priority = Convert.ToSingle(block.Properties["Priority"]);
        
        base.Decompile(ref gameObject, block, parent);
    }
	
    private void Awake()
    {
        _child = transform.GetChild(0);
        _child.hideFlags = HideFlags.HideInHierarchy;

        _child.TryGetComponent(out _filter);
        _child.TryGetComponent(out _renderer);
    }

    private void Update()
    {
        _filter.hideFlags = HideFlags.HideInInspector;
        _renderer.hideFlags = HideFlags.HideInInspector;
        
        _child.localScale = Bounds;
    }

    private MeshFilter _filter;
    private MeshRenderer _renderer;

}