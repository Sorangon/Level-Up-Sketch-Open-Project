using UnityEngine;

[ExecuteAlways]
public abstract class ShaderProperty : MonoBehaviour {
	[SerializeField] protected string _propertyId;
	[SerializeField] protected SharedPropertyBlock _sharedPropertyBlock = null;

	private void OnDidApplyAnimationProperties() {
		ApplyProperty();
	}

	protected abstract void ApplyProperty();
}
