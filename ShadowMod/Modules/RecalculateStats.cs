using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using ShadowMod;
using ShadowMod.Characters.Survivors.Shadow;

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
                if (self.HasBuff(ShadowBuffs.boostBuff))
                {
                    HedgehogUtils.Boost.BoostLogic.BoostStats(self, stats, ShadowStaticValues.boostListedSpeedCoefficient);
                    stats.armorAdd += ShadowStaticValues.boostArmor;
                }
            }
        }
    }
}
