//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

using UnityEngine;

namespace SorangonToolset.ArtToolset {

    /// <summary>
    /// An asset that generate a ramp texture from a gradient
    /// </summary>
    [CreateAssetMenu(menuName = "Art Toolset/Generated Texture/Ramp Texture", fileName = "NewRampTexture", order = 800)]
    public class RampTexture : CurveTexture {
        #region Settings
        [SerializeField, GradientUsage(true)] public Gradient ramp = new Gradient();
        #endregion

        #region Texture
        protected override TextureFormat GetTextureFormat() {
            return TextureFormat.RGBAFloat;
        }

        public override Color SampleTexture1D(float ratio) {
            return ramp.Evaluate(ratio);
        }
        #endregion
    }
}
