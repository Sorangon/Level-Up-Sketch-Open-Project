using UnityEngine;

[ExecuteAlways]
public class PositionShaderProperty : ShaderProperty {
	[SerializeField] private Transform _position = null;

	private void LateUpdate() {
		ApplyProperty();
	}

	protected override void ApplyProperty() {
		Vector3 pos = _position.position;
		Debug.Log(pos);
		_sharedPropertyBlock.SetVector(_propertyId, new Vector4(pos.x, pos.y, pos.z, 0f));
	}
}
