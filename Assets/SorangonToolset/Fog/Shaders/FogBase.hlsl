#ifndef FOG_BASE_INCLUDED
#define FOG_BASE_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

//------------------------ INPUTS -----------------------------
struct Attributes
{
    float4 positionOS : POSITION;
};

struct Varyings
{
    float4 positionHCS : SV_POSITION;
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings vert(Attributes IN)
{
    Varyings output = (Varyings)0;
    output.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
    return output;
}



//--------------------- COMMON PROPERTIES -----------------------
half _FogDensity;
    
float4 _FogDistanceParams;
#define FOG_START_DISTANCE _FogDistanceParams.x
#define FOG_DISTANCE _FogDistanceParams.y
#define FOG_VOLUMETRIC_DISTANCE _FogDistanceParams.z

float4 _FogHeightParams;
#define FOG_HEIGHT_START _FogHeightParams.x
#define FOG_HEIGHT _FogHeightParams.y



//---------------------- DEPTH AND FOG COMPUTE METHODS -------------------
real GetDepth(float2 uv)
{
    #if UNITY_REVERSED_Z 
    real depth = SampleSceneDepth(uv);
    #else
    real depth = lerp(UNITY_NEAR_CLIP_VALUE,1,SampleSceneDepth(uv));
    #endif

    return depth;
}

float3 GetPixelWorldSpacePosition(half2 uv)
{
    float depth = GetDepth(uv);
    return ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
}


float GetDistanceFogMask(float distanceFromView)
{
    return saturate((distanceFromView / FOG_DISTANCE) - FOG_START_DISTANCE / FOG_DISTANCE);
}


float GetHeightFogMask(float heightCoords)
{
    return saturate(1 - (heightCoords / FOG_HEIGHT) + FOG_HEIGHT_START/ FOG_HEIGHT);
}

#endif