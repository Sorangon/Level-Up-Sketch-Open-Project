using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class DataPatternGenerator : MonoBehaviour {
	#region Classes
	[Serializable]
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
	#endregion


	#region Settings
	[SerializeField] private NoiseLayer _layer = new NoiseLayer();
	[SerializeField] private AnimationCurve _weightOverLength = AnimationCurve.Constant(0f, 1f, 1f);

	[Space]
	[SerializeField, Min(0f)] private float _width = 0.05f;

	[Space]
	[SerializeField] private int _seed = 150;
	public bool _autoGenerate = true;

	[SerializeField] private LineRenderer _source = null;
	[SerializeField] private LineRenderer _target = null;
	#endregion


	#region Properties
	public LineRenderer Source => _source;
	#endregion

	#region Currents
	private static int _currentSeed = -15000;
	private static Vector3[] _sourcePoints = new Vector3[50];
	private static List<Vector3> _noisedPoints = new List<Vector3>(400);
	private static List<Vector3> _snappedPoints = new List<Vector3>(600);
	private static int _currentRandomizedAxis = 0;
	#endregion


	#region Generation
	[ContextMenu("Generate")]
	public void Generate() {
		if (_target == null) {
			CreateTarget();
		}

		_currentSeed = _seed;
		
		_noisedPoints.Clear();
		_snappedPoints.Clear();
		int pointsCount = _source.GetPositions(_sourcePoints);

		float maxDistance = 0f;

		float remainingDistance = 0f;
		Vector3 nextPos = _sourcePoints[0];
		
		for (int i = 0; i < pointsCount - 1; i++) {
			Vector3 a = _sourcePoints[i];
			Vector3 b = _sourcePoints[i + 1];
			float distance = Vector3.Magnitude(b - a);
			float currentDistance = remainingDistance;

			void AddPoint(bool bypassWeight) {
				float ratio = currentDistance / distance;
				float globalRatio = (float)i / (float)pointsCount + ratio / (float)pointsCount;
				float weight = _weightOverLength.Evaluate(globalRatio);
				
				if (bypassWeight || weight >= 1.0f || weight > Random.value) {
					_noisedPoints.Add(nextPos);
				}
				
				Vector3 originPos = Vector3.Lerp(a, b, ratio);
				nextPos = originPos + _layer.GetRandomPosition();
				currentDistance += _layer.GetNextPointDistance();
			}
			
			AddPoint(true);

			while (currentDistance < distance) {
				AddPoint(false);
			}

			remainingDistance = currentDistance - distance;
		}

		_noisedPoints.Add(_sourcePoints[pointsCount - 1]);

		for (int i = 0; i < _noisedPoints.Count - 1; i++) {
			Vector3 a = _noisedPoints[i];
			Vector3 b = _noisedPoints[i + 1];
			_snappedPoints.Add(a);
			Vector3 dir = b - a;
			Vector3 previousPoint = Vector3.zero;

			_currentRandomizedAxis = Random.RandomRange(0, 2);
			for (int j = 0; j < 2; j++) {
				Vector3 point = dir;
				for (int k = 0; k < 3; k++) {
					if(k == _currentRandomizedAxis) continue;
					point[k] = 0f;
				}

				_currentRandomizedAxis += RandomSign();
				if (_currentRandomizedAxis > 2) {
					_currentRandomizedAxis = 0;
				}else if (_currentRandomizedAxis < 0) {
					_currentRandomizedAxis = 2;
				}

				_snappedPoints.Add(point + a + previousPoint);
				previousPoint = point;
			}
		}
		
		_snappedPoints.Add(_noisedPoints[^1]);
		
		_target.positionCount = _snappedPoints.Count;
		_target.SetPositions(_snappedPoints.ToArray());
		_target.widthMultiplier = _width;
	}

	private static float RandomRange(float min, float max) {
		Random.InitState(_currentSeed);
		float rand = Random.Range(min, max);
		_currentSeed++;
		return rand;
	}

	private static int RandomSign() {
		Random.InitState(_currentSeed);
		float random = Random.value;
		_currentSeed++;
		if (random < 0.5f) {
			return -1;
		}

		return 1;
	}
	
	private void CreateTarget() {
		var go = new GameObject("Generated");
		go.transform.SetParent(transform, false);
		go.transform.localPosition = Vector3.zero;
		_target = go.AddComponent<LineRenderer>();
		_target.useWorldSpace = false;
	}
	#endregion


	#region Editor
#if UNITY_EDITOR
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
	

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Vector3 origin = transform.position; 
		for (int i = 0; i < _source.positionCount - 1; i++) {
			Gizmos.DrawLine(_source.GetPosition(i) + origin, _source.GetPosition(i + 1) + origin);
		}
	}
#endif
	#endregion
	
	public void Bake() {
		int posCount = _target.positionCount;
		Vector3[] points = new Vector3[posCount];
		_target.GetPositions(points);
		
		_source.positionCount = posCount;
		_source.SetPositions(points);
		_source.widthMultiplier = _target.widthMultiplier;
		_source.numCapVertices = _target.numCapVertices;
		_source.numCornerVertices = _target.numCornerVertices;
		_source.sharedMaterials = _target.sharedMaterials;
		
		DestroyImmediate(_target.gameObject);
		DestroyImmediate(this);
	}
}