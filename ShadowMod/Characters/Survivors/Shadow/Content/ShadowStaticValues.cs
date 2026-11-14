using System;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowStaticValues
    {
        #region General
        public const float homingRange = 48f;
        public const float homingRangeHero = 75f;
        public const float homingRangeSuper = 90f;

        public const float chaosMarkDuration = 15f;
        #endregion

        #region Paths
        #region Doom
        public const int doomPowersMax = 3;
        #endregion
        #endregion

        #region Chaos Snap
        public const float chaosSnapPummelDamageCoefficient = 1.5f;
        public const float chaosSnapPummelProcCoefficient = 0.6f;
        public const float chaosSnapPummelFrequency = 4.4f;
        public const float chaosSnapPummelInterval = 1 / chaosSnapPummelFrequency;

        public const float chaosSnapPummelMaxDuration = 2f;

        public const float chaosSnapFinalDamageCoefficient = 7f;
        public const float chaosSnapFinalPushForce = 400f;

        public const int chaosSnapStocks = 3;
        public const float chaosSnapCooldown = 5f;

        #region Doom Blast
        public const float doomBlastExtraDamageCoefficient = 10f;
        public const float doomBlastFinalProcCoefficient = 1.5f;
        #endregion

        #endregion

        #region Chaos Spear
        public const float chaosSpearDamageCoefficient = 7f;

        public const float chaosSpearCooldown = 7f;

        #region Doom Spear
        public const int doomSpearCount = 5;
        #endregion

        #endregion

        #region Boost
        public const float boostListedSpeedCoefficient = 0.35f;

        public const float boostArmor = 50;
        #endregion

        #region Chaos Boost
        public const float chaosBoostDamageCoefficient = 3f;

        public const float chaosBoostRadius = 20f;

        public const RoR2.BlastAttack.FalloffModel chaosBoostFalloff = RoR2.BlastAttack.FalloffModel.HalfLinear;

        public const float chaosBoost1Stats = 0.3f;
        public const int chaosBoost1Armor = 50;

        public const float chaosBoost2Stats = 0.5f;
        public const int chaosBoost2Armor = 60;

        public const float chaosBoost3Stats = 0.65f;
        public const int chaosBoost3Armor = 70;

        public const float chaosBoostMaxDuration = 30f;
        public const float chaosBoostDurationPerExtraStock = 10f;

        public const int chaosBoostKillsToCharge = 10;
        public const int chaosBoostKillsToUpgrade = 10; // doesn't work
        #endregion

        #region Chaos Blast
        public const float chaosBlastDamageCoefficient = 10f;
        public const float chaosBlastFrequency = 4f;
        public const float chaosBlastInterval = 1 / chaosBlastFrequency;
        public const float chaosBlastDuration = 1.5f;
        public const float chaosBlastChargeDuration = 3f;
        public const float chaosBlastMinChargeDuration = 1f;

        public const float chaosBlastRadius = 30f;
        #endregion
    }
}