using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
		FontStyles fontStyle = _textMesh.fontStyle;
		Color color = _textMesh.color;
	
		text = fontStyle.HasFlag(FontStyles.Bold) ? $"<b>{text}</b>" : text;
		text = fontStyle.HasFlag(FontStyles.Italic) ? $"<i>{text}</i>" : text;
		text = fontStyle.HasFlag(FontStyles.Strikethrough) ? $"<s>{text}</s>" : text;
		text = fontStyle.HasFlag(FontStyles.Underline) ? $"<u>{text}</u>" : text;
		text = fontStyle.HasFlag(FontStyles.UpperCase) ? $"<uppercase>{text}</uppercase>" : text;
		text = fontStyle.HasFlag(FontStyles.LowerCase) ? $"<lowercase>{text}</lowercase>" : text;
		text = fontStyle.HasFlag(FontStyles.SmallCaps) ? $"<smallcaps>{text}</smallcaps>" : text;
		text = fontStyle.HasFlag(FontStyles.Superscript) ? $"<sup>{text}</sup>" : text;
		text = fontStyle.HasFlag(FontStyles.Subscript) ? $"<sub>{text}</sub>" : text;
		text = fontStyle.HasFlag(FontStyles.Highlight) ? $"<highlight>{text}</highlight>" : text;
		text = color != Color.white ? $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>" : text;
		
		block.Properties = new Dictionary<string, object>
		{
			{ "Text", text },
			{ "DisplaySize", (SerializableVector)_textMesh.rectTransform.sizeDelta }
		};

		base.Compile(block);
	}

	public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
	{
		TMP_Text tmp = Create<GameObject>("Assets/Resources/Blocks/Text.prefab").GetComponent<TMP_Text>();
		gameObject = tmp.gameObject;

		string text = Convert.ToString(block.Properties["Text"]);
		FontStyles fontStyle = ExtractFromRichText(text, out string sanitized);
		
		tmp.text = sanitized;
		tmp.fontStyle = fontStyle;
		tmp.rectTransform.sizeDelta = JsonConvert.DeserializeObject<Vector2>(block.Properties["DisplaySize"].ToString());

		base.Decompile(ref gameObject, block, parent);
	}

	private static FontStyles ExtractFromRichText(string text, out string sanitized)
	{
		FontStyles styles = FontStyles.Normal;
		
		if (string.IsNullOrEmpty(text))
		{
			sanitized = string.Empty;
			return styles;
		}

		if (text.Contains("<b>"))
			styles |= FontStyles.Bold;

		if (text.Contains("<i>"))
			styles |= FontStyles.Italic;

		if (text.Contains("<u>"))
			styles |= FontStyles.Underline;

		if (text.Contains("<s>"))
			styles |= FontStyles.Strikethrough;
		
		sanitized = Regex.Replace(text, @"<\/?(b|i|u|s)>", string.Empty);
		return styles;
	}
}
