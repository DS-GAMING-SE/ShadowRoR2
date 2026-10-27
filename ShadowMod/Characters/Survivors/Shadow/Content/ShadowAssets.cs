using ShadowMod.Modules;
using RoR2;
using RoR2.Projectile;
using RoR2.ContentManagement;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using R2API;
using UnityEngine.UI;
using RoR2.UI;
using UnityEngine.Rendering.PostProcessing;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject chaosSpearOrbEffect;

        public static GameObject boostFlashEffect;
        public static GameObject boostAuraEffect;

        public static GameObject chaosBoostActivationEffect;
        public static GameObject bombExplosionEffect;

        public static GameObject pathItemHeroNotificationPrefab;
        public static GameObject pathItemDarkNotificationPrefab;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        #region AssetGUIDs
        public static AssetReferenceT<GameObject> characterModel = new AssetReferenceT<GameObject>("a32fb17ad7dfd7b4f8b893feaf7d7512");
        public static AssetReferenceT<GameObject> displayPrefab = new AssetReferenceT<GameObject>("9552880169ac54248bdf012a5175ef46");

        public static AssetReferenceT<RuntimeAnimatorController> animator = new AssetReferenceT<RuntimeAnimatorController>("f1a82152954bc974895f8ff9f035f84f");
        public static AssetReferenceT<RuntimeAnimatorController> displayAnimator = new AssetReferenceT<RuntimeAnimatorController>("4397b394b0996d44ba60f0f7769a33d1");
        public static AssetReferenceT<Avatar> animatorAvatar = new AssetReferenceT<Avatar>("3f55d8352b2212743b809db2974cdd5e");

        public static AssetReferenceTexture characterIcon = new AssetReferenceTexture("fbdab69c75e3cd94596054335b2c2744");
        public static AssetReferenceSprite passiveSkillIcon = new AssetReferenceSprite("9fcf79bda87678d4e80f4eb6462e4fdf");
        public static AssetReferenceSprite chaosSnapSkillIcon = new AssetReferenceSprite("89141efa71806744d9caf29b253b0d33");
        public static AssetReferenceSprite chaosSpearSkillIcon = new AssetReferenceSprite("18f6e1df055a2dd4085e66d3d3c5979e");
        public static AssetReferenceSprite boostSkillIcon = new AssetReferenceSprite("1c07abb3d41d38c4cac5563c3c1630d7");
        public static AssetReferenceSprite chaosBoostSkillIcon = new AssetReferenceSprite("6c6ddbdba1fa78d4caaf9fe144d3ac0e");
        public static AssetReferenceSprite chaosBoostUpgradedSkillIcon = new AssetReferenceSprite("8e7fefe0591a7bf41912ee88085e598d");
        public static AssetReferenceSprite chaosBlastSkillIcon = new AssetReferenceSprite("d36947727e0d2484d8bba10709948a83");
        public static AssetReferenceSprite voicelineEnableSkillIcon = new AssetReferenceSprite("c2cc0e85354a76044a13ee6e9667480a");
        public static AssetReferenceSprite voicelineDisableSkillIcon = new AssetReferenceSprite("13162ebc3e59e934eaf3c5aa849b2948");

        public static AssetReferenceT<Sprite> shadowLogoIcon = new("eec1a2a0d7c225548aa2ba6f9c8a5531");

        public static AssetReferenceT<Material> defaultSkinMaterial = new AssetReferenceT<Material>("14d6d121af8aca345b797120c5f6331b");
        public static AssetReferenceT<Mesh> defaultSkinMesh = new AssetReferenceT<Mesh>("fee9d8e08c8c818498f7613f868f2e34");
        public static AssetReferenceT<Mesh> defaultSkinSwordMesh = new AssetReferenceT<Mesh>("06ba2d7b66b5a7c47ae54f402cb9e132");
        public static AssetReferenceT<Mesh> defaultSkinGunMesh = new AssetReferenceT<Mesh>("95bb770c878495b4296296a4b09a6034");
        public static AssetReferenceSprite defaultSkinIcon = new AssetReferenceSprite("0be3f4ea9f44e3d4da982f8a0fca23a9");

        public static AssetReferenceT<Material> masterySkinMaterial = new AssetReferenceT<Material>("507751368e16e18409e1c93f8022eb8c");
        public static AssetReferenceT<Mesh> masterySkinMesh = new AssetReferenceT<Mesh>("e49dd36434c00024985424af2da4cedd");
        public static AssetReferenceT<Mesh> masterySkinSwordMesh = new AssetReferenceT<Mesh>("7a3e4cd720806a248b00c548032956cd");
        public static AssetReferenceSprite masterySkinIcon = new AssetReferenceSprite("639a10fc5e926d644a0bfc95000aaa8d");

        public static AssetReferenceT<GameObject> projectileGhost = new AssetReferenceT<GameObject>("2b1ae6eb92856db41a261cf5336101dc");

        public static AssetReferenceT<GameObject> projectileExplodeEffect = new AssetReferenceT<GameObject>("05b273758480af74a919e826c7b80a86");
        public static AssetReferenceT<GameObject> swingEffect = new AssetReferenceT<GameObject>("3534552e7829f9842ba3156065afc540");
        public static AssetReferenceT<GameObject> hitEffect = new AssetReferenceT<GameObject>("48eebb9268b618943a4b60bb011fb96d");

        public static AssetReferenceT<GameObject> chaosBoostActivation = new("ad91f54836c7f9249beb441a7c743d31");
        #endregion

        public static void Init()
        {
            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();
        }

        #region effects
        private static void CreateEffects()
        {
            #region Common Stuff
            Texture texRedRamp = AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampTeleporter_png).WaitForCompletion();
            Texture texRedPinkRamp = AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampArchWisp_png).WaitForCompletion();
            Texture texPurpleRamp /*pink and cool when brighter*/ = AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_Common_ColorRamps.texRampRailgun_png).WaitForCompletion();

            Material matTracerBright = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matTracerBright_mat).WaitForCompletion();
            Material matGlowHard = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC4.matProvUltFlareGlowHard2_mat).WaitForCompletion();
            Material matGlowSoft = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matGlow1Soft_mat).WaitForCompletion();
            Material matSpark = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matSpark1_mat).WaitForCompletion();
            Material matDistortionFaded = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matDistortionFaded_mat).WaitForCompletion();
            Material matSphereBurst = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.matDefectiveUnitDetonateSpherePulse_mat).WaitForCompletion());
            matSphereBurst.SetColor("_TintColor", new Color(3f, 0f, 0.06f));
            Material matSphereParticle = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_JumpDamageStrike.matJumpDamageStrikeBlast_mat).WaitForCompletion());
            matSphereParticle.SetTexture("_RemapTex", texRedPinkRamp);
            Material matRedEyeFlash = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Titan.matTitanLaserGlob_mat).WaitForCompletion();
            Material matPinkPurpleLightning = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_ShockDamageAura.matDroneShockDamageElectricity02_mat).WaitForCompletion());
            matPinkPurpleLightning.SetFloat("_AlphaBias", 0f);
            matPinkPurpleLightning.SetFloat("_AlphaBoost", 3.5f);
            matPinkPurpleLightning.SetTexture("_RemapTex", texPurpleRamp);
            Material matGenericRing = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_BFG.matBeamSphereBeam_mat).WaitForCompletion());
            matGenericRing.SetTexture("_RemapTex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampTritone_png).WaitForCompletion());
            matGenericRing.SetVector("_CutoffScroll", new Vector4(-20, 0, 10, 0));
            matGenericRing.EnableKeyword("VERTEXCOLOR");
            Material matChaosBoostOverlay = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_CritOnUse.matFullCrit_mat).WaitForCompletion());
            matChaosBoostOverlay.SetTexture("_RemapTex", texRedPinkRamp);

            Mesh donut2Mesh = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.mdlVFXDonut2_fbx).WaitForCompletion();
            Mesh noiseSphereMesh /*double radius of normal sphere?*/ = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.mdlNoiseSphere_01_fbx_GEO_NoiseSphere_).WaitForCompletion();
            Mesh electricMesh1 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt01_fbx).WaitForCompletion();
            Mesh electricMesh2 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt02_fbx).WaitForCompletion();
            Mesh electricMesh3 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt03_fbx).WaitForCompletion();
            Mesh electricMesh4 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt04_fbx).WaitForCompletion();
            Mesh[] electricMeshes = [electricMesh1, electricMesh2, electricMesh3, electricMesh4];
            Mesh electricRingMesh = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.mdlBentLightning01_fbx).WaitForCompletion();

            PostProcessProfile ppRed = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_Railgunner.ppLocalRailgunnerSuper_asset).WaitForCompletion();
            PostProcessProfile ppPain = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC4_Enforcer.ppLocalEnfShieldTackle_asset).WaitForCompletion();
            PostProcessProfile ppDoom = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_title_PostProcessing.ppLocalTPActivation_asset).WaitForCompletion();
            PostProcessProfile ppChaosBlast = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_SolusWingForm1.ppOverheat_asset).WaitForCompletion();
            #endregion
            ShadowAssets.projectileExplodeEffect.LoadAssetAsync().Completed += delegate (AsyncOperationHandle<GameObject> x)
            { CreateBombExplosionEffect(x.Result);
                CreateBombProjectile();
            };

            ShadowAssets.swingEffect.LoadAssetAsync().Completed += delegate (AsyncOperationHandle<GameObject> x)
            { swordSwingEffect = Asset.CreateEffect(x.Result, 1f, true, ""); };
            ShadowAssets.hitEffect.LoadAssetAsync().Completed += delegate (AsyncOperationHandle<GameObject> x)
            { swordHitImpactEffect = Asset.CreateEffect(x.Result, 1f); };

            chaosSpearOrbEffect = RoR2.Orbs.OrbStorageUtility.Get("Prefabs/Effects/OrbEffects/BarrierOrbEffect");

            boostFlashEffect = HedgehogUtils.Assets.CreateBoostFlashEffect("ShadowBoostFlash", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampDroneFire_png).WaitForCompletion(), ShadowSurvivor.shadowYellowColor);
            boostAuraEffect = HedgehogUtils.Assets.CreateBoostAuraEffect("ShadowBoostAura", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampDroneFire_png).WaitForCompletion(), ShadowSurvivor.shadowYellowColor);

            chaosBoostActivation.LoadAssetAsync().Completed += (x) =>
            {
                chaosBoostActivationEffect = Asset.CreateEffect(x.Result, 1f, true, "", out var vfx, out var effect);
                vfx.vfxIntensity = VFXAttributes.VFXIntensity.Medium;
                effect.applyScale = true;
                var pp = chaosBoostActivationEffect.transform.GetChild(0);
                var ppDuration = pp.gameObject.AddComponent<PostProcessDuration>();
                ppDuration.ppVolume = pp.GetComponent<PostProcessVolume>();
                ppDuration.ppVolume.sharedProfile = ppRed;
                ppDuration.maxDuration = 0.35f;
                ppDuration.ppWeightCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0f);
                chaosBoostActivationEffect.transform.GetChild(1).gameObject.AddLightIntensityCurve(AnimationCurve.EaseInOut(0f, 1f, 1f, 0f), 0.5f);
                chaosBoostActivationEffect.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matSphereBurst;
                chaosBoostActivationEffect.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowHard;
                chaosBoostActivationEffect.transform.GetChild(4).GetComponent<ParticleSystemRenderer>().sharedMaterial = HedgehogUtils.Assets.glitchOverlayMaterial;
                chaosBoostActivationEffect.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = matDistortionFaded;
                chaosBoostActivationEffect.transform.GetChild(6).gameObject.SetMaterialAndMesh(matSphereParticle, noiseSphereMesh);
                chaosBoostActivationEffect.transform.GetChild(7).gameObject.SetMaterialAndMeshes(matPinkPurpleLightning, electricMeshes);
                chaosBoostActivationEffect.transform.GetChild(8).GetComponent<ParticleSystemRenderer>().sharedMaterial = matSpark;
                chaosBoostActivationEffect.transform.GetChild(9).GetComponent<ParticleSystemRenderer>().sharedMaterial = matTracerBright;
                chaosBoostActivationEffect.transform.GetChild(10).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowHard;

                ShakeEmitter shakeEmitter = chaosBoostActivationEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.4f;
                shakeEmitter.radius = 4f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = true;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 0.5f,
                    frequency = 40f,
                    cycleOffset = 0f
                };
            };

            Material pathItemNotificationBackgroundMat = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_UI.matUIIconDronePurchase_mat).WaitForCompletion());
            pathItemNotificationBackgroundMat.name = "matUIShadowPathItemBackground";
            pathItemNotificationBackgroundMat.SetColor("_Color", Color.white);
            pathItemNotificationBackgroundMat.SetFloat("_PatternPixelSize", 90f);
            pathItemNotificationBackgroundMat.SetTexture("_OverlayTex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_TiledTextures.texCloudDifferenceBW2_png).WaitForCompletion());
            pathItemHeroNotificationPrefab = CreatePathItemNotificationPrefab("ShadowPathItemHeroNotificationPanel", new Color32(149, 205, 229, 255), pathItemNotificationBackgroundMat);
            pathItemDarkNotificationPrefab = CreatePathItemNotificationPrefab("ShadowPathItemDarkNotificationPanel", new Color32(254, 124, 124, 255), pathItemNotificationBackgroundMat);
            // Captain tazer for blue part of Chaos Blast
        }

        private static GameObject CreatePathItemNotificationPrefab(string name, Color color, Material mat)
        {
            GameObject prefab = PrefabAPI.InstantiateClone(AssetAsyncReferenceManager<GameObject>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_UI.DroneRepairNotificationPanel_prefab).WaitForCompletion(), name, false);
            Transform canvas = prefab.transform.Find("CanvasGroup");
            Transform bgTransform = canvas.Find("IconArea/BG");
            if (bgTransform)
            {
                if (bgTransform.TryGetComponent<AsyncImage>(out var asyncImage))
                {
                    asyncImage.Clear();
                    asyncImage.SetAddress(shadowLogoIcon);
                }
                if (bgTransform.TryGetComponent<RawImage>(out var image))
                {
                    image.color = color;
                    image.material = mat;
                    image.texture = AssetAsyncReferenceManager<Sprite>.LoadAsset(shadowLogoIcon).WaitForCompletion().texture;
                }
                ((RectTransform)bgTransform).sizeDelta = new Vector2(96, 96);
            }
            else
            {
                Log.Error("Background not found on PathItemNotificationPrefab");
            }
            Transform flashTransform = canvas.Find("Flash");
            if (flashTransform && flashTransform.TryGetComponent<RawImage>(out var flashImage))
            {
                flashImage.color = color;
            }
            return prefab;
        }

        private static void CreateBombExplosionEffect(GameObject prefab)
        {
            bombExplosionEffect = Asset.CreateEffect(prefab, 5f, false, "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        internal static void AddLightIntensityCurve(this GameObject gameObject, AnimationCurve curve, float duration)
        {
            var light = gameObject.AddComponent<LightIntensityCurve>();
            light.curve = curve;
            light.timeMax = duration;
        }
        internal static void SetMaterialAndMesh(this GameObject gameObject, Material mat, Mesh mesh)
        {
            if (gameObject.TryGetComponent<ParticleSystemRenderer>(out var renderer))
            {
                renderer.sharedMaterial = mat;
                renderer.mesh = mesh;
            }
        }
        internal static void SetMaterialAndMeshes(this GameObject gameObject, Material mat, Mesh[] mesh)
        {
            if (gameObject.TryGetComponent<ParticleSystemRenderer>(out var renderer))
            {
                renderer.sharedMaterial = mat;
                renderer.SetMeshes(mesh);
            }
        }
        #endregion effects

        #region projectiles

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            
            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();
            bombController.ghostPrefabAddress = projectileGhost;

            bombController.startSound = "";

            Content.AddProjectilePrefab(bombProjectilePrefab);
        }
        #endregion projectiles
    }
}
