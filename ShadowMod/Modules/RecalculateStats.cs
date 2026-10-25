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
            RecalculateStatsAPI.GetStatCoefficients += HenryRecalculateStats;
        }
        private static void HenryRecalculateStats(CharacterBody self, RecalculateStatsAPI.StatHookEventArgs stats)
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
                            chaosBoostStats = chaosBoost2Stats;
                            chaosBoostArmor = chaosBoost2Armor;
                            break;
                    }
                    stats.attackSpeedMultAdd += chaosBoostStats;
                    stats.moveSpeedMultAdd += chaosBoostStats;
                    stats.armorAdd += chaosBoostArmor;
                }
            }
        }
    }
}
