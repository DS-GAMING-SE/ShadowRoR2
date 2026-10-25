using ShadowMod.Modules;
using ShadowMod.Characters.Survivors.Shadow.Achievements;
using LookingGlass.LookingGlassLanguage;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static ShadowMod.Modules.Tokens;
using static ShadowMod.Characters.Survivors.Shadow.ShadowStaticValues;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowTokens
    {
        public static void Init()
        {
            AddShadowTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddShadowTokens()
        {
            string prefix = ShadowSurvivor.SHADOW_PREFIX;

            string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, ?????.";
            string outroFailure = "..and so he vanished, ?????.";

            Language.Add(prefix + "NAME", "Shadow");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Ultimate Lifeform");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Mephiles");
            #endregion

            #region Common Keywords
            Language.Add(prefix + "KEYWORD_HOMING", $"{KeywordText("Homing", "Targets the enemy closest to the crosshair, indicated by the reticle.")}");
            Language.Add(prefix + "KEYWORD_CHAOS_MARK", $"{KeywordText("Chaos Mark", $"Using {UtilityText("Chaos Snap")} or killing the target will consume the mark to refresh {UtilityText("Chaos Snap")}'s cooldown.")}");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", $"{HedgehogUtils.Helpers.wipIcon} All of Me");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", $"With the <color=#e7c7e6>Artifact of Chaos Emeralds</color> enabled, Shadow can choose to {UtilityText("wield the Chaos Emeralds")}, accept the {RedText("dark influence of Black Doom")}, or forsake both and {HealingText("fight only with his own power")}.");
            Language.Add(prefix + "KEYWORD_HERO_PATH", $"{KeywordText($"{UtilityText("Hero")}", $"Collect the Chaos Emeralds. Improves your {UtilityText("mobility")} and increases your {UtilityText("Homing")} range.")}");
            Language.Add(prefix + "KEYWORD_DARK_PATH", $"{KeywordText($"{RedText("Dark")}", $"Accept Doom Powers at the end of each stage. Permanently upgrade your skills into more aggressive variants.")}");
            Language.Add(prefix + "KEYWORD_NEUTRAL_PATH", $"{KeywordText($"{HealingText("Neutral")}", $"Remove your Inhibitor Rings for a stage, gaining incredible power at the cost of disabling all of your items.")}");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_CHAOS_SNAP_NAME", $"Chaos Snap");
            Language.Add(prefix + "PRIMARY_CHAOS_SNAP_DESCRIPTION", $"{UtilityText("Homing")}. Teleport to the target and pummel them for {DamageValueText(chaosSnapPummelDamageCoefficient)} repeatedly while the skill is held. Release to {UtilityText("Launch")} the target for {DamageValueText(chaosSnapFinalDamageCoefficient)}. {HedgehogUtils.Helpers.wipIcon} Use your {UtilityText("secondary skill")} to {UtilityText("Launch")} the target up. {HedgehogUtils.Helpers.wipIcon} Use your {UtilityText("utility skill")} to teleport away from or behind the target.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_CHAOS_SPEAR_NAME", $"Chaos Spear");
            Language.Add(prefix + "SECONDARY_CHAOS_SPEAR_DESCRIPTION", DamageText("Shocking")+". "+ UtilityText("Homing")+". " + agilePrefix + $"Fire a Chaos Spear dealing {DamageValueText(chaosSpearDamageCoefficient)} and applying a {UtilityText("Chaos Mark")}.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_BOOST_NAME", $"Boost");
            Language.Add(prefix + "UTILITY_BOOST_DESCRIPTION", $"Spend boost meter to {UtilityText($"move {100f * boostListedSpeedCoefficient}% faster")} than normal. If airborne, do a short {UtilityText("mid-air dash")}.");
            #endregion

            #region Special
            Language.Add(prefix + "KEYWORD_KILL_COOLDOWN", $"{KeywordText("kill cooldown", $"This skill's cooldown is only charged by killing enemies. Additional skill stocks increase the skill's duration.")}");

            Language.Add(prefix + "SPECIAL_CHAOS_BOOST_NAME", $"{HedgehogUtils.Helpers.wipIcon} Chaos Boost");
            Language.Add(prefix + "SPECIAL_CHAOS_BOOST_DESCRIPTION", $"{RedText("kill cooldown")}. Release a burst of energy dealing {DamageValueText(chaosBoostDamageCoefficient)} and applying {UtilityText("Chaos Marks")}. Enhance your stats for {UtilityText(chaosBoostMaxDuration+"s")}. Killing {chaosBoostKillsToUpgrade} enemies will {UtilityText("upgrade")} the effect and reset its duration.");

            Language.Add(prefix + "SPECIAL_CHAOS_BLAST_NAME", $"{HedgehogUtils.Helpers.wipIcon} Chaos Blast");
            Language.Add(prefix + "SPECIAL_CHAOS_BLAST_DESCRIPTION", $"{HedgehogUtils.Helpers.wipIcon}");

            Language.Add(prefix + "SPECIAL_CHAOS_CONTROL_NAME", $"{HedgehogUtils.Helpers.wipIcon} Chaos Control");
            Language.Add(prefix + "SPECIAL_CHAOS_CONTROL_DESCRIPTION", $"{HedgehogUtils.Helpers.wipIcon}");
            #endregion

            #region Paths
            Language.Add(prefix + "HERO_PICKUP_SUFFIX", $"{UtilityText("HERO: The Chaos Emeralds enhance your innate chaos abilities, increasing the range of your Homing skills.")}");
            Language.Add(prefix + "DARK_PICKUP_SUFFIX", $"{RedText("DARK: Black Doom's power has permanently altered you, granting increased stats and more aggressive skills.")}");
            #endregion

            Language.Add(prefix + "VOICELINES_ENABLE_DESCRIPTION", $"Voicelines recorded by {UtilityText("MotherEggVA")}.");

            #region Achievements
            Language.Add(GetAchievementNameToken(ShadowMasteryAchievement.identifier), "Shadow: Mastery");
            Language.Add(GetAchievementDescriptionToken(ShadowMasteryAchievement.identifier), "As Shadow, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
