using TMPro;
using UnityEngine;

/// <summary>
/// Write a filled text int a target UI Text field
/// </summary>
[ExecuteAlways]
public class TextAnimator : MonoBehaviour {
	[SerializeField, Range(0f, 1f)] private float _drawRatio = 1.0f;
	[SerializeField, Min(1)] private int _loops = 1;
	[SerializeField] private string _text = "Hello world";

	[Space]
	[SerializeField] private TextMeshProUGUI _textMesh = null;

	private void OnDidApplyAnimationProperties() {
		UpdateText();
	}

	private void OnValidate() {
		UpdateText();
	}

	private void UpdateText() {
		float currentT = (_drawRatio * _loops);
		if (currentT > 1.0) {
			currentT %= 1.0f;
		}
		
		int charCount = Mathf.RoundToInt(_text.Length * currentT);
			
		if (charCount > 0) {
			string text = _text.Substring(0,charCount);
			_textMesh.text = text;
		}else {
			_textMesh.text = string.Empty;
		}
	}
}