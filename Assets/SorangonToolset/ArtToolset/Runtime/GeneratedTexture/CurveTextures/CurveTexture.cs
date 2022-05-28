//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    /// <summary>
    /// A generated texture that only computed from a value on one dimension
    /// </summary>
    public abstract class CurveTexture : GeneratedTexture{
        #region Enums
        public enum Mapping {
            Horizontal,
            Vertical
        }
        #endregion

        #region Data
        [SerializeField] protected Mapping m_mapping = Mapping.Horizontal;
        [SerializeField] protected bool m_invert = false;
        #endregion

        #region Texture
        protected override void ComputeTexture() {
            int pixelCount = m_mapping == Mapping.Horizontal ? m_Texture.width : m_Texture.height;
            for(int i = 0; i < pixelCount; i++) {
                float ratio = (float)i / (float)pixelCount;

                if(m_invert) {
                    ratio = 1 - ratio;
                }

                if(m_mapping == Mapping.Horizontal) {
                    m_Texture.SetPixel(i, 0, SampleTexture1D(ratio));
                } else {
                    m_Texture.SetPixel(0, i, SampleTexture1D(ratio));
                }
            }

            m_Texture.Apply();
        }

        protected override Texture2D CreateTexture() {
            int xDim = m_mapping == Mapping.Horizontal ? 128 : 1;
            int yDim = m_mapping == Mapping.Horizontal ? 1 : 128;

            return new Texture2D(xDim, yDim, GetTextureFormat(), false) {
                wrapMode = TextureWrapMode.Clamp
            };
        }

        protected abstract TextureFormat GetTextureFormat();
        public abstract Color SampleTexture1D(float ratio);
        #endregion
    }
}