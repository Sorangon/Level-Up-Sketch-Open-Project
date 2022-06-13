using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Settings = SorangonToolset.UrpFog.FogRenderFeature.Settings;

namespace SorangonToolset.UrpFog {
	public class FogRenderPass : ScriptableRenderPass {
		#region Construction
		public FogRenderPass(Settings settings) {
			m_Settings = settings;
			if (m_FogMaterial == null) {
				m_FogMaterial = CoreUtils.CreateEngineMaterial("Hidden/SorangonToolset/Fog");
			}
		}
		#endregion


		#region Currents
		private Settings m_Settings;
		private Material m_FogMaterial;
		private Material m_VolumetricMaterial;

		private RenderTargetIdentifier m_Source;
		private RenderTargetIdentifier m_Destination;

		private int destinationRTId = Shader.PropertyToID("_TempTarget");
		private Fog m_Component = null;

		//Shader properties
		private static int m_FogDensityColorProp = Shader.PropertyToID("_FogDensity");
		private static int m_FogDistanceParamsProp = Shader.PropertyToID("_FogDistanceParams");
		private static int m_FogHeightParamsProp = Shader.PropertyToID("_FogHeightParams");
		private static int m_FogStartColorProp = Shader.PropertyToID("_FogStartColor");
		private static int m_FogEndColorProp = Shader.PropertyToID("_FogEndColor");
		#endregion


		#region Setup

		private void SetFogParameters(Material mat) {
			mat.SetFloat(m_FogDensityColorProp, m_Component.density.value);

			//Distance parameters
			float startDist = m_Component.startDistance.value;
			Vector4 distanceParams = new Vector4(
				startDist,
				m_Component.endDistance.value - startDist,
				0f,
				0f
			);
			mat.SetVector(m_FogDistanceParamsProp, distanceParams);

			//Height Parameters
			float startHeight = m_Component.heightStart.value;
			Vector4 heightParams = new Vector4(
				startHeight,
				m_Component.heightEnd.value - startHeight,
				0f,
				0f
			);
			mat.SetVector(m_FogHeightParamsProp, heightParams);
		}
		
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
			m_Component = VolumeManager.instance.stack.GetComponent<Fog>();
			
			//Check if volumetric must be calculated
			
			//Base fog setup
			SetFogParameters(m_FogMaterial);
			m_FogMaterial.SetColor(m_FogStartColorProp, m_Component.startColor.value);
			m_FogMaterial.SetColor(m_FogEndColorProp, m_Component.endColor.value);
		}
		#endregion


		#region Camera Callbacks
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
			var renderer = renderingData.cameraData.renderer;
			m_Source = renderer.cameraColorTarget;

			RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
			cmd.GetTemporaryRT(destinationRTId, desc);
			m_Destination = new RenderTargetIdentifier(destinationRTId);
		}
		#endregion


		#region Render
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
			if (renderingData.cameraData.cameraType == CameraType.Preview || m_Component.density.value <= 0) return;
			
			DoRenderFog(context);
		}

		private void DoRenderFog(ScriptableRenderContext context) {
			CommandBuffer cmd = CommandBufferPool.Get();
			
			cmd.Blit(m_Source, m_Destination, m_FogMaterial, 0);
			cmd.Blit(m_Destination, m_Source);
			context.ExecuteCommandBuffer(cmd);
			
			CommandBufferPool.Release(cmd);
		}
		#endregion
	}
}