using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CapybaraComponent : SchematicBlock
{
    [Tooltip("Whether the capybara should have a collider.")]
    public bool Collider = true;

    public override BlockType BlockType => BlockType.Capybara;

    public override void Compile(SchematicBlockData block)
    {
        block.Properties = new Dictionary<string, object>
        {
            { "Collider", Collider },
        };

        base.Compile(block);
    }

	public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
	{
        CapybaraComponent capybaraComponent = Create<CapybaraComponent>("Assets/Resources/Blocks/Capybara.prefab");
        gameObject = capybaraComponent.gameObject;

        capybaraComponent.Collider = block.Properties.TryGetValue("Collider", out object hasCollider) && Convert.ToBoolean(hasCollider);

		base.Decompile(ref gameObject, block, parent);
	}
}

