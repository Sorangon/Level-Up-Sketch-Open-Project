using UnityEngine.Rendering.Universal;

namespace SorangonToolset.UrpFog {
	public class FogRenderFeature : ScriptableRendererFeature {
		#region Settings
		public Settings settings = new Settings();
		#endregion


		#region Classes
		[System.Serializable]
		public class Settings {
		    public bool renderAfterTransparents = true;
		}
		#endregion


		#region Currents
		private FogRenderPass m_Pass;
		#endregion


		public override void Create() {
			m_Pass = new FogRenderPass(settings);
			m_Pass.renderPassEvent = settings.renderAfterTransparents ?  
				RenderPassEvent.AfterRenderingTransparents : 
				RenderPassEvent.BeforeRenderingTransparents;
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
			renderer.EnqueuePass(m_Pass);
		}
	}
}