using UnityEngine;

[ExecuteAlways]
public class FloatShaderProperty : MonoBehaviour {
	[SerializeField] private string _propertyId;
	[SerializeField] private float _value;

	[SerializeField] private SharedPropertyBlock _sharedPropertyBlock = null;

	private void OnDidApplyAnimationProperties() {
		_sharedPropertyBlock.SetFloat(_propertyId, _value);
	}
}