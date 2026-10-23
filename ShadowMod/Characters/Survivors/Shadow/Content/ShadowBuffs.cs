using BepInEx.Configuration;
using LookingGlass;
using LookingGlass.LookingGlassLanguage;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowBuffs
    {
        public static BuffDef boostBuff;
        public static BuffDef chaosMarkDebuff;
        public static BuffDef chaosBoostBuff;

        public static void Init()
        {
            boostBuff = Modules.Content.CreateAndAddBuff("bdShadowBoost",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/CloakSpeed").iconSprite,
                ShadowSurvivor.shadowYellowColor,
                false,
                false);
            chaosMarkDebuff = Modules.Content.CreateAndAddBuff("bdShadowChaosMark",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/TeslaField").iconSprite,
                ShadowSurvivor.shadowYellowColor,
                false,
                true, false, BuffDef.StackingDisplayMethod.Default, BuffDef.Flags.ExcludeFromNoxiousThorns);
            chaosBoostBuff = Modules.Content.CreateAndAddBuff("bdShadowChaosBoost",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/CloakSpeed").iconSprite,
                ShadowSurvivor.shadowRedColor,
                true,
                false);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LookingGlass.PluginInfo.PLUGIN_GUID))
            {
                RoR2Application.onLoad += LookingGlassSetup;
            }
        }

        private static void LookingGlassSetup()
        {
            if (Language.languagesByName.TryGetValue("en", out RoR2.Language en))
            {
                RegisterLookingGlassBuff(en, boostBuff, "Shadow Boost", $"Gain <style=cIsUtility>+{ShadowStaticValues.boostArmor} armor</style>. Gain <style=cIsUtility>+{ShadowStaticValues.boostListedSpeedCoefficient * 100}% movement speed</style>.");
                RegisterLookingGlassBuff(en, chaosMarkDebuff, "Shadow Chaos Mark", $"{HedgehogUtils.Helpers.wipIcon}");
                RegisterLookingGlassBuff(en, chaosBoostBuff, "Shadow Chaos Boost", $"{HedgehogUtils.Helpers.wipIcon}");
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void RegisterLookingGlassBuff(Language lang, BuffDef buff, string name, string description)
        {
            LookingGlassLanguageAPI.SetupToken(lang, $"NAME_{buff.name}", name);
            LookingGlassLanguageAPI.SetupToken(lang, $"DESCRIPTION_{buff.name}", description);
        }
    }
}
