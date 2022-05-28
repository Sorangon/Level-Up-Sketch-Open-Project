using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataPatternGenerator))]
public class DataPatternGeneratorEditor : Editor {
	private const int MAX_SOURCE_POINTS = 50; 
	
	private static Vector3[] _latestSourcePoints = new Vector3[MAX_SOURCE_POINTS];
	private static int _latestSourcePointsCount = -1;
	
	private static Vector3[] _currentSourcePoints = new Vector3[MAX_SOURCE_POINTS];
	private static int _currentSourcePointsCount = -1;

	private void OnEnable() {
		var generator = target as DataPatternGenerator;
		if (generator.Source == null) return;
		_currentSourcePointsCount = generator.Source.GetPositions(_currentSourcePoints);
		CopySourcePoints(generator.Source);
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		var generator = target as DataPatternGenerator;
		if (generator.Source == null) return;
		var source = generator.Source; 
		
		if (source.positionCount > MAX_SOURCE_POINTS) {
			source.positionCount = MAX_SOURCE_POINTS;
		}

		
		if (!generator._autoGenerate) {
			EditorGUILayout.Space();
			if (GUILayout.Button("Generate")) {
				generator.Generate();
			}
		} else {
			_currentSourcePointsCount = source.GetPositions(_currentSourcePoints);
			if (HasSourceChanged()) {
				generator.Generate();
				CopySourcePoints(source);
			}
		}

		if (GUILayout.Button("Bake")) {
			generator.Bake();
		}
	}

	private void CopySourcePoints(LineRenderer source) {
		
	}

	private bool HasSourceChanged() {
		if (_currentSourcePoints != _latestSourcePoints) {
			return true;
		}

		for (int i = 0; i < _currentSourcePointsCount; i++) {
			if (_currentSourcePoints[i] != _latestSourcePoints[i]) {
				return true;
			}
		}

		return false;
	}
	
}