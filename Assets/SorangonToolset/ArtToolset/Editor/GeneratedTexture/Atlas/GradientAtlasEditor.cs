//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset.Editors {
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GradientAtlas))]
    public class GradientAtlasEditor : GeneratedTextureEditor {
        #region Currents
        private SerializedProperty m_CurvesProp = null;
        private SerializedProperty m_ResolutionProp = null;
        private bool m_IsAtlastTooSmall = false;

        private static readonly string[] m_AvailableResolutions = {
            "16",
            "32",
            "64",
            "128",
            "256",
            "512",
        };
        #endregion

        #region Callbacks
        protected override void OnEnable() {
            base.OnEnable();
            CheckIfAtlasTooSmall();
        }

        protected override void FindProperties() {
            base.FindProperties();
            m_CurvesProp = serializedObject.FindProperty("m_Gradients");
            m_ResolutionProp = serializedObject.FindProperty("m_Resolution");
        }

        protected override void DrawInspector() {
            if (m_IsAtlastTooSmall) {
                EditorGUILayout.HelpBox("Your atlas resolution is too small to generate all textures. " +
                    "You must create another atlast or increase the resolution of this one", MessageType.Warning);
            }

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_CurvesProp);
            int currentResValue = m_ResolutionProp.intValue;
            int defaultId;
            if (currentResValue < 16) {
                defaultId = 0;
            } else {
                int closestPowerOfTwo = Mathf.ClosestPowerOfTwo(currentResValue);
                defaultId = Mathf.RoundToInt(Mathf.Log(closestPowerOfTwo, 2)) - 4; //We add 4 because the first array choice is the power of 4
            }

            int id = EditorGUILayout.Popup("Atlas Resolution", defaultId, m_AvailableResolutions);
            m_ResolutionProp.intValue = Mathf.RoundToInt(Mathf.Pow(2, id + 4));

            if (EditorGUI.EndChangeCheck()) {
                CheckIfAtlasTooSmall();
                SetComputeFlagUp();
            }

            serializedObject.ApplyModifiedProperties();
            base.DrawInspector();
        }
        #endregion

        #region Utility
        private void CheckIfAtlasTooSmall() {
            m_IsAtlastTooSmall = m_CurvesProp.arraySize > m_ResolutionProp.intValue;
        }
        #endregion
    }
}