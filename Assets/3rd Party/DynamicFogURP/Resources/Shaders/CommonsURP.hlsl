#ifndef DYNAMIC_FOG_2_COMMONS_URP
#define DYNAMIC_FOG_2_COMMONS_URP

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

// ***** Uncomment to enable orthographic camera support
//#define ORTHO_SUPPORT

// ***** Uncomment to enable alternate world space reconstruction function
//#define USE_ALTERNATE_RECONSTRUCT_API


// Common URP code
#define VR_ENABLED defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED) || defined(SINGLE_PASS_STEREO)

#if defined(USE_ALTERNATE_RECONSTRUCT_API) || VR_ENABLED 
   #define GET_WORLD_POSITION(scrPos) GetWorldPosFromDepthAlt(scrPos)
#else
   #define GET_WORLD_POSITION(scrPos) GetWorldPosFromDepth(scrPos)
#endif

TEXTURE2D(_CustomDepthTexture);
SAMPLER(sampler_CustomDepthTexture);
int DF2_FLIP_DEPTH_TEXTURE;
float rawDepth;

inline void GetRawDepth(float2 uv) {
    rawDepth = SampleSceneDepth(DF2_FLIP_DEPTH_TEXTURE ? float2(uv.x, 1.0 - uv.y) : uv);
}


float3 GetWorldPosFromDepth(float4 scrPos) {

    float2 uv =  scrPos.xy / scrPos.w;

    // World position reconstruction
    GetRawDepth(uv);
    float4 raw   = mul(UNITY_MATRIX_I_VP, float4(uv * 2.0 - 1.0, rawDepth, 1.0));
    float3 worldPos  = raw.xyz / raw.w;

    #if UNITY_REVERSED_Z
         rawDepth = 1.0 - rawDepth;
    #endif

    return worldPos;
}


// Alternate reconstruct API; URP 7.4 doesn't set UNITY_MATRIX_I_VP correctly in VR, so we need to use this alternate method

inline float GetLinearEyeDepth(float2 uv) {
    GetRawDepth(uv);
  	float sceneLinearDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
    #if defined(ORTHO_SUPPORT)
        if (unity_OrthoParams.w) {
            #if UNITY_REVERSED_Z
                rawDepth = 1.0 - rawDepth;
            #endif
            float orthoDepth = lerp(_ProjectionParams.y, _ProjectionParams.z, rawDepth);
            sceneLinearDepth = lerp(sceneLinearDepth, orthoDepth, unity_OrthoParams.w);
        }
    #endif
    return sceneLinearDepth;
}


float3 GetWorldPosFromDepthAlt(float4 scrPos) {
    float2 uv =  scrPos.xy / scrPos.w;
    float vz = GetLinearEyeDepth(uv);

    #if defined(ORTHO_SUPPORT)
        if (unity_OrthoParams.w) {
            return vz;
        }
    #endif
    float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
    float2 suv = uv;
    #if UNITY_SINGLE_PASS_STEREO
        // If Single-Pass Stereo mode is active, transform the
        // coordinates to get the correct output UV for the current eye.
        float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
        suv = (suv - scaleOffset.zw) / scaleOffset.xy;
    #endif
    float3 vpos = float3((suv * 2 - 1) / p11_22, -1) * vz;
    float4 wpos = mul(unity_CameraToWorld, float4(vpos, 1));
    return wpos.xyz;
}


#endif // DYNAMIC_FOG_2_COMMONS_URP

