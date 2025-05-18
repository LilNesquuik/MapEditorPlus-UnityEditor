using System.Collections.Generic;
using UnityEngine;

public class WorkstationComponent : SchematicBlock
{
    [Tooltip("Whether the workstation should be interactable in game. If set to false it won't be enabled in game.")]
    public bool IsInteractable = true;

    public override BlockType BlockType => BlockType.Workstation;

    public override bool Compile(SchematicBlockData block, Schematic schematic)
    {
        block.BlockType = BlockType.Workstation;
        block.Properties = new Dictionary<string, object>
        {
            { "IsInteractable", IsInteractable },
        };

        return true;
    }
}

