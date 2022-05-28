//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    public abstract class GeneratedTexture : ScriptableObject {
        #region Data
        [SerializeField] protected Texture2D m_Texture = null;
        [SerializeField] private bool m_RecalculateOnLoad = false;
        #endregion

        #region Callbacks
        private void OnEnable() {
            if(m_RecalculateOnLoad) {
                GetTexture(true);
            }
        }
        #endregion

        #region Texture
        /// <summary>
        /// Returns the generated texture
        /// </summary>
        /// <param name="recompute">Should the texture should be recomputed, in case a parameter had been changed</param>
        /// <returns></returns>
        public Texture2D GetTexture(bool recompute = false) {
            bool generateTexture = m_Texture == null;
            if(generateTexture) {
#if UNITY_EDITOR
                RegenerateTexture();
#else
                m_Texture = CreateTexture();
#endif
            }

            if(recompute || generateTexture) {
                ComputeTexture();
            }

            return m_Texture;
        }

        /// <summary>
        /// Compute each pixel of the texture and return the result
        /// </summary>
        /// <returns></returns>
        protected abstract void ComputeTexture();

        /// <summary>
        /// Generate a the texture with target dimensions 
        /// </summary>
        /// <returns></returns>
        protected abstract Texture2D CreateTexture();
        #endregion

#if UNITY_EDITOR
        private void RegenerateTexture() {
            m_Texture = CreateTexture();
        }
#endif
    }
}
