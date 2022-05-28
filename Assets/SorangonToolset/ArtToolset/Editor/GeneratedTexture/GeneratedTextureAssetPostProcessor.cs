//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset.Editors {
    using UnityEditor;

    /// <summary>
    /// Operate when a generated texture is created
    /// </summary>
    public class GeneratedTextureAssetPostProcessor : AssetPostprocessor {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            if(importedAssets.Length > 0) {
                for(int i = 0; i < importedAssets.Length; i++) {
                    var createdGT = (GeneratedTexture)AssetDatabase.LoadAssetAtPath(importedAssets[i], typeof(GeneratedTexture));
                    if(createdGT != null) {
                        GeneratedTextureEditor.SetupTextureAsset(createdGT);
                    }
                }
            }
        }
    }
}