using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class GraphicElementFlash : MonoBehaviour {
	[SerializeField, Min(0f)] private Vector2 _minMaxAlpha = new Vector2(0f, 1f);
	[SerializeField, Min(1)] private int _loops = 1;
	[SerializeField, Range(0f, 1f)] private float _offset = 0f; 
	
	[Space]
	[SerializeField] private Graphic _graphic; 
	private void OnDidApplyAnimationProperties() {
		UpdateColor();
	}

	private void OnValidate() {
		UpdateColor();
	}
	
	private void UpdateColor() {
		var col = _graphic.color;
		float alpha = Mathf.Lerp(_minMaxAlpha.x, _minMaxAlpha.y, Mathf.PingPong(_offset * _loops,0.5f));
		_graphic.color = new Color(col.r, col.g, col.b, alpha);
	}

}