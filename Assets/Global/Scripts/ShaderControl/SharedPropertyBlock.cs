using UnityEngine;

[ExecuteAlways]
public class SharedPropertyBlock : MonoBehaviour {
	#region Settings
	[SerializeField] private Renderer[] _renderers = {};
	#endregion


	#region Currents
	private MaterialPropertyBlock _materialPropertyBlock;
	#endregion

	
	#region Callbacks
	private void OnEnable() {
		_materialPropertyBlock = new MaterialPropertyBlock();
	}

	public void SetFloat(string propertyId, float value) {
		_materialPropertyBlock.SetFloat(propertyId, value);
		ApplyProperties();
	}
	
	public void SetColor(string propertyId, Color value) {
		_materialPropertyBlock.SetColor(propertyId, value);
		ApplyProperties();
	}
	
	public void SetVector(string propertyId, Vector4 value) {
		_materialPropertyBlock.SetVector(propertyId, value);
		ApplyProperties();
	}
	
	public void ApplyProperties() {
		for (int i = 0; i < _renderers.Length; i++) {
			_renderers[i].SetPropertyBlock(_materialPropertyBlock);
		}
	}
	#endregion
}