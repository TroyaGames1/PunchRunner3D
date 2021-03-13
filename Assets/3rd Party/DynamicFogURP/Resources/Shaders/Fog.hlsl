#include "CommonsURP.hlsl"
#include "FogOfWar.hlsl"

sampler2D _MainTex;
sampler2D _GradientTex;
float jitter;
half4 _Color;
half4 _SecondColor;

float3 _SunDir;
float _LightDiffusionIntensity, _LightDiffusionPower;
float3 _WindDirection;
float _DitherStrength;
half3 _LightColor;
float3  _DensityData;
float4 _Geom;
float4 _NoiseData;

#define NOISE_SCALE _NoiseData.x
#define NOISE_INTENSITY _NoiseData.y
#define NOISE_SHIFT _NoiseData.z
#define NOISE_COLOR _NoiseData.w

#define BOTTOM_LEVEL _Geom.x
#define MAX_HEIGHT _Geom.y
#define DISTANCE_MAX _Geom.z

float3  _Density;
#define DENSITY _Density.x
#define HEIGHT_FALLOFF _Density.y

void SetJitter(float4 scrPos) {
    float2 uv = (scrPos.xy / scrPos.w) * _ScreenParams.xy;
    const float3 magic = float3( 0.06711056, 0.00583715, 52.9829189 );
    jitter = frac( magic.z * frac( dot( uv, magic.xy ) ) );
}


inline float3 ProjectOnPlane(float3 v, float3 planeNormal) {
    float sqrMag = dot(planeNormal, planeNormal);
    float dt = dot(v, planeNormal);
	return v - planeNormal * dt / sqrMag;
}

inline float3 GetRayStart(float3 wpos) {
    float3 cameraPosition = GetCameraPositionWS();
    #if defined(ORTHO_SUPPORT) 
	    float3 cameraForward = UNITY_MATRIX_V[2].xyz;
	    float3 rayStart = ProjectOnPlane(wpos - cameraPosition, cameraForward) + cameraPosition;
        return lerp(cameraPosition, rayStart, unity_OrthoParams.w);
    #elif DF2_BOX_PROJECTION
	    float3 cameraForward = UNITY_MATRIX_V[2].xyz;
	    return ProjectOnPlane(wpos - cameraPosition, cameraForward) + cameraPosition;
    #else
        return cameraPosition;
    #endif
}


half4 GetFogColor(float3 rayStart, float3 wpos) {

	float3 ray = wpos - rayStart;
   	float dist = length(ray);
    clip(DISTANCE_MAX - dist);

	float3 rayDir = ray / dist;

	half sunAmount = max( dot( rayDir, _SunDir.xyz ), 0.0 );    
	half diffusion = step(0.99999, rawDepth) * pow(sunAmount, _LightDiffusionPower) * _LightDiffusionIntensity;
    float t = (abs(_SunDir.y) + abs(rayDir.y)) * 0.5;
    half4 gradientColor = tex2D(_GradientTex, float2(t, 0));

    float2 noiseTexCoord = wpos.xz * NOISE_SCALE + _WindDirection.xz;
    half noise = tex2D(_MainTex, noiseTexCoord).r;
    half colorNoise = saturate(noise + NOISE_SHIFT) * NOISE_COLOR;

    half4 baseColor = lerp(_Color, _SecondColor, colorNoise);
	half3 fogColor = lerp(baseColor.rgb, baseColor.rgb * gradientColor.rgb, gradientColor.a) * _LightColor + diffusion;

    float rayStartY = rayStart.y - BOTTOM_LEVEL + noise * NOISE_INTENSITY;

	half fogAmount = (HEIGHT_FALLOFF / DENSITY) * exp(-rayStartY * DENSITY) * (1.0-exp( -dist * rayDir.y * DENSITY )) / rayDir.y;

    float x = MAX_HEIGHT / max(0.001, wpos.y);
    fogAmount = saturate(fogAmount) * saturate(x);

	half4 res = half4(fogColor, fogAmount * baseColor.a);

    #if DF2_FOW
        res *= ApplyFogOfWar(wpos);
    #endif

	res = max(0, res + (jitter - 0.5) * _DitherStrength);

    return res;

}