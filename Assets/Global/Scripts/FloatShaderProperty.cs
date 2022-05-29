using UnityEngine;

[ExecuteAlways]
public class FloatShaderProperty : ShaderProperty {
	[SerializeField] private float _value;

	protected override void ApplyProperty() {
		_sharedPropertyBlock.SetFloat(_propertyId, _value);
	}
}