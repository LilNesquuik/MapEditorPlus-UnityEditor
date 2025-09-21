using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TextComponent : SchematicBlock
{
	public override BlockType BlockType => BlockType.Text;

	private TMP_Text _textMesh;
	private MeshRenderer _renderer;

	private void Awake()
	{
		TryGetComponent(out _textMesh);
		TryGetComponent(out _renderer);
	}

	private void Update()
	{
		_textMesh.margin = Vector4.zero;
		_renderer.hideFlags = HideFlags.HideInInspector;
	}

	public override void Compile(SchematicBlockData block)
	{
		string text = _textMesh.text;
		
		if (_textMesh.fontStyle.HasFlag(FontStyle.Bold))
			text = $"<b>{text}</b>";
		if (_textMesh.fontStyle.HasFlag(FontStyle.Italic))
			text = $"<i>{text}</i>";
		
		if (_textMesh.color != Color.white)
			text = $"<color=#{ColorUtility.ToHtmlStringRGB(_textMesh.color)}>{text}</color>";
		
		block.Properties = new Dictionary<string, object>
		{
			{ "Text", text },
			{ "DisplaySize", (SerializableVector)_textMesh.rectTransform.sizeDelta }
		};

		base.Compile(block);
	}

	public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
	{
		TMP_Text text = Create<GameObject>("Assets/Resources/Blocks/Text.prefab").GetComponent<TMP_Text>();
		gameObject = text.gameObject;

		text.text = Convert.ToString(block.Properties["Text"]);
		text.rectTransform.sizeDelta = JsonConvert.DeserializeObject<Vector2>(block.Properties["DisplaySize"].ToString());

		base.Decompile(ref gameObject, block, parent);
	}
}
