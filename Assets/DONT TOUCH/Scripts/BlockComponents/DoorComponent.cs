using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DoorComponent : SchematicBlock
{
    [Tooltip("The type of door.")]
    public DoorType DoorType = DoorType.Lcz;
    
    [Tooltip("Whether the door is open by default.")]
    public bool IsOpen;

    [Tooltip("Whether the door is locked by default.")]
    public bool IsLocked;
    
    [Tooltip("The permissions required to open the door.")]
    public DoorPermissionFlags RequiredPermissions = DoorPermissionFlags.None;
    
    [Tooltip("Whether all permissions are required to open the door.")]
    public bool RequireAll = true;

    public override BlockType BlockType => DoorType switch
    {
        DoorType.Lcz => BlockType.LczDoor,
        DoorType.Hcz => BlockType.HczDoor,
        DoorType.Ez => BlockType.EzDoor,
        DoorType.HeavyBulkDoor => BlockType.HeavyBulkDoor,
        _ => throw new ArgumentOutOfRangeException(nameof(DoorType), DoorType, null)
    };

    public override void Compile(SchematicBlockData block)
    {
        block.Properties = new Dictionary<string, object>
        {
            { "DoorType", DoorType },
            { "IsOpen", IsOpen },
            { "IsLocked", IsLocked },
            { "RequiredPermissions", RequiredPermissions },
            { "RequireAll", RequireAll }
        };

        base.Compile(block);
    }

    public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
    {
        DoorComponent workstationComponent = Create<DoorComponent>("Assets/Resources/Blocks/Door.prefab");
        gameObject = workstationComponent.gameObject;

        workstationComponent.DoorType = block.Properties.TryGetValue("DoorType", out object doorType) ? (DoorType)Enum.Parse(typeof(DoorType), doorType.ToString()) : DoorType.Lcz;
        workstationComponent.IsOpen = block.Properties.TryGetValue("IsOpen", out object isOpen) && Convert.ToBoolean(isOpen);
        workstationComponent.IsLocked = block.Properties.TryGetValue("IsLocked", out object isLocked) && Convert.ToBoolean(isLocked);
        workstationComponent.RequiredPermissions = block.Properties.TryGetValue("RequiredPermissions", out object requiredPermissions) ? (DoorPermissionFlags)Enum.Parse(typeof(DoorPermissionFlags), requiredPermissions.ToString()) : DoorPermissionFlags.None;
        workstationComponent.RequireAll = block.Properties.TryGetValue("RequireAll", out object requireAll) && Convert.ToBoolean(requireAll);

        base.Decompile(ref gameObject, block, parent);
    }
    
    
    private DoorType _prevDoorType = DoorType.Lcz;

    private void Update()
    {
        if (_prevDoorType == DoorType)
            return;
        
        GetByDoorType.SetActive(false);
        
        _prevDoorType = DoorType;
        
        GetByDoorType.SetActive(true);
    }

    public GameObject GetByDoorType
    {
        get
        {
            switch (_prevDoorType)
            {
                case DoorType.Lcz:
                    return transform.Find("Lcz").gameObject;
                case DoorType.Hcz:
                    return transform.Find("Hcz").gameObject;
                case DoorType.Ez:
                    return transform.Find("Ez").gameObject;
                case DoorType.HeavyBulkDoor:
                    return transform.Find("HeavyBulkDoor").gameObject;
                default:
                    return transform.Find("Lcz").gameObject;
            }   
        }
    }
}