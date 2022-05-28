//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset.Editors {
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// The custom editor class of the Ramp Texture
    /// </summary>
    [CustomEditor(typeof(BezierCurveTexture))]
    public class BezierCurveTextureEditor : CurveTextureEditor {
        #region Styles
        private GUIContent m_curveLabelContent = new GUIContent("Curve", "The curve that will be sampled on a single channel mask texture (R)");
        #endregion

        #region Serialized Properties
        private SerializedProperty m_curveProperty = null;

        protected override void FindProperties() {
            base.FindProperties();
            m_curveProperty = serializedObject.FindProperty("curve");
        }
        #endregion

        #region Callbacks
        protected override void DrawInspector() {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_curveProperty, m_curveLabelContent);
            if(EditorGUI.EndChangeCheck()) {
                SetComputeFlagUp();
            }
            base.DrawInspector();
        }
        #endregion
    }
}