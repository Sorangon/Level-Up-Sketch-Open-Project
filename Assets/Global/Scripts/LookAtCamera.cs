using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class LookAtCamera : MonoBehaviour {
	private void LateUpdate() {
		Vector3 target;

		Vector3 GetCameraPosition() {
			return Camera.main.transform.position;
		}
		
		if (Application.isPlaying) {
			target = GetCameraPosition();
		} else {
#if UNITY_EDITOR
			var lastView = SceneView.lastActiveSceneView;
			if (lastView) {
				target = lastView.camera.transform.position;
			} else {
				target = GetCameraPosition();
			}
#endif
			target = GetCameraPosition();
		}
		
		transform.rotation = Quaternion.LookRotation(transform.position - target);
	}
}