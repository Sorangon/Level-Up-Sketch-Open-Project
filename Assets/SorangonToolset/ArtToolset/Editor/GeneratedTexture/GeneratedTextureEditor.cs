//Julien Delaunay(Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset.Editors {
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// The base class for Generated textures custom editors 
    /// </summary>
    public abstract class GeneratedTextureEditor : Editor {
        #region Constants
        private const float MAX_PREVIEW_HEIGHT = 400f;
        private const float PREVIEW_PADDING = 20f;
        #endregion

        #region Enums
        public enum BakeFormat {
            EXR,
            JPEG,
            PNG
        }
        #endregion

        /// <summary>
        /// If this value is negative, the height will not be overriden
        /// </summary>
        protected virtual float m_OverridenPreviewTextureHeight => -1;

        #region Currents
        private MethodInfo m_ComputeTextureMethod = null;
        private FieldInfo m_GeneratedTextureField = null;
        private SerializedProperty m_RecalculateOnLoad = null;
        private bool m_ComputeTextureFlag = false;
        private Texture2D m_CurrentTexture = null;
        private Vector2 m_LastRectSize = Vector2.zero;
        #endregion

        #region Callbacks
        protected virtual void OnEnable() {
            FindProperties();
            m_ComputeTextureMethod = typeof(GeneratedTexture).GetMethod("ComputeTexture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_GeneratedTextureField = typeof(GeneratedTexture).GetField("m_Texture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_RecalculateOnLoad = serializedObject.FindProperty("m_RecalculateOnLoad");

            m_CurrentTexture = m_GeneratedTextureField.GetValue(target) as Texture2D;

            Undo.undoRedoPerformed += OnPerformUndoRedo;
        }

        protected virtual void OnDisable() {
            Undo.undoRedoPerformed -= OnPerformUndoRedo;
        }

        public override sealed void OnInspectorGUI() {
            serializedObject.Update();

            DrawInspector();

            if (m_ComputeTextureFlag) {
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                m_ComputeTextureMethod.Invoke(target, null);
                m_ComputeTextureFlag = false;
            }

            //Separator
            GUILayout.Space(20f);
            GUILayout.Box(new GUIContent(), GUILayout.Height(3f), GUILayout.Width(EditorGUIUtility.currentViewWidth - 25f));
            GUILayout.Space(6f);

            DrawBakeButtons();
            GUILayout.Space(6f);
            EditorGUILayout.PropertyField(m_RecalculateOnLoad);

            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(20);

            DrawPreview();
        }

        protected virtual void DrawInspector() { }
        #endregion

        #region Initialization
        protected virtual void FindProperties() { }
        #endregion

        #region Texture
        /// <summary>
        /// Create or just rename a target generated texture
        /// </summary>
        /// <param name="gt"></param>
        internal static void SetupTextureAsset(GeneratedTexture gt) {
            string path = AssetDatabase.GetAssetPath(gt);

            //Checks if the texture subobject already exists
            Texture2D textureObject = null;
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i = 0; i < assets.Length; i++) {
                if (assets[i] is Texture2D) {
                    textureObject = assets[i] as Texture2D;
                    break;
                }
            }

            if (textureObject == null) {
                //Create a new one
                textureObject = gt.GetTexture(true);
                AssetDatabase.AddObjectToAsset(textureObject, gt);
            }

            textureObject.name = gt.name + "_Texture";
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        protected void SetComputeFlagUp() {
            m_ComputeTextureFlag = true;
        }
        #endregion

        #region Panels
        private void DrawBakeButtons() {
            if (GUILayout.Button("Bake PNG")) {
                BakeTexture(BakeFormat.PNG);
            }

            if (GUILayout.Button("Bake JPEG")) {
                BakeTexture(BakeFormat.JPEG);
            }

            if (GUILayout.Button("Bake EXR")) {
                BakeTexture(BakeFormat.EXR);
            }
        }
        #endregion

        #region Bake
        private void BakeTexture(BakeFormat format) {
            //Get save path
            string extension = format.ToString();
            string projectPath = EditorUtility.SaveFilePanelInProject("Bake texture" + extension, target.name, extension.ToLower(), "Bake the texture in " + extension + " format");
            if (projectPath.Length <= 0) return;
            string path = projectPath.Remove(0, 6); //Remove "Assets" characters
            path = Application.dataPath + path; //Final path


            //Encode texture
            byte[] encodedTexture = { };
            Texture2D srcTex = ((GeneratedTexture)target).GetTexture(true);
            switch (format) {
                case BakeFormat.PNG:
                    encodedTexture = srcTex.EncodeToPNG();
                    break;
                case BakeFormat.JPEG:
                    encodedTexture = srcTex.EncodeToJPG();
                    break;
                case BakeFormat.EXR:
                    encodedTexture = srcTex.EncodeToEXR();
                    break;
            }

            //Save texture
            File.WriteAllBytes(path, encodedTexture);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Select new texture
            Texture newTex = AssetDatabase.LoadAssetAtPath(projectPath, typeof(Texture2D)) as Texture2D;
            EditorGUIUtility.PingObject(newTex);
            Selection.activeObject = newTex;
        }
        #endregion

        #region Undo
        private void OnPerformUndoRedo() {
            SetComputeFlagUp();
        }
        #endregion

        #region Utility
        private void DrawPreview() {
            EditorGUILayout.BeginVertical("HelpBox");
            if (m_CurrentTexture == null) {
                EditorGUILayout.HelpBox("Cannot display texture. Ensure this one is generated correrctly !", MessageType.Error);
                return;
            }

            EditorGUILayout.LabelField("Preview", new GUIStyle("Label") { alignment = TextAnchor.MiddleCenter, fontSize = 14, fontStyle = FontStyle.Bold });

            Rect lastRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint) {
                if (m_OverridenPreviewTextureHeight > 0) {
                    m_LastRectSize.x = lastRect.width;
                    m_LastRectSize.y = m_OverridenPreviewTextureHeight + PREVIEW_PADDING;
                } else {
                    m_LastRectSize.y = lastRect.width;
                    if (m_LastRectSize.y > MAX_PREVIEW_HEIGHT) {
                        m_LastRectSize.y = MAX_PREVIEW_HEIGHT;
                        m_LastRectSize.x = MAX_PREVIEW_HEIGHT;
                    } else {
                        m_LastRectSize.x = lastRect.width;
                    }
                }

                Rect newRect = new Rect(lastRect.x + PREVIEW_PADDING * 0.5f + Mathf.Max(lastRect.width - m_LastRectSize.x, 0f) * 0.5f
                    , lastRect.y + 25, m_LastRectSize.x - PREVIEW_PADDING, m_LastRectSize.y - PREVIEW_PADDING);

                EditorGUI.DrawPreviewTexture(newRect, m_CurrentTexture);
            }

            GUILayout.Space(m_LastRectSize.y);

            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}