#ifndef FOG_INCLUDED
#define FOG_INCLUDED

#include "FogBase.hlsl"
    
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

//Fog parameters
half4 _FogEndColor;
half4 _FogStartColor;
    
float UnclampedSmoothstep(float a, float b, float t)
{
    t = (t - a)/(b - a);
    return t * t * (3.0 - 2.0 * t); 
}

float3 CombineFog(float3 inputColor, float2 uv)
{
    float3 wsCoordinate = GetPixelWorldSpacePosition(uv);
    float distFromView = distance(wsCoordinate, _WorldSpaceCameraPos);
                
    half distFogMask = GetDistanceFogMask(distFromView);
    float3 fogColor = lerp(_FogStartColor.rgb, _FogEndColor.rgb, distFogMask);

    half heightFog = GetHeightFogMask(wsCoordinate.y);
    half fogMask = distFogMask * _FogDensity * heightFog;
    fogMask = UnclampedSmoothstep(0.0,1.0,fogMask);

    return lerp(inputColor, fogColor, fogMask);
}


float4 frag (Varyings IN) : SV_TARGET
{
    float2 uv = IN.positionHCS.xy / _ScaledScreenParams.xy;
    float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    return float4(CombineFog(col.rgb, uv), 1.0);
}

#endif