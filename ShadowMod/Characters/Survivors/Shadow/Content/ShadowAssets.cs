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
using RoR2.AsyncManagement;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject chaosMarkConsumeEffect;

        public static EntityStateGameObject chaosSpearOrbEffect;
        public static EntityStateGameObject chaosSpearHitEffect;
        public static Material chaosSpearShockingOverlay;
        public static EntityStateGameObject chaosSpearShockedEffect;


        public static Material doomSpearShockingOverlay;

        public static GameObject boostFlashEffect;
        public static GameObject boostAuraEffect;

        public static EntityStateGameObject chaosBoostActivationEffect;
        public static EntityStateGameObject chaosBlastExplosionEffect;
        public static GameObject bombExplosionEffect;

        public static GameObject pathItemHeroNotificationPrefab;
        public static GameObject pathItemDarkNotificationPrefab;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        #region AssetGUIDs
        public static AssetReferenceT<GameObject> characterModelRef = new AssetReferenceT<GameObject>("a32fb17ad7dfd7b4f8b893feaf7d7512");
        public static AssetReferenceT<GameObject> displayPrefabRef = new AssetReferenceT<GameObject>("9552880169ac54248bdf012a5175ef46");

        public static AssetReferenceT<RuntimeAnimatorController> animatorRef = new AssetReferenceT<RuntimeAnimatorController>("f1a82152954bc974895f8ff9f035f84f");
        public static AssetReferenceT<RuntimeAnimatorController> displayAnimatorRef = new AssetReferenceT<RuntimeAnimatorController>("4397b394b0996d44ba60f0f7769a33d1");
        public static AssetReferenceT<Avatar> animatorAvatarRef = new AssetReferenceT<Avatar>("3f55d8352b2212743b809db2974cdd5e");

        #region Character Icons
        public static AssetReferenceTexture characterIconRef = new AssetReferenceTexture("fbdab69c75e3cd94596054335b2c2744");
        public static AssetReferenceSprite passiveSkillIconRef = new AssetReferenceSprite("9fcf79bda87678d4e80f4eb6462e4fdf");

        public static AssetReferenceSprite chaosSnapSkillIconRef = new AssetReferenceSprite("89141efa71806744d9caf29b253b0d33");
        public static AssetReferenceSprite chaosSpearSkillIconRef = new AssetReferenceSprite("18f6e1df055a2dd4085e66d3d3c5979e");
        public static AssetReferenceSprite boostSkillIconRef = new AssetReferenceSprite("1c07abb3d41d38c4cac5563c3c1630d7");
        public static AssetReferenceSprite chaosBoostSkillIconRef = new AssetReferenceSprite("6c6ddbdba1fa78d4caaf9fe144d3ac0e");
        public static AssetReferenceSprite chaosBoostUpgradedSkillIconRef = new AssetReferenceSprite("8e7fefe0591a7bf41912ee88085e598d");
        public static AssetReferenceSprite chaosBlastSkillIconRef = new AssetReferenceSprite("d36947727e0d2484d8bba10709948a83");

        public static AssetReferenceSprite voicelineEnableSkillIconRef = new AssetReferenceSprite("c2cc0e85354a76044a13ee6e9667480a");
        public static AssetReferenceSprite voicelineDisableSkillIconRef = new AssetReferenceSprite("13162ebc3e59e934eaf3c5aa849b2948");

        #region Doom Skill Icons
        public static AssetReferenceSprite doomBlastSkillIconRef = new AssetReferenceSprite("c69ee8e8c0ccd5c4c9ab016e47a5328c");
        public static AssetReferenceSprite doomSpearSkillIconRef = new AssetReferenceSprite("e53b10eb5fbccc745a308a44d9f9eb34");
        public static AssetReferenceSprite doomBoostSkillIconRef = new AssetReferenceSprite("1c07abb3d41d38c4cac5563c3c1630d7");
        public static AssetReferenceSprite doomWingSkillIconRef = new AssetReferenceSprite("872766d8c9b26e846b35b3d2a63e5664");
        public static AssetReferenceSprite doomWingUpgradedSkillIconRef = new AssetReferenceSprite("c43d3d32e0bb5aa43966e4e9cf0ee935");
        #endregion
        #endregion

        #region Paths Related
        public static AssetReferenceT<Sprite> shadowLogoIconRef = new("eec1a2a0d7c225548aa2ba6f9c8a5531");
        public static AssetReferenceT<GameObject> blackDoomInteractableRef = new("46576300890e4a14aa5ef325e2976e7b");
        #endregion

        #region Default Skin
        public static AssetReferenceT<Material> defaultSkinMaterialRef = new AssetReferenceT<Material>("14d6d121af8aca345b797120c5f6331b");
        public static AssetReferenceT<Mesh> defaultSkinMeshRef = new AssetReferenceT<Mesh>("fee9d8e08c8c818498f7613f868f2e34");
        public static AssetReferenceT<Mesh> defaultSkinSwordMeshRef = new AssetReferenceT<Mesh>("06ba2d7b66b5a7c47ae54f402cb9e132");
        public static AssetReferenceT<Mesh> defaultSkinGunMeshRef = new AssetReferenceT<Mesh>("95bb770c878495b4296296a4b09a6034");
        public static AssetReferenceSprite defaultSkinIconRef = new AssetReferenceSprite("0be3f4ea9f44e3d4da982f8a0fca23a9");
        #endregion

        #region Mastery Skin
        public static AssetReferenceT<Material> masterySkinMaterialRef = new AssetReferenceT<Material>("507751368e16e18409e1c93f8022eb8c");
        public static AssetReferenceT<Mesh> masterySkinMeshRef = new AssetReferenceT<Mesh>("e49dd36434c00024985424af2da4cedd");
        public static AssetReferenceT<Mesh> masterySkinSwordMeshRef = new AssetReferenceT<Mesh>("7a3e4cd720806a248b00c548032956cd");
        public static AssetReferenceSprite masterySkinIconRef = new AssetReferenceSprite("639a10fc5e926d644a0bfc95000aaa8d");
        #endregion

        #region Skill VFX
        public static AssetReferenceT<GameObject> projectileGhost = new AssetReferenceT<GameObject>("2b1ae6eb92856db41a261cf5336101dc");

        public static AssetReferenceT<GameObject> projectileExplodeEffect = new AssetReferenceT<GameObject>("05b273758480af74a919e826c7b80a86");
        public static AssetReferenceT<GameObject> swingEffect = new AssetReferenceT<GameObject>("3534552e7829f9842ba3156065afc540");
        public static AssetReferenceT<GameObject> hitEffect = new AssetReferenceT<GameObject>("48eebb9268b618943a4b60bb011fb96d");

        public static AssetReferenceT<GameObject> chaosSpearOrbRef = new("141d1d52f19f4c846b85dc6c728e4766");
        public static AssetReferenceT<GameObject> chaosSpearHitRef = new("cb1538cda4ed7d146bf7d2718d328508");
        public static AssetReferenceT<GameObject> chaosSpearShockedRef = new("872882cee89840e4c85e402a249d26af");

        public static AssetReferenceT<GameObject> chaosBoostActivationRef = new("ad91f54836c7f9249beb441a7c743d31");
        public static AssetReferenceT<GameObject> chaosBlastExplosionRef = new("e035764ea9303ca41a4c9d0138783c24");
        #endregion
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
            Texture texRampDefault = AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampDefault_png).WaitForCompletion();
            Texture texRampTwotone /*Use 26 rgb to make wind*/ = AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampTwotone_jpg).WaitForCompletion();

            Material matTracerBright = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matTracerBright_mat).WaitForCompletion();
            Material matGlowHard = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC4.matProvUltFlareGlowHard2_mat).WaitForCompletion();
            Material matGlowSoft = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matGlow1Soft_mat).WaitForCompletion();
            Material matHitSpark = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matSpark1_mat).WaitForCompletion();
            Material matDistortionFaded = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.matDistortionFaded_mat).WaitForCompletion();
            Material matSphereBurst = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.matDefectiveUnitDetonateSpherePulse_mat).WaitForCompletion());
            matSphereBurst.SetColor("_TintColor", new Color(3f, 0f, 0.06f));
            Material matSphereParticle = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_JumpDamageStrike.matJumpDamageStrikeBlast_mat).WaitForCompletion());
            matSphereParticle.SetTexture("_RemapTex", texRedPinkRamp);
            Material matRedEyeFlash /*combine with EyeFlash component*/ = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Titan.matTitanLaserGlob_mat).WaitForCompletion();
            Material matPinkPurpleLightning = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_ShockDamageAura.matDroneShockDamageElectricity02_mat).WaitForCompletion());
            matPinkPurpleLightning.SetFloat("_Boost", 6f);
            matPinkPurpleLightning.SetFloat("_AlphaBias", 0f);
            matPinkPurpleLightning.SetFloat("_AlphaBoost", 3.5f);
            matPinkPurpleLightning.SetTexture("_RemapTex", texPurpleRamp);
            Material matYellowLightning = new Material(matPinkPurpleLightning);
            matYellowLightning.SetTexture("_RemapTex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampWarbanner2_png).WaitForCompletion());
            matYellowLightning.SetFloat("_AlphaBoost", 2f);
            Material matYellowLightningBroken = new Material(matYellowLightning);
            matYellowLightningBroken.SetTexture("_Cloud1Tex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_TiledTextures.texCloudDifferenceBW1_png).WaitForCompletion());
            matYellowLightningBroken.SetVector("_CutoffScroll", new Vector4(16, 48, 0, 0));
            Material matDoomLightning = new Material(matYellowLightning);
            matDoomLightning.SetTexture("_RemapTex", texRedRamp);
            matDoomLightning.SetFloat("_AlphaBoost", 4f);
            matDoomLightning.SetFloat("_AlphaBias", 0.1f);
            matDoomLightning.SetFloat("_Boost", 10f);
            Material matDoomLightningBroken = new Material(matYellowLightningBroken);
            matDoomLightningBroken.SetTexture("_RemapTex", texRedRamp);
            matDoomLightningBroken.SetFloat("_AlphaBoost", 3f);
            matDoomLightningBroken.SetFloat("_Boost", 10f);
            Material matGenericRing = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_BFG.matBeamSphereBeam_mat).WaitForCompletion());
            matGenericRing.name = "matShadowGenericRing";
            matGenericRing.SetTexture("_RemapTex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampTritone_png).WaitForCompletion());
            matGenericRing.SetVector("_CutoffScroll", new Vector4(-20, 0, 10, 0));
            matGenericRing.EnableKeyword("VERTEXCOLOR");
            Material matChaosBoostOverlay = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_CritOnUse.matFullCrit_mat).WaitForCompletion());
            matChaosBoostOverlay.SetTexture("_RemapTex", texRedPinkRamp);
            Material matSubtleWindTrail = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Brother.matBrotherStealAura_mat).WaitForCompletion();
            Material matChaosBlastOuterSphere = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_SolusAmalgamator.matSolusAmalgamatorDeathSphere_mat).WaitForCompletion());
            matChaosBlastOuterSphere.name = "matShadowChaosBlastOuterSphere";
            matChaosBlastOuterSphere.SetTexture("_RemapTex", texPurpleRamp);
            matChaosBlastOuterSphere.SetColor("_TintColor", new Color(2f, 0.03f, 0.13f));
            matChaosBlastOuterSphere.SetTexture("_Cloud1Tex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_TiledTextures.texCloudCrackedIceInverted_png).WaitForCompletion());
            matChaosBlastOuterSphere.SetTextureScale("_Cloud1Tex", new Vector2(2f, 5f));
            matChaosBlastOuterSphere.SetVector("_CutoffScroll", new Vector4(6, 10, -30, 30));
            matChaosBlastOuterSphere.SetFloat("_RimPower", 0.75f);
            matChaosBlastOuterSphere.SetFloat("_RimStrength", 0.92f);
            matChaosBlastOuterSphere.SetFloat("_Boost", 1f);
            matChaosBlastOuterSphere.SetFloat("_AlphaBoost", 0.35f);
            Material matBurnGroundSphere = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC2_Items_MeteorAttackOnHighDamage.matRunicLensMeteorStrikeImpactIndicator_mat).WaitForCompletion());
            matBurnGroundSphere.name = "matShadowBurnGround";
            matBurnGroundSphere.SetColor("_TintColor", new Color(1f, 0.45f, 0.6f));
            matBurnGroundSphere.SetFloat("_InvFade", 0.13f);
            matBurnGroundSphere.SetFloat("_SoftPower", 1f);
            matBurnGroundSphere.SetFloat("_Boost", 5f);
            Material matChaosBlastFog = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_VoidCamp.matVoidCampSphereBackface_mat).WaitForCompletion());
            matChaosBlastFog.name = "matShadowChaosBlastFog";
            matChaosBlastFog.SetColor("_TintColor", new Color(0.6f, 0f, 0.1f));
            matChaosBlastFog.SetFloat("_InvFade", 0.02f);
            Material matGlowStrongHorizontal = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_SolusHeart.matSolusHeartEyeBlink_mat).WaitForCompletion());
            matGlowStrongHorizontal.name = "matShadowGlowStrongHorizontal";
            matGlowStrongHorizontal.SetColor("_TintColor", Color.white);
            matGlowStrongHorizontal.SetFloat("_InvFade", 1f);
            matGlowStrongHorizontal.SetFloat("_Boost", 20f);
            matGlowStrongHorizontal.SetInt("_ZTest", 8);
            matGlowStrongHorizontal.SetFloat("_DepthOffset", -5f);
            matGlowStrongHorizontal.EnableKeyword("VERTEXCOLOR");
            Material matSubtleSoftRing = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_moon2.matMoonBatteryShockwaveRing_mat).WaitForCompletion());
            matSubtleSoftRing.name = "matShadowSubtleSoftRing";
            matSubtleSoftRing.SetTexture("_RemapTex", texRampDefault);
            matSubtleSoftRing.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f));
            matSubtleSoftRing.SetFloat("_Boost", 1f);
            matSubtleSoftRing.SetFloat("_AlphaBoost", 2f);
            matSubtleSoftRing.SetFloat("_InvFade", 0.1f);
            matSubtleSoftRing.SetInt("_ZTest", 8);
            matSubtleSoftRing.SetFloat("_DepthOffset", -10f);
            chaosSpearShockingOverlay = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common.matIsShocked_mat).WaitForCompletion());
            chaosSpearShockingOverlay.SetTexture("_RemapTex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_Common_ColorRamps.texRampMinorConstructElectric_png).WaitForCompletion());
            chaosSpearShockingOverlay.SetFloat("_Boost", 4f);
            doomSpearShockingOverlay = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Drones.matIsShockedRed_mat).WaitForCompletion();
            // Look into Death Mark VFX mats for Doom

            Mesh donut1Mesh = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.mdlVFXDonut1_fbx).WaitForCompletion();
            Mesh donut2Mesh = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.mdlVFXDonut2_fbx).WaitForCompletion();
            Mesh donut5Mesh = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_VFX.mdlVFXDonut5_fbx).WaitForCompletion();
            Mesh noiseSphereMesh /*double radius of normal sphere?*/ = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.mdlNoiseSphere_01_fbx_GEO_NoiseSphere_).WaitForCompletion();
            Mesh electricMesh1 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt01_fbx).WaitForCompletion();
            Mesh electricMesh2 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt02_fbx).WaitForCompletion();
            Mesh electricMesh3 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt03_fbx).WaitForCompletion();
            Mesh electricMesh4 = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_DefectiveUnit.mdlDetonate_DebuffBolt04_fbx).WaitForCompletion();
            Mesh[] electricMeshes = [electricMesh1, electricMesh2, electricMesh3, electricMesh4];
            Mesh electricRingMesh = AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.mdlBentLightning01_fbx).WaitForCompletion();
            Mesh pSphere1Mesh = /*0.005x size*/AssetAsyncReferenceManager<Mesh>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3.mdlSphereTriLarge_fbx_pSphere1_).WaitForCompletion();

            PostProcessProfile ppRed = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC1_Railgunner.ppLocalRailgunnerSuper_asset).WaitForCompletion();
            PostProcessProfile ppPain = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC4_Enforcer.ppLocalEnfShieldTackle_asset).WaitForCompletion();
            PostProcessProfile ppDoom = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_title_PostProcessing.ppLocalTPActivation_asset).WaitForCompletion();
            PostProcessProfile ppChaosBlast = AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_SolusWingForm1.ppOverheat_asset).WaitForCompletion();
            PostProcessProfile ppChaosBlastImpact = ScriptableObject.Instantiate<PostProcessProfile>(AssetAsyncReferenceManager<PostProcessProfile>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC4_ReaperBoss.ppReapBossInitialDeath_asset).WaitForCompletion());
            ppChaosBlastImpact.name = "ppShadowChaosBlastImpact";
            if (ppChaosBlastImpact.TryGetSettings(out RampFog fog))
            {
                fog.fogColorMid.Override(new Color(0.5f, 0f, 0.06f));
            }
            #endregion
            ShadowAssets.projectileExplodeEffect.LoadAssetAsync().Completed += delegate (AsyncOperationHandle<GameObject> x)
            { CreateBombExplosionEffect(x.Result);
                CreateBombProjectile();
            };

            ShadowAssets.swingEffect.LoadAssetAsync().Completed += delegate (AsyncOperationHandle<GameObject> x)
            { swordSwingEffect = Asset.CreateEffect(x.Result, 1f, true, ""); };
            ShadowAssets.hitEffect.LoadAssetAsync().Completed += delegate (AsyncOperationHandle<GameObject> x)
            { swordHitImpactEffect = Asset.CreateEffect(x.Result, 1f); };

            chaosMarkConsumeEffect = AssetAsyncReferenceManager<GameObject>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_Items_SpeedOnPickup.SpeedOnPickupEffectTier2_prefab).WaitForCompletion();

            boostFlashEffect = HedgehogUtils.Assets.CreateBoostFlashEffect("ShadowBoostFlash", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampDroneFire_png).WaitForCompletion(), ShadowSurvivor.shadowYellowColor);
            boostAuraEffect = HedgehogUtils.Assets.CreateBoostAuraEffect("ShadowBoostAura", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_ColorRamps.texRampDroneFire_png).WaitForCompletion(), ShadowSurvivor.shadowYellowColor);

            #region Chaos Spear Hit
            GameObject chaosSpearHitPrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(chaosSpearHitRef).WaitForCompletion();
            chaosSpearHitEffect = new EntityStateGameObject(chaosSpearHitRef.AssetGUID);
            chaosSpearHitEffect.unloadType = AsyncReferenceHandleUnloadType.OnRunEnd;
            Asset.CreateEffect(chaosSpearHitPrefab, 0.4f, false, "Play_dsshadowthehedgehog_chaos_spear_hit", out _, out _);
            chaosSpearHitPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = HedgehogUtils.Assets.chromaImpact;
            chaosSpearHitPrefab.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowSoft;
            chaosSpearHitPrefab.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matHitSpark;
            chaosSpearHitPrefab.transform.GetChild(3).gameObject.AddLightIntensityCurve(AnimationCurve.EaseInOut(0f, 1f, 1f, 0f), 0.4f);
            chaosSpearHitPrefab.transform.GetChild(4).GetComponent<ParticleSystemRenderer>().sharedMaterial = matDistortionFaded;
            #endregion

            #region Chaos Spear Orb
            GameObject chaosSpearOrbPrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(chaosSpearOrbRef).WaitForCompletion();
            chaosSpearOrbEffect = new EntityStateGameObject(chaosSpearOrbRef.AssetGUID);
            chaosSpearOrbEffect.unloadType = AsyncReferenceHandleUnloadType.OnRunEnd;
            Asset.CreateEffect(chaosSpearOrbPrefab, -1f, false, "", out _, out var chaosSpearOrbEff);
            chaosSpearOrbEff.positionAtReferencedTransform = false;
            var chaosSpearOrbComponent = chaosSpearOrbPrefab.AddComponent<RoR2.Orbs.OrbEffect>();
            chaosSpearOrbComponent.movementCurve = AnimationCurve.Linear(0, 0, 1f, 1f);
            chaosSpearOrbComponent.endEffect = chaosSpearHitEffect;

            chaosSpearOrbPrefab.transform.GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matYellowLightning;
            chaosSpearOrbPrefab.transform.GetChild(2).gameObject.AddComponent<RotateObject>().rotationSpeed = new Vector3(0, 0, 2500);
            chaosSpearOrbPrefab.transform.GetChild(2).GetChild(0).GetComponent<TrailRenderer>().sharedMaterial = matYellowLightningBroken;
            chaosSpearOrbPrefab.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matTracerBright;
            #endregion

            #region Chaos Spear Shocked
            GameObject chaosSpearShockedPrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(chaosSpearShockedRef).WaitForCompletion();
            chaosSpearShockedEffect = new EntityStateGameObject(chaosSpearShockedRef.AssetGUID);
            chaosSpearShockedEffect.unloadType = AsyncReferenceHandleUnloadType.OnRunEnd;
            Asset.CreateEffect(chaosSpearShockedPrefab, -1f, false, "", out _, out var chaosSpearShockedEff);
            chaosSpearShockedEff.applyScale = true;
            var chaosSpearShockedDuration = chaosSpearShockedPrefab.AddComponent<ScaleParticleSystemDuration>();
            chaosSpearShockedDuration.initialDuration = 5f;
            chaosSpearShockedDuration.particleSystems = [chaosSpearShockedPrefab.transform.GetChild(0).GetComponent<ParticleSystem>()];
            chaosSpearShockedPrefab.transform.GetChild(0).gameObject.SetMaterialAndMesh(matYellowLightningBroken, electricRingMesh);
            #endregion

            #region Chaos Boost Activation
            GameObject chaosBoostActivationPrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(chaosBoostActivationRef).WaitForCompletion();
            chaosBoostActivationEffect = new EntityStateGameObject(chaosBoostActivationRef.AssetGUID);
            chaosBoostActivationEffect.unloadType = AsyncReferenceHandleUnloadType.OnRunEnd;
            Asset.CreateEffect(chaosBoostActivationPrefab, 1f, true, "", out var chaosBoostActivationVfx, out var chaosBoostActivationEff);
            chaosBoostActivationVfx.vfxIntensity = VFXAttributes.VFXIntensity.Medium;
            chaosBoostActivationEff.applyScale = true;
            var pp = chaosBoostActivationPrefab.transform.GetChild(0);
            var ppDuration = pp.gameObject.AddComponent<PostProcessDuration>();
            ppDuration.ppVolume = pp.GetComponent<PostProcessVolume>();
            ppDuration.ppVolume.sharedProfile = ppRed;
            ppDuration.maxDuration = 0.35f;
            ppDuration.ppWeightCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0f);
            chaosBoostActivationPrefab.transform.GetChild(1).gameObject.AddLightIntensityCurve(AnimationCurve.EaseInOut(0f, 1f, 1f, 0f), 0.5f);
            chaosBoostActivationPrefab.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matSphereBurst;
            chaosBoostActivationPrefab.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowHard;
            chaosBoostActivationPrefab.transform.GetChild(4).GetComponent<ParticleSystemRenderer>().sharedMaterial = HedgehogUtils.Assets.glitchOverlayMaterial;
            chaosBoostActivationPrefab.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = matDistortionFaded;
            chaosBoostActivationPrefab.transform.GetChild(6).gameObject.SetMaterialAndMesh(matSphereParticle, noiseSphereMesh);
            chaosBoostActivationPrefab.transform.GetChild(7).gameObject.SetMaterialAndMeshes(matPinkPurpleLightning, electricMeshes);
            chaosBoostActivationPrefab.transform.GetChild(8).GetComponent<ParticleSystemRenderer>().sharedMaterial = matHitSpark;
            chaosBoostActivationPrefab.transform.GetChild(9).GetComponent<ParticleSystemRenderer>().sharedMaterial = matTracerBright;
            chaosBoostActivationPrefab.transform.GetChild(10).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowHard;

            ShakeEmitter shakeEmitter = chaosBoostActivationPrefab.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.4f;
            shakeEmitter.radius = 15f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = true;

            shakeEmitter.wave = new Wave
            {
                amplitude = 0.4f,
                frequency = 40f,
                cycleOffset = 0f
            };
            #endregion

            #region Chaos Blast Explosion
            GameObject chaosBlastExplosionPrefab = AssetAsyncReferenceManager<GameObject>.LoadAsset(chaosBlastExplosionRef).WaitForCompletion();
            chaosBlastExplosionEffect = new EntityStateGameObject(chaosBlastExplosionRef.AssetGUID);
            chaosBlastExplosionEffect.unloadType = AsyncReferenceHandleUnloadType.OnRunEnd;
            Asset.CreateEffect(chaosBlastExplosionPrefab, 7f, false, "", out var chaosBlastExplosionVfx, out var chaosBlastExplosionEff);
            chaosBlastExplosionVfx.vfxIntensity = VFXAttributes.VFXIntensity.High;
            chaosBlastExplosionEff.applyScale = true;
            var flashPP = chaosBlastExplosionPrefab.transform.GetChild(2);
            var flashPPDuration = flashPP.gameObject.AddComponent<PostProcessDuration>();
            flashPPDuration.ppVolume = flashPP.GetComponent<PostProcessVolume>();
            flashPPDuration.ppVolume.sharedProfile = ppChaosBlastImpact;
            flashPPDuration.maxDuration = 0.15f;
            flashPPDuration.ppWeightCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            var interiorPP = chaosBlastExplosionPrefab.transform.GetChild(1);
            var interiorPPDuration = interiorPP.gameObject.AddComponent<PostProcessDuration>();
            interiorPPDuration.ppVolume = interiorPP.GetComponent<PostProcessVolume>();
            interiorPPDuration.ppVolume.sharedProfile = ppChaosBlast;
            interiorPPDuration.maxDuration = 2f;
            interiorPPDuration.ppWeightCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            var exteriorPP = chaosBlastExplosionPrefab.transform.GetChild(0);
            var exteriorPPDuration = exteriorPP.gameObject.AddComponent<PostProcessDuration>();
            exteriorPPDuration.ppVolume = exteriorPP.GetComponent<PostProcessVolume>();
            exteriorPPDuration.ppVolume.sharedProfile = ppRed;
            exteriorPPDuration.maxDuration = 2f;
            exteriorPPDuration.ppWeightCurve = AnimationCurve.EaseInOut(0f, 0.65f, 1f, 0f);
            chaosBlastExplosionPrefab.transform.GetChild(3).gameObject.AddLightIntensityCurve(AnimationCurve.EaseInOut(0f, 1f, 1f, 0f), 1.8f);
            chaosBlastExplosionPrefab.transform.GetChild(4).gameObject.SetMaterialAndMesh(matChaosBlastOuterSphere, pSphere1Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Gravekeeper.matOmniExplosion1ArchWisp_mat).WaitForCompletion();
            chaosBlastExplosionPrefab.transform.GetChild(6).gameObject.SetMaterialAndMesh(matSphereParticle, noiseSphereMesh);
            chaosBlastExplosionPrefab.transform.GetChild(7).gameObject.SetMaterialAndMesh(matSubtleWindTrail, donut5Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(8).gameObject.SetMaterialAndMesh(matSubtleWindTrail, donut1Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(9).GetComponent<ParticleSystemRenderer>().sharedMaterial = matTracerBright;
            chaosBlastExplosionPrefab.transform.GetChild(10).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowSoft;
            chaosBlastExplosionPrefab.transform.GetChild(11).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowHard;
            chaosBlastExplosionPrefab.transform.GetChild(12).gameObject.SetMaterialAndMesh(matBurnGroundSphere, pSphere1Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(13).gameObject.SetMaterialAndMesh(matChaosBlastFog, pSphere1Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(14).GetComponent<ParticleSystemRenderer>().sharedMaterial = HedgehogUtils.Assets.darkSparkle;
            chaosBlastExplosionPrefab.transform.GetChild(15).GetComponent<ParticleSystemRenderer>().sharedMaterial = matGlowStrongHorizontal;
            chaosBlastExplosionPrefab.transform.GetChild(16).gameObject.SetMaterialAndMesh(matSubtleSoftRing, donut1Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(17).gameObject.SetMaterialAndMesh(matSubtleSoftRing, donut1Mesh);
            chaosBlastExplosionPrefab.transform.GetChild(18).gameObject.SetMaterialAndMesh(matGenericRing, donut2Mesh);

            ShakeEmitter impactsShakeEmitter = chaosBlastExplosionPrefab.AddComponent<ShakeEmitter>();
            impactsShakeEmitter.amplitudeTimeDecay = true;
            impactsShakeEmitter.duration = 0.4f;
            impactsShakeEmitter.radius = 150f;
            impactsShakeEmitter.scaleShakeRadiusWithLocalScale = true;

            impactsShakeEmitter.wave = new Wave
            {
                amplitude = 1.5f,
                frequency = 30f,
                cycleOffset = 0f
            };
            ShakeEmitter lingerShakeEmitter = chaosBlastExplosionPrefab.AddComponent<ShakeEmitter>();
            lingerShakeEmitter.amplitudeTimeDecay = true;
            lingerShakeEmitter.duration = 2.1f;
            lingerShakeEmitter.radius = 100f;
            lingerShakeEmitter.scaleShakeRadiusWithLocalScale = true;

            lingerShakeEmitter.wave = new Wave
            {
                amplitude = 0.4f,
                frequency = 9f,
                cycleOffset = 0f
            };
            #endregion

            Material pathItemNotificationBackgroundMat = new Material(AssetAsyncReferenceManager<Material>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_DLC3_UI.matUIIconDronePurchase_mat).WaitForCompletion());
            pathItemNotificationBackgroundMat.name = "matUIShadowPathItemBackground";
            pathItemNotificationBackgroundMat.SetColor("_Color", Color.white);
            pathItemNotificationBackgroundMat.SetFloat("_PatternPixelSize", 90f);
            pathItemNotificationBackgroundMat.SetTexture("_OverlayTex", AssetAsyncReferenceManager<Texture>.LoadAsset(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common_TiledTextures.texCloudDifferenceBW2_png).WaitForCompletion());
            pathItemHeroNotificationPrefab = CreatePathItemNotificationPrefab("ShadowPathItemHeroNotificationPanel", new Color32(149, 205, 229, 255), pathItemNotificationBackgroundMat);
            pathItemDarkNotificationPrefab = CreatePathItemNotificationPrefab("ShadowPathItemDarkNotificationPanel", new Color32(254, 124, 124, 255), pathItemNotificationBackgroundMat);
            // Captain tazer for blue part of Chaos Blast
        }
        private static void CreateChaosBlast()
        {

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
                    asyncImage.SetAddress(shadowLogoIconRef);
                }
                if (bgTransform.TryGetComponent<RawImage>(out var image))
                {
                    image.color = color;
                    image.material = mat;
                    image.texture = AssetAsyncReferenceManager<Sprite>.LoadAsset(shadowLogoIconRef).WaitForCompletion().texture;
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
        private static void AddLightIntensityCurve(this GameObject gameObject, AnimationCurve curve, float duration)
        {
            var light = gameObject.AddComponent<LightIntensityCurve>();
            light.curve = curve;
            light.timeMax = duration;
        }
        private static void SetMaterialAndMesh(this GameObject gameObject, Material mat, Mesh mesh)
        {
            if (gameObject.TryGetComponent<ParticleSystemRenderer>(out var renderer))
            {
                renderer.sharedMaterial = mat;
                renderer.mesh = mesh;
            }
        }
        private static void SetMaterialAndMeshes(this GameObject gameObject, Material mat, Mesh[] mesh)
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
