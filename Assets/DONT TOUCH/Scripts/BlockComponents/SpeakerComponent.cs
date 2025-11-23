using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SpeakerComponent : SchematicBlock
{
    public override BlockType BlockType => BlockType.Speaker;
    
    private Transform _child;
    
    public override void Compile(SchematicBlockData block)
    {
        block.Properties = new Dictionary<string, object>
        {
            { "Clips", _audioSource.clip?.name },
            { "MinDistance", _audioSource.minDistance },
            { "MaxDistance", _audioSource.maxDistance },
            { "Volume", _audioSource.volume },
            { "Loop", _audioSource.loop }
        };
		
        base.Compile(block);
    } 

    public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
    {
        SpeakerComponent speakerComponent = Create<SpeakerComponent>("Assets/Resources/Blocks/Speaker.prefab");
        AudioSource audioSource = speakerComponent.GetComponent<AudioSource>();
        
        audioSource.clip = Resources.Load<AudioClip>(block.Properties["Clips"] as string);
        audioSource.minDistance = Convert.ToSingle(block.Properties["MinDistance"]);
        audioSource.maxDistance = Convert.ToSingle(block.Properties["MaxDistance"]);
        audioSource.volume = Convert.ToSingle(block.Properties["Volume"]);
        audioSource.loop = Convert.ToBoolean(block.Properties["Loop"]);
        
        base.Decompile(ref gameObject, block, parent);
    }
	
    private void Start()
    {
        transform.TryGetComponent(out _audioSource);
    }

    private AudioSource _audioSource;

}