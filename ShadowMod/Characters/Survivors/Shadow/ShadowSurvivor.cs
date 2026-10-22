using BepInEx.Configuration;
using ShadowMod.Modules;
using ShadowMod.Modules.Characters;
using ShadowMod.Characters.Survivors.Shadow.Components;
using ShadowMod.Characters.Survivors.Shadow.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.ContentManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using HG;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class ShadowSurvivor : SurvivorBase<ShadowSurvivor>
    {
        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "ShadowTheHedgehogBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "ShadowTheHedgehogMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabGUID => ShadowAssets.characterModel.AssetGUID;
        public override string displayPrefabGUID => ShadowAssets.displayPrefab.AssetGUID;

        public const string SHADOW_PREFIX = ShadowPlugin.DEVELOPER_PREFIX + "_SHADOW_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => SHADOW_PREFIX;

        public static Color shadowBlackColor = new Color(0f, 0.02f, 0.1f);
        public static Color shadowRedColor = new Color(0.9f, 0.1f, 0.3f);
        public static Color shadowYellowColor = new Color(1f, 0.75f, 0.1f);

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = SHADOW_PREFIX + "NAME",
            subtitleNameToken = SHADOW_PREFIX + "SUBTITLE",

            characterPortrait = ShadowAssets.characterIcon.LoadAssetAsync().WaitForCompletion(),
            bodyColor = shadowRedColor,
            sortPosition = 138,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1f,
            armor = 0f,
            moveSpeed = 8.5f,
            acceleration = 50f,

            jumpCount = 2,
        };

        public override CustomRendererInfo[] customRendererInfos =>
                [new CustomRendererInfo
                {
                    childName = "Model",
                    materialReference = ShadowAssets.defaultSkinMaterial
                },
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    materialReference = ShadowAssets.defaultSkinMaterial
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                    materialReference = ShadowAssets.defaultSkinMaterial
                }];

        public override UnlockableDef characterUnlockableDef => ShadowUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => null;

        //set in base classes

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Henry");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            ShadowUnlockables.Init();

            base.InitializeCharacter();

            ShadowConfig.Init();
            ShadowStates.Init();
            ShadowTokens.Init();

            ShadowAssets.Init();
            ShadowBuffs.Init();

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            RecalculateStats.Initialize();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<HedgehogUtils.Miscellaneous.MomentumPassive>();
            bodyPrefab.AddComponent<HedgehogUtils.Boost.BoostLogic>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = SHADOW_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = SHADOW_PREFIX + "PASSIVE_DESCRIPTION",
                icon = ShadowAssets.passiveSkillIcon.LoadAssetAsync().WaitForCompletion(),
            };
            #region Multiple Passives
            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            /*GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryPassive",
                skillNameToken = HENRY_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = HENRY_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);*/
            #endregion
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "HenrySlash",
                    SHADOW_PREFIX + "PRIMARY_SLASH_NAME",
                    SHADOW_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    ShadowAssets.primarySkillIcon.LoadAssetAsync().WaitForCompletion(),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 2;
            primarySkillDef1.stepGraceDuration = 0.5f;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = SHADOW_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = SHADOW_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = ShadowAssets.secondarySkillIcon.LoadAssetAsync().WaitForCompletion(),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            /*SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = SHADOW_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = SHADOW_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = ShadowAssets.utilitySkillIcon.LoadAssetAsync().WaitForCompletion(),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Roll)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });*/

            SkillDef boostSkillDef = HedgehogUtils.Boost.SkillDefs.CreateBoostSkillDef<HedgehogUtils.Boost.SkillDefs.BoostSkillDef>(
                SHADOW_PREFIX + "UTILITY_BOOST_NAME",
                SHADOW_PREFIX + "UTILITY_BOOST_DESCRIPTION",
                ShadowAssets.utilitySkillIcon.LoadAssetAsync().WaitForCompletion(),
                new EntityStates.SerializableEntityStateType(typeof(Boost)),
                new EntityStates.SerializableEntityStateType(typeof(BoostIdle)),
                new EntityStates.SerializableEntityStateType(typeof(Brake)),
                shadowYellowColor);

            Skills.AddUtilitySkills(bodyPrefab, boostSkillDef);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryBomb",
                skillNameToken = SHADOW_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = SHADOW_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = ShadowAssets.specialSkillIcon.LoadAssetAsync().WaitForCompletion(),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ModelSkinController skinController2 = displayPrefab.gameObject.AddComponent<ModelSkinController>();
            skinController._animatorControllerAddress = ShadowAssets.animator;
            skinController._avatarAddress = ShadowAssets.animatorAvatar;
            skinController2._animatorControllerAddress = ShadowAssets.displayAnimator;
            skinController2._avatarAddress = ShadowAssets.animatorAvatar;
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();
            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            AssetAsyncReferenceManager<Material>.LoadAsset(ShadowAssets.defaultSkinMaterial).Completed += delegate (AsyncOperationHandle<Material> x)
            { x.Result.SetHopooMaterial(); };
            AssetAsyncReferenceManager<Material>.LoadAsset(ShadowAssets.masterySkinMaterial).Completed += delegate (AsyncOperationHandle<Material> x)
            { x.Result.SetHopooMaterial(); };

            SkinDefParams defaultSkinDefParams = ScriptableObject.CreateInstance<SkinDefParams>();
            defaultSkinDefParams.rendererInfos = ArrayUtils.Clone(defaultRendererinfos);
            defaultSkinDefParams.meshReplacements =  
            [ new SkinDefParams.MeshReplacement { meshAddress = ShadowAssets.defaultSkinMesh, renderer = defaultRendererinfos[0].renderer },
            new SkinDefParams.MeshReplacement { meshAddress = ShadowAssets.defaultSkinSwordMesh, renderer = defaultRendererinfos[1].renderer },
            new SkinDefParams.MeshReplacement { meshAddress = ShadowAssets.defaultSkinGunMesh, renderer = defaultRendererinfos[2].renderer }];
            R2API.SkinDefParamsInfo defaultSkinParamsInfo = new R2API.SkinDefParamsInfo
            {
                Name = SHADOW_PREFIX + "DEFAULT_SKIN_NAME",
                NameToken = "DEFAULT_SKIN",
                Icon = Addressables.LoadAssetAsync<Sprite>(ShadowAssets.defaultSkinIcon).WaitForCompletion(),
                UnlockableDef = null,
                RootObject = prefabCharacterModel.gameObject,
                SkinDefParams = defaultSkinDefParams
            };
            SkinDef defaultSkin = R2API.Skins.CreateNewSkinDef(defaultSkinParamsInfo);
            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            SkinDefParams masterySkinDefParams = ScriptableObject.CreateInstance<SkinDefParams>();
            masterySkinDefParams.rendererInfos = ArrayUtils.Clone(defaultRendererinfos);
            masterySkinDefParams.rendererInfos[0].defaultMaterialAddress = ShadowAssets.masterySkinMaterial;
            masterySkinDefParams.rendererInfos[1].defaultMaterialAddress = ShadowAssets.masterySkinMaterial;
            masterySkinDefParams.rendererInfos[2].defaultMaterialAddress = ShadowAssets.masterySkinMaterial;
            masterySkinDefParams.meshReplacements =
            [ new SkinDefParams.MeshReplacement { meshAddress = ShadowAssets.masterySkinMesh, renderer = defaultRendererinfos[0].renderer },
            new SkinDefParams.MeshReplacement { meshAddress = ShadowAssets.masterySkinSwordMesh, renderer = defaultRendererinfos[1].renderer },
            new SkinDefParams.MeshReplacement { meshAddress = ShadowAssets.defaultSkinGunMesh, renderer = defaultRendererinfos[2].renderer }];
            R2API.SkinDefParamsInfo masterySkinParamsInfo = new R2API.SkinDefParamsInfo
            {
                Name = SHADOW_PREFIX + "MASTERY_SKIN_NAME",
                NameToken = SHADOW_PREFIX + "MASTERY_SKIN_NAME",
                Icon = Addressables.LoadAssetAsync<Sprite>(ShadowAssets.masterySkinIcon).WaitForCompletion(),
                UnlockableDef = ShadowUnlockables.masterySkinUnlockableDef,
                RootObject = prefabCharacterModel.gameObject,
                SkinDefParams = masterySkinDefParams
            };
            SkinDef masterySkin = R2API.Skins.CreateNewSkinDef(masterySkinParamsInfo);
            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();
            skinController2.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            ShadowAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }
    }
}