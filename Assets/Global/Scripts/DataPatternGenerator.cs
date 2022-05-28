using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class DataPatternGenerator : MonoBehaviour {
	[System.Serializable]
	private class NoiseLayer {
		[SerializeField, Min(0.005f)] private Vector2 _frequencyRange = new Vector2(0.2f, 0.5f);
		[SerializeField] private Vector2 _amplitudeRange = new Vector2(0.1f, 0.3f);

		/// <summary>
		/// Generate a new point and return next distance reached
		/// </summary>
		/// <returns></returns>
		public float GetNextPointDistance() {
			return RandomRange(_frequencyRange.x, _frequencyRange.y);
		}

		public Vector3 GetRandomPosition() {
			Vector3 randomVec = new Vector3(
				RandomRange(-1f, 1f),
				RandomRange(-1f, 1f),
				RandomRange(-1f, 1f)
			);
			return randomVec.normalized * RandomRange(_amplitudeRange.x, _amplitudeRange.y);
		}
	}


	[SerializeField] private NoiseLayer _layer = new NoiseLayer();

	[Space]
	[SerializeField] private AnimationCurve _weightOverLength = AnimationCurve.Constant(0f, 1f, 1f);
	[SerializeField, Min(0f)] private float _width = 0.05f;

	[Space]
	[SerializeField] private int _seed = 150;
	[SerializeField] private bool _autoGenerate = true;

	[SerializeField] private LineRenderer _source = null;
	[SerializeField] private LineRenderer _target = null;

	private static int _currentSeed = -15000;

	[ContextMenu("Generate")]
	private void Generate() {
		if (_target == null) {
			CreateTarget();
		}

		_currentSeed = _seed;

		List<Vector3> points = new List<Vector3>();

		int pointsCount = _source.positionCount;
		Vector3[] sourcePoints = new Vector3[pointsCount];
		_source.GetPositions(sourcePoints);
		float maxDistance = 0f;

		float remainingDistance = 0f;
		Vector3 nextPos = sourcePoints[0];
		for (int i = 0; i < pointsCount - 1; i++) {
			Vector3 a = sourcePoints[i];
			Vector3 b = sourcePoints[i + 1];
			float distance = Vector3.Magnitude(b - a);
			float currentDistance = remainingDistance;
			do {
				points.Add(nextPos);
				Vector3 originPos = Vector3.Lerp(a, b, currentDistance / distance);
				nextPos = originPos + _layer.GetRandomPosition();
				currentDistance += _layer.GetNextPointDistance();
			} while (currentDistance < distance);

			remainingDistance = currentDistance - distance;
		}

		points.Add(sourcePoints[^1]);
		_target.positionCount = points.Count;
		_target.SetPositions(points.ToArray());
		_target.widthMultiplier = _width;
	}

	private void OnValidate() {
		if (_source == null) {
			_source = GetComponent<LineRenderer>();
			_source.useWorldSpace = false;
		}

		_source.widthMultiplier = 0f;

		if (_autoGenerate) {
			Generate();
		}
	}

	private void CreateTarget() {
		var go = new GameObject("Generated");
		go.transform.SetParent(transform, false);
		go.transform.localPosition = Vector3.zero;
		_target = go.AddComponent<LineRenderer>();
		_target.useWorldSpace = false;
	}

	private static float RandomRange(float min, float max) {
		Random.InitState(_currentSeed);
		float rand = Random.Range(min, max);
		_currentSeed++;
		return rand;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Vector3 origin = transform.position; 
		for (int i = 0; i < _source.positionCount - 1; i++) {
			Gizmos.DrawLine(_source.GetPosition(i) + origin, _source.GetPosition(i + 1) + origin);
		}
	}
}