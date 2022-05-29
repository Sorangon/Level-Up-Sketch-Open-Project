using UnityEngine;

[ExecuteAlways]
public class ColorShaderProperty : ShaderProperty
{
	[SerializeField, ColorUsage(true, true)] private Color _value;

	protected override void ApplyProperty() {
		_sharedPropertyBlock.SetColor(_propertyId, _value);
	}
}
