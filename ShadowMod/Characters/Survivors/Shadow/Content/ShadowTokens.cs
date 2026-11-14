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

            string desc = "Shadow is a fast and aggressive melee survivor who tears through enemies individually ---reword?---.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Shadow's lack of group damage can be circumvented with smart use of his Launching ability." + Environment.NewLine + Environment.NewLine
             + "< ! > Chaos Snap is your go-to attack for any situation. Use it in tandem with your other skills to avoid its cooldown." + Environment.NewLine + Environment.NewLine
             + "< ! > Chaos Spear -." + Environment.NewLine + Environment.NewLine
             + "< ! > Chaos Boost marks all nearby enemies, letting you attack without restraint and recharge the skill at the same time. Use it at the right time and watch your power snowball." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, ?????.";
            string outroFailure = "..and so he vanished, ?????.";

            Language.Add(prefix + "NAME", "Shadow");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Ultimate Lifeform");
            //Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Mephiles");
            #endregion

            #region Common Keywords
            Language.Add(prefix + "KEYWORD_HOMING", KeywordText("Homing", "Targets the enemy closest to the crosshair, indicated by the reticle."));
            Language.Add(prefix + "KEYWORD_CHAOS_MARK", KeywordText("Chaos Mark", $"Using {UtilityText("Chaos Snap")} or killing the target will consume the mark to refresh {UtilityText("Chaos Snap")}'s cooldown."));
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", $"{HedgehogUtils.Helpers.wipIcon} All of Me");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", $"With the <color=#e7c7e6>Artifact of Chaos Emeralds</color> enabled, Shadow can choose to {UtilityText("wield the Chaos Emeralds")}, accept the {RedText("dark influence of Black Doom")}, or forsake both and {HealingText("fight only with his own power")}.");
            Language.Add(prefix + "KEYWORD_HERO_PATH", KeywordText($"{UtilityText("Hero")}", $"Collect the Chaos Emeralds. Improves your {UtilityText("mobility")} and increases your {UtilityText("Homing")} range."));
            Language.Add(prefix + "KEYWORD_DARK_PATH", KeywordText($"{RedText("Dark")}", $"Accept Doom Powers at the end of each stage. {RedText("Permanently upgrade your skills")} into more aggressive variants."));
            Language.Add(prefix + "KEYWORD_NEUTRAL_PATH", KeywordText($"{HealingText("Neutral")}", $"{UtilityText("Upon death")}, {UtilityText("return to life")} and remove your inhibitor rings for the rest of the stage, giving {DamageText("incredible power")} but {HealthText("disabling your items")}."));
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_CHAOS_SNAP_NAME", $"Chaos Snap");
            Language.Add(prefix + "PRIMARY_CHAOS_SNAP_DESCRIPTION", $"{UtilityText("Homing")}. Teleport to the target and pummel them for {DamageValueText(chaosSnapPummelDamageCoefficient)} repeatedly while the skill is held. Release to {UtilityText("Launch")} the target for {DamageValueText(chaosSnapFinalDamageCoefficient)}. {HedgehogUtils.Helpers.wipIcon} Use your {UtilityText("utility skill")} to teleport away from or behind the target.");

            #region Doom
            Language.Add(prefix + "PRIMARY_DOOM_BLAST_NAME", RedText(HedgehogUtils.Helpers.wipIcon + " Doom Blast"));
            Language.Add(prefix + "PRIMARY_DOOM_BLAST_DESCRIPTION", $"{UtilityText("Homing")}. Teleport to the target and pummel them for {DamageValueText(chaosSnapPummelDamageCoefficient)} repeatedly while the skill is held. Release to {UtilityText("Launch")} the target for {DamageValueText(chaosSnapFinalDamageCoefficient)}. {HedgehogUtils.Helpers.wipIcon} Use your {UtilityText("utility skill")} to teleport away from or behind the target. Hold for the skill's entire duration to attack again for {RedText($"{doomBlastExtraDamageCoefficient * 100f}%")}");
            Language.Add(prefix + "PRIMARY_DOOM_BLAST_PICKUP", $"Hold for the skill's entire duration to attack again for {RedText($"{doomBlastExtraDamageCoefficient * 100f}%")}. {RedText("Corrupts Chaos Snap")}.");
            #endregion
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_CHAOS_SPEAR_NAME", $"Chaos Spear");
            Language.Add(prefix + "SECONDARY_CHAOS_SPEAR_DESCRIPTION", DamageText("Shocking")+". "+ UtilityText("Homing")+". " + agilePrefix + $"Fire a Chaos Spear dealing {DamageValueText(chaosSpearDamageCoefficient)} and applying a {UtilityText("Chaos Mark")}.");
            
            #region Doom
            Language.Add(prefix + "SECONDARY_DOOM_SPEAR_NAME", RedText(HedgehogUtils.Helpers.wipIcon + "Doom Spear"));
            Language.Add(prefix + "SECONDARY_DOOM_SPEAR_DESCRIPTION", DamageText("Shocking") + ". " + UtilityText("Homing") + ". " + agilePrefix + $"Fire Doom Spears {RedText($"at up to {doomSpearCount} targets")} dealing {DamageValueText(chaosSpearDamageCoefficient)} and applying {UtilityText("Chaos Marks")}.");
            Language.Add(prefix + "SECONDARY_DOOM_SPEAR_PICKUP", $"Fire Doom Spears {RedText($"at up to {doomSpearCount} targets")} at once. {RedText("Corrupts Chaos Spear")}.");
            #endregion

            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_BOOST_NAME", $"Boost");
            Language.Add(prefix + "UTILITY_BOOST_DESCRIPTION", $"Spend boost meter to {UtilityText($"move {100f * boostListedSpeedCoefficient}% faster")} than normal. If airborne, do a short {UtilityText("mid-air dash")}.");
            #endregion

            #region Special
            Language.Add(prefix + "KEYWORD_KILL_COOLDOWN", KeywordText("kill cooldown", $"Charge this skill's cooldown by killing enemies or consuming {UtilityText("Chaos Marks")}. Additional charges increase the skill's duration."));
            #region Chaos Boost
            Language.Add(prefix + "SPECIAL_CHAOS_BOOST_NAME", $"Chaos Boost");
            Language.Add(prefix + "SPECIAL_CHAOS_BOOST_DESCRIPTION", $"{RedText("kill cooldown")}. Release a burst of energy dealing {DamageValueText(chaosBoostDamageCoefficient)} and applying {UtilityText("Chaos Marks")}. Enhance your stats for {UtilityText(chaosBoostMaxDuration+"s")}. Reactivate to {UtilityText("upgrade")} the effect up to {DamageText("3")} times and reset its duration. After {DamageText("3")} uses, unleash {DeathText("Chaos Blast")}.");

            #region Doom
            Language.Add(prefix + "SPECIAL_DOOM_WING_NAME", RedText(HedgehogUtils.Helpers.wipIcon + "Doom Wing"));
            Language.Add(prefix + "SPECIAL_DOOM_WING_DESCRIPTION", $"{RedText("kill cooldown")}. Release a burst of energy dealing {DamageValueText(chaosBoostDamageCoefficient)} and applying {UtilityText("Chaos Marks")}. Grow wings, {RedText("greatly enhancing your stats")} and gaining {RedText("flight while boosting")} for {UtilityText(chaosBoostMaxDuration + "s")}. Reactivate to reset its duration. After {DamageText("3")} uses, unleash {DeathText("Chaos Blast")}.");
            Language.Add(prefix + "SPECIAL_DOOM_WING_PICKUP", $"Grow Doom Wings, {RedText($"greatly increasing your stats")} and allowing you to {RedText("fly while boosting")}. {RedText("Corrupts Chaos Boost")}.");
            #endregion
            #endregion

            #region Chaos Blast
            string chaosBlastDescription = $"{UtilityText("Homing")}. Hold the target in place and release a massive explosion dealing {DamageValueText(chaosBlastDamageCoefficient)} repeatedly.";
            Language.Add(prefix + "KEYWORD_CHAOS_BLAST", KeywordText("Chaos Blast", chaosBlastDescription));
            Language.Add(prefix + "SPECIAL_CHAOS_BLAST_NAME", $"Chaos Blast");
            Language.Add(prefix + "SPECIAL_CHAOS_BLAST_DESCRIPTION", chaosBlastDescription);
            #endregion

            #region Chaos Control
            Language.Add(prefix + "SPECIAL_CHAOS_CONTROL_NAME", $"{HedgehogUtils.Helpers.wipIcon} Chaos Control");
            Language.Add(prefix + "SPECIAL_CHAOS_CONTROL_DESCRIPTION", $"{HedgehogUtils.Helpers.wipIcon}");
            #endregion

            #endregion

            #region Paths
            Language.Add(prefix + "HERO_PICKUP_SUFFIX", $"{UtilityText("This marks the path you've chosen.")}");
            Language.Add(prefix + "DARK_PICKUP_SUFFIX", $"{RedText("This marks the path you've chosen.")}");

            Language.Add(prefix + "ITEM_DOOM_POWER", $"{HedgehogUtils.Helpers.wipIcon} Doom Power");
            string doomPowerDescription = $"{RedText("Permanently upgrade your skills")} into more aggressive variants.";
            Language.Add(prefix + "ITEM_DOOM_POWER_PICKUP", doomPowerDescription);
            Language.Add(prefix + "ITEM_DOOM_POWER_DESC", doomPowerDescription);
            //Language.Add(prefix + "ITEM_DOOM_POWER_LORE", "");

            Language.Add(prefix + "BLACK_DOOM_SPAWN", RedText("Black Doom appears.."));
            Language.Add(prefix + "BLACK_DOOM_NAME", "Black Doom");
            Language.Add(prefix + "BLACK_DOOM_CONTEXT", "Accept Doom's Power..?");
            Language.Add(prefix + "BLACK_DOOM_DESCRIPTION", "As Shadow, gain a Doom Power, which permanently upgrades one of your skills.\n\nThis will set you on the Dark path for the rest of the run, making you unable to use the Chaos Emeralds.");
            #endregion

            Language.Add(prefix + "VOICELINES_ENABLE_DESCRIPTION", $"Voicelines recorded by {UtilityText("MotherEggVA")}.");

            #region Achievements
            Language.Add(GetAchievementNameToken(ShadowMasteryAchievement.identifier), "Shadow: Mastery");
            Language.Add(GetAchievementDescriptionToken(ShadowMasteryAchievement.identifier), "As Shadow, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
