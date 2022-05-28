//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

using UnityEngine;

namespace SorangonToolset.ArtToolset {

    /// <summary>
    /// An asset that generate a ramp texture from a gradient
    /// </summary>
    [CreateAssetMenu(menuName = "Art Toolset/Generated Texture/Bezier Curve Texture", fileName = "NewCurveTexture", order = 800)]
    public class BezierCurveTexture : CurveTexture {
        #region Settings
        public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        #endregion

        #region Texture
        protected override TextureFormat GetTextureFormat() {
#if UNITY_ANDROID
            return TextureFormat.RGBA32;
#else
            return TextureFormat.R16;
#endif
        }

        public override Color SampleTexture1D(float ratio) {
            float value = curve.Evaluate(ratio);
            value = Mathf.Clamp01(value);
            return Color.white * value;
        }
#endregion
    }
}

