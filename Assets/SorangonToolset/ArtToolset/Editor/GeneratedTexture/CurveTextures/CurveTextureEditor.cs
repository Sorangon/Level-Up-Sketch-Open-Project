namespace SorangonToolset.ArtToolset.Editors {
    using UnityEditor;
    using UnityEngine;

    public class CurveTextureEditor : GeneratedTextureEditor {
        #region Serialized Properties
        private SerializedProperty m_invertProp = null;

        protected override void FindProperties() {
            base.FindProperties();
            m_invertProp = serializedObject.FindProperty("m_invert");
        }
        #endregion

        #region Properties
        protected override float m_OverridenPreviewTextureHeight => 20;
        #endregion

        #region Drawing
        protected override void DrawInspector() {
            base.DrawInspector();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_invertProp);
            if(EditorGUI.EndChangeCheck()) {
                SetComputeFlagUp();
            }
        }
        #endregion
    }
}