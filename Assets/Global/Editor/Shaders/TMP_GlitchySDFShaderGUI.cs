using UnityEngine;
using UnityEditor;

namespace TMPro.EditorUtilities
{
    public class TMP_GlitchySDFShaderGUI : TMP_SDFShaderGUI {

        private static bool s_Glitch = false;
        
        protected override void DoGUI()
        {
            base.DoGUI();
            
            s_Glitch = BeginPanel("Glitch", s_Glitch);
            if (s_Glitch) {
                DoGlitchPanel();
            }
            EndPanel();
            EditorGUILayout.LabelField("Test");    
        }

        protected void DoGlitchPanel() {
            EditorGUI.indentLevel += 1;
            DoSlider("_GlitchVertexDisplacementAmount", "Vertex Displacement Amount");

            EditorGUILayout.Space();
            DoSlider("_GlitchTextDeformAmount", "Text Deform Amount");
            DoVector2("_GlitchTextDeformTiling", "Text Deform Tiling");

            EditorGUILayout.Space();
            DoSlider("_GlitchColorAmount", "Color Amount");
            DoFloat("_GlitchNoisePixelate", "Color Noise Pixelate");
            DoTexture2D("_GlitchColorMap", "Color Map", false);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
        }
    }
}
