using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using ShadowMod;
using ShadowMod.Characters.Survivors.Shadow;
using static ShadowMod.Characters.Survivors.Shadow.ShadowBuffs;
using static ShadowMod.Characters.Survivors.Shadow.ShadowStaticValues;

namespace ShadowMod.Modules
{
    internal class RecalculateStats
    {
        public static void Initialize()
        {
            RecalculateStatsAPI.GetStatCoefficients += ShadowRecalculateStats;
        }
        private static void ShadowRecalculateStats(CharacterBody self, RecalculateStatsAPI.StatHookEventArgs stats)
        {
            if (self)
            {
                if (self.HasBuff(boostBuff))
                {
                    HedgehogUtils.Boost.BoostLogic.BoostStats(self, stats, boostListedSpeedCoefficient);
                    stats.armorAdd += boostArmor;
                }
                if (self.HasBuff(chaosBoostBuff))
                {
                    float chaosBoostStats;
                    int chaosBoostArmor;
                    switch (self.GetBuffCount(chaosBoostBuff))
                    {
                        case 1:
                            chaosBoostStats = chaosBoost1Stats;
                            chaosBoostArmor = chaosBoost1Armor;
                            break;
                        case 2:
                            chaosBoostStats = chaosBoost2Stats;
                            chaosBoostArmor = chaosBoost2Armor;
                            break;
                        case 3:
                        default:
                            chaosBoostStats = chaosBoost3Stats;
                            chaosBoostArmor = chaosBoost3Armor;
                            break;
                    }
                    stats.attackSpeedMultAdd += chaosBoostStats;
                    stats.moveSpeedMultAdd += chaosBoostStats;
                    stats.armorAdd += chaosBoostArmor;
                }
                if (self.HasBuff(doomWingFormBuff))
                {
                    stats.attackSpeedMultAdd += doomWingFormStats;
                    stats.moveSpeedMultAdd += doomWingFormStats;
                    stats.damageMultAdd += doomWingFormDamage;
                    stats.armorAdd += doomWingFormArmor;
                }
                if (self.HasBuff(doomBoostBuff))
                {
                    HedgehogUtils.Boost.BoostLogic.BoostStats(self, stats, doomBoostListedSpeedCoefficient);
                    stats.armorAdd += boostArmor;
                }
                if (self.HasBuff(superBoostBuff))
                {
                    HedgehogUtils.Boost.BoostLogic.BoostStats(self, stats, superBoostListedSpeedCoefficient);
                    stats.armorAdd += boostArmor;
                }
            }
        }
    }
}
