Shader "Hidden/SorangonToolset/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainLightVolumetricMask ("Main Light Volumetric Mask", 2D) = "black" {}
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always ZWrite Off Cull Off
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "../Shaders/Fog.hlsl"
            ENDHLSL
        }
    }
}
