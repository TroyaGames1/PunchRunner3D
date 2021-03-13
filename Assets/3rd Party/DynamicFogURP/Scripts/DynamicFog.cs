//------------------------------------------------------------------------------------------------------------------
// Dynamic Fog & Mist 2
// Created by Kronnect
//------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace DynamicFogAndMist2 {


    [ExecuteInEditMode]
    public partial class DynamicFog : MonoBehaviour {

        public DynamicFogProfile profile;

        public bool enableFade;
        public float fadeDistance = 1;
        public Transform fadeController;
        [Tooltip("Tints the fog volume in light red")]
        public bool showBoundary;

        const string SKW_FOW = "DF2_FOW";
        const string SKW_BOX_PROJECTION = "DF2_BOX_PROJECTION";

        Renderer r;
        Material fogMat, turbulenceMat;
        Material fogMat2D, turbulenceMat2D;
        RenderTexture rtTurbulence;
        float turbAcum;
        float windAcum;
        Vector3 sunDir;
        float dayLight;
        static Texture2D noiseTex;
        Texture2D gradientTex;
        Mesh debugMesh;
        Material fogDebugMat;
        DynamicFogProfile activeProfile, lerpProfile;
        Vector3 lastControllerPosition;

        void OnEnable() {
            if (noiseTex == null) {
                noiseTex = Resources.Load<Texture2D>("Textures/NoiseTex256");
            }
            FogOfWarInit();
            UpdateMaterialProperties();
        }

        private void OnDisable() {
            if (profile != null) {
                profile.onSettingsChanged -= UpdateMaterialProperties;
            }
        }

        private void OnValidate() {
            UpdateMaterialProperties();
        }

        private void OnDestroy() {
            if (rtTurbulence != null) {
                rtTurbulence.Release();
            }
            if (fogMat != null) {
                DestroyImmediate(fogMat);
                fogMat = null;
            }
            FogOfWarDestroy();
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        }

        void LateUpdate() {
            if (fogMat == null || r == null || profile == null) return;

            DynamicFogManager globalManager = DynamicFogManager.instance;
            Light sun = globalManager.sun;
            if (sun != null) {
                sunDir = -sun.transform.forward;
                fogMat.SetVector("_SunDir", sunDir);
                dayLight = 1f + sunDir.y * 2f;
                if (dayLight < 0) dayLight = 0; else if (dayLight > 1f) dayLight = 1f;
                Color lightColor = sun.color * (sun.intensity * dayLight * profile.brightness);
                fogMat.SetVector("_LightColor", lightColor);
            }

            windAcum += Time.deltaTime;
            windAcum %= 10000;
            fogMat.SetVector("_WindDirection", profile.direction * windAcum);

            transform.rotation = Quaternion.identity;

            UpdateNoise();

            if (enableFade) {
                ComputeFade();
                ApplyProfileSettings();
            }

            if (enableFogOfWar) {
                UpdateFogOfWar();
            }

            if (showBoundary) {
                if (fogDebugMat == null) {
                    fogDebugMat = new Material(Shader.Find("Hidden/DynamicFog2/DynamicFogDebug"));
                }
                if (debugMesh == null) {
                    MeshFilter mf = GetComponent<MeshFilter>();
                    if (mf != null) {
                        debugMesh = mf.sharedMesh;
                    }
                }
                Matrix4x4 m = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Graphics.DrawMesh(debugMesh, m, fogDebugMat, 0);
            }
        }


        void UpdateNoise() {
            if (profile == null || noiseTex == null) return;

            if (rtTurbulence == null || rtTurbulence.width != noiseTex.width) {
                RenderTextureDescriptor desc = new RenderTextureDescriptor(noiseTex.width, noiseTex.height, RenderTextureFormat.ARGB32, 0);
                rtTurbulence = new RenderTexture(desc);
                rtTurbulence.wrapMode = TextureWrapMode.Repeat;
            }
            turbAcum += Time.deltaTime * profile.speed;
            turbAcum %= 10000;
            turbulenceMat.SetFloat("_Amount", turbAcum);
            Graphics.Blit(noiseTex, rtTurbulence, turbulenceMat);

            fogMat.SetTexture("_MainTex", rtTurbulence);
        }

        public void UpdateMaterialProperties() {

            if (this == null || gameObject == null || !gameObject.activeInHierarchy) return;

            fadeDistance = Mathf.Max(0.1f, fadeDistance);

            r = GetComponent<Renderer>();

            if (profile == null) {
                if (fogMat == null && r != null) {
                    fogMat = new Material(Shader.Find("DynamicFog2/Empty"));
                    fogMat.hideFlags = HideFlags.DontSave;
                    r.sharedMaterial = fogMat;
                }
                return;
            }
            profile.onSettingsChanged -= UpdateMaterialProperties;
            profile.onSettingsChanged += UpdateMaterialProperties;

            if (fogMat2D == null) {
                fogMat2D = new Material(Shader.Find("DynamicFog2/DynamicFog2DURP"));
                fogMat2D.hideFlags = HideFlags.DontSave;
            }
            fogMat = fogMat2D;
            if (turbulenceMat2D == null) {
                turbulenceMat2D = new Material(Shader.Find("DynamicFog2/Turbulence2D"));
            }
            turbulenceMat = turbulenceMat2D;

            if (r != null) {
                r.sharedMaterial = fogMat;
            }

            if (fogMat == null || profile == null) return;

            profile.ValidateSettings();

            lastControllerPosition.x = float.MaxValue;
            ComputeFade();
            ApplyProfileSettings();
        }

        void ComputeFade() {

            float alphaMultiplier = 1f;

            if (maskEditorEnabled) alphaMultiplier = 0.85f;
            if (enableFade && Application.isPlaying) {
                if (fadeController == null) {
                    Camera cam = Camera.main;
                    if (cam != null) {
                        fadeController = Camera.main.transform;
                    }
                }
                if (fadeController != null) {

                    if (lastControllerPosition != fadeController.position) {

                        activeProfile = profile;

                        lastControllerPosition = fadeController.position;

                        // Self volume
                        float t = ComputeVolumeFade(transform, fadeDistance);
                        alphaMultiplier *= t;

                        // Check sub-volumes
                        int subVolumeCount = DynamicFogSubVolume.subVolumes.Count;
                        for (int k = 0; k < subVolumeCount; k++) {
                            DynamicFogSubVolume subVolume = DynamicFogSubVolume.subVolumes[k];
                            if (subVolume.profile == null) continue;
                            t = ComputeVolumeFade(subVolume.transform, subVolume.fadeDistance);
                            if (t > 0) {
                                if (lerpProfile == null) {
                                    lerpProfile = ScriptableObject.CreateInstance<DynamicFogProfile>();
                                }
                                lerpProfile.Lerp(activeProfile, subVolume.profile, t);
                                activeProfile = lerpProfile;
                            }
                        }
                    }
                }
            }

            if (activeProfile == null) {
                activeProfile = profile;
            }

            Color fogAlbedo = activeProfile.tintColor;
            Color secondColor = activeProfile.noiseColor;
            fogAlbedo.a *= alphaMultiplier;
            secondColor.a *= alphaMultiplier;

            fogMat.SetColor("_Color", fogAlbedo);
            fogMat.SetColor("_SecondColor", secondColor);
        }

        float ComputeVolumeFade(Transform transform, float fadeDistance) {
            Vector3 diff = transform.position - fadeController.position;
            diff.x = diff.x < 0 ? -diff.x : diff.x;
            diff.y = diff.y < 0 ? -diff.y : diff.y;
            diff.z = diff.z < 0 ? -diff.z : diff.z;
            Vector3 extents = transform.lossyScale * 0.5f;
            Vector3 gap = extents - diff;
            float minDiff = gap.x < gap.y ? gap.x : gap.y;
            minDiff = minDiff < gap.z ? minDiff : gap.z;
            fadeDistance += 0.0001f;
            float t = Mathf.Clamp01(minDiff / fadeDistance);
            return t;
        }


        void ApplyProfileSettings() {
            r.sortingLayerID = activeProfile.sortingLayerID;
            r.sortingOrder = activeProfile.sortingOrder;

            // update gradient texture
            int w = DynamicFogProfile.GRADIENT_TEXTURE_WIDTH;
            if (gradientTex == null) {
                gradientTex = new Texture2D(w, 1, TextureFormat.RGBA32, false);
                gradientTex.wrapMode = TextureWrapMode.Clamp;
            }
            gradientTex.SetPixels(activeProfile.gradientColors);
            gradientTex.Apply();
            fogMat.SetTexture("_GradientTex", gradientTex);
            fogMat.renderQueue = activeProfile.renderQueue;
            fogMat.SetVector("_Geom", new Vector4(activeProfile.baseAltitude, 0.001f + activeProfile.maxHeight, activeProfile.distanceMax));
            fogMat.SetVector("_NoiseData", new Vector4(1f / (1f + activeProfile.scale * 200f), activeProfile.turbulence, activeProfile.shift, activeProfile.noiseColorBlend));
            fogMat.SetFloat("_LightDiffusionPower", activeProfile.lightDiffusionPower);
            fogMat.SetFloat("_LightDiffusionIntensity", activeProfile.lightDiffusionIntensity);
            fogMat.SetVector("_Density", new Vector3(1.001f - profile.densityExponential, activeProfile.densityLinear));
            fogMat.SetFloat("_DitherStrength", activeProfile.dithering * 0.01f);

            if (activeProfile.boxProjection) {
                fogMat.EnableKeyword(SKW_BOX_PROJECTION);
            } else {
                fogMat.DisableKeyword(SKW_BOX_PROJECTION);
            }
            if (enableFogOfWar) {
                fogMat.SetTexture("_FogOfWarTex", fogOfWarTexture);
                fogMat.SetVector("_FogOfWarCenter", fogOfWarCenter);
                fogMat.SetVector("_FogOfWarSize", fogOfWarSize);
                Vector3 ca = fogOfWarCenter - 0.5f * fogOfWarSize;
                fogMat.SetVector("_FogOfWarCenterAdjusted", new Vector3(ca.x / (fogOfWarSize.x + 0.0001f), 1f, ca.z / (fogOfWarSize.z + 0.0001f)));
                fogMat.EnableKeyword(SKW_FOW);
            } else {
                fogMat.DisableKeyword(SKW_FOW);
            }
        }

    }


}
