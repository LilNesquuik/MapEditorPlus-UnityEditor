using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[ExecuteInEditMode]
public class TriggerComponent : SchematicBlock
{
	public override BlockType BlockType => BlockType.Trigger;
	
	[Tooltip("Specifies when the trigger should activate (e.g., OnEnter, OnExit, etc.).")]
	public TriggerType triggerType = TriggerType.OnEnter;

	[Tooltip("The name of the effect to apply when the trigger is activated (e.g., PitDeath, Heal, etc.).")]
	public string effectName = "PitDeath";

	[Tooltip("Effect intensity. 255 represents the maximum possible intensity.")]
	[Range(0, 255)]
	public byte intensity = byte.MaxValue;

	[Tooltip("Effect duration in seconds.")]
	public float duration = 1f;

	[Tooltip("If enabled, the effect duration will be added instead of replaced when reapplied.")]
	public bool addDuration = false;
	
	public override void Compile(SchematicBlockData block)
	{
		block.Properties = new Dictionary<string, object>
		{
			{ "TriggerType", triggerType },
			{ "EffectName", effectName },
			{ "Intensity", intensity },
			{ "Duration", duration },
			{ "AddDuration", addDuration }
		};

		base.Compile(block);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
	{
		TriggerComponent cullingComponent = Create<TriggerComponent>("Assets/Resources/Blocks/Trigger.prefab");
		cullingComponent.triggerType = (TriggerType)Convert.ToByte(block.Properties["TriggerType"]);
		cullingComponent.effectName = block.Properties["EffectName"].ToString();
		cullingComponent.intensity = Convert.ToByte(block.Properties["Intensity"]);
		cullingComponent.duration = Convert.ToSingle(block.Properties["Duration"]);
		cullingComponent.addDuration = Convert.ToBoolean(block.Properties["AddDuration"]);
		
		base.Decompile(ref gameObject, block, parent);
	}
}
