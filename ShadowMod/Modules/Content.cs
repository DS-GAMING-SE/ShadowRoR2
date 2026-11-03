using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ShadowMod.Modules
{
    //consolidate contentaddition here in case something breaks and/or want to move to r2api
    internal class Content
    {
        internal static void AddCharacterBodyPrefab(GameObject bprefab)
        {
            ContentPacks.bodyPrefabs.Add(bprefab);
        }

        internal static void AddMasterPrefab(GameObject prefab)
        {
            ContentPacks.masterPrefabs.Add(prefab);
        }

        internal static void AddProjectilePrefab(GameObject prefab)
        {
            ContentPacks.projectilePrefabs.Add(prefab);
        }

        internal static void AddSurvivorDef(SurvivorDef survivorDef)
        {

            ContentPacks.survivorDefs.Add(survivorDef);
        }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix) { CreateSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, null, 100f); }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, float sortPosition) { CreateSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, null, sortPosition); }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, UnlockableDef unlockableDef) { CreateSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, unlockableDef, 100f); }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, UnlockableDef unlockableDef, float sortPosition)
        {
            SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
            survivorDef.bodyPrefab = bodyPrefab;
            survivorDef.displayPrefab = displayPrefab;
            survivorDef.primaryColor = charColor;

            survivorDef.cachedName = bodyPrefab.name.Replace("Body", "");
            survivorDef.displayNameToken = tokenPrefix + "NAME";
            survivorDef.descriptionToken = tokenPrefix + "DESCRIPTION";
            survivorDef.outroFlavorToken = tokenPrefix + "OUTRO_FLAVOR";
            survivorDef.mainEndingEscapeFailureFlavorToken = tokenPrefix + "OUTRO_FAILURE";

            survivorDef.desiredSortPosition = sortPosition;
            survivorDef.unlockableDef = unlockableDef;

            Modules.Content.AddSurvivorDef(survivorDef);
        }

        internal static void AddUnlockableDef(UnlockableDef unlockableDef)
        {
            ContentPacks.unlockableDefs.Add(unlockableDef);
        }
        internal static UnlockableDef CreateAndAddUnlockbleDef(string identifier, string nameToken, AssetReferenceT<Sprite> achievementIcon)
        {
            UnlockableDef unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
            unlockableDef.cachedName = identifier;
            unlockableDef.nameToken = nameToken;
            unlockableDef.achievementIconRef = achievementIcon;

            AddUnlockableDef(unlockableDef);

            return unlockableDef;
        }

        internal static void AddSkillDef(SkillDef skillDef)
        {
            ContentPacks.skillDefs.Add(skillDef);
        }

        internal static void AddSkillFamily(SkillFamily skillFamily)
        {
            ContentPacks.skillFamilies.Add(skillFamily);
        }

        internal static void AddEntityState(Type entityState)
        {
            ContentPacks.entityStates.Add(entityState);
        }

        internal static void AddBuffDef(BuffDef buffDef)
        {
            ContentPacks.buffDefs.Add(buffDef);
        }
        internal static BuffDef CreateAndAddBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff, bool isHidden = false, BuffDef.StackingDisplayMethod stackDisplayMethod = BuffDef.StackingDisplayMethod.Default, BuffDef.Flags flags = BuffDef.Flags.NONE)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;
            buffDef.isHidden = isHidden;
            buffDef.stackingDisplayMethod = stackDisplayMethod;
            buffDef.flags = flags;

            AddBuffDef(buffDef);

            return buffDef;
        }

        internal static void AddEffectDef(EffectDef effectDef)
        {
            ContentPacks.effectDefs.Add(effectDef);
        }
        internal static EffectDef CreateAndAddEffectDef(GameObject effectPrefab)
        {
            EffectDef effectDef = new EffectDef(effectPrefab);

            AddEffectDef(effectDef);

            return effectDef;
        }
        public static ItemDef AddItemDef(string itemName, string token, AssetReferenceT<Sprite> icon, AssetReferenceT<GameObject> pickupModel, ItemTierDef itemTierDef)
        {
            ItemDef itemDef = ScriptableObject.CreateInstance<ItemDef>();
            itemDef.name = itemName;
            itemDef.tier = ItemTier.AssignedAtRuntime;
            itemDef.pickupModelReference = pickupModel;
            itemDef.pickupIconSpriteReference = icon != null && icon.RuntimeKeyIsValid() ? icon : new AssetReferenceT<Sprite>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Core.texNullIcon_png);
            itemDef.canRemove = false;
            itemDef._itemTierDef = itemTierDef;
            if (!itemTierDef)
            {
                itemDef.hidden = true;
                itemDef.deprecatedTier = ItemTier.NoTier; itemDef.tier = ItemTier.NoTier; 
            }

            string prefix = ShadowMod.Characters.Survivors.Shadow.ShadowSurvivor.SHADOW_PREFIX + "ITEM_";

            itemDef.nameToken = prefix + token;
            itemDef.pickupToken = prefix + token + "_PICKUP";
            itemDef.descriptionToken = prefix + token + "_DESC";
            itemDef.loreToken = prefix + token + "_LORE";
            itemDef.tags =
            [
                ItemTag.CannotSteal,
                ItemTag.WorldUnique,
                ItemTag.CannotDisable,
                ItemTag.CanBeRental
            ];

            ContentPacks.itemDefs.Add(itemDef);

            return itemDef;
        }
        public static ItemTierDef AddItemTierDef(string token)
        {
            ItemTierDef tier = ScriptableObject.CreateInstance<ItemTierDef>();
            tier.tier = ItemTier.AssignedAtRuntime;
            tier.isDroppable = true;
            tier.canRestack = true;
            tier.canRebirth = false;
            tier.canScrap = false;
            tier.pickupRules = ItemTierDef.PickupRules.Default;
            tier.name = ShadowMod.Characters.Survivors.Shadow.ShadowSurvivor.SHADOW_PREFIX + "ITEM_TIER_" + token;
            tier.highlightPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_UI.HighlightTier3Item_prefab).WaitForCompletion();
            tier.dropletDisplayPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common.Tier3Orb_prefab).WaitForCompletion();
            tier.colorIndex = ColorCatalog.ColorIndex.Tier3Item;
            tier.darkColorIndex = ColorCatalog.ColorIndex.Tier3ItemDark;
            tier.bgIconTexture = Addressables.LoadAssetAsync<Texture>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_Common.texTier3BGIcon_png).WaitForCompletion();

            ContentPacks.itemTierDefs.Add(tier);

            return tier;
        }

        internal static void AddNetworkSoundEventDef(NetworkSoundEventDef networkSoundEventDef)
        {
            ContentPacks.networkSoundEventDefs.Add(networkSoundEventDef);
        }
        internal static NetworkSoundEventDef CreateAndAddNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            AddNetworkSoundEventDef(networkSoundEventDef);
            
            return networkSoundEventDef;
        }
    }
}