using System;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowStaticValues
    {
        public const float swordDamageCoefficient = 2.8f;

        public const float gunDamageCoefficient = 4.2f;

        public const float bombDamageCoefficient = 16f;

        #region Chaos Snap
        public const float chaosSnapPummelDamageCoefficient = 1f;
        public const float chaosSnapPummelFrequency = 3f;

        public const float chaosSnapPummelMaxDuration = 2f;

        public const float chaosSnapFinalDamageCoefficient = 6f;

        public const int chaosSnapStocks = 3;
        public const float chaosSnapCooldown = 4f;
        #endregion

        #region Chaos Spear
        public const float chaosSpearDamageCoefficient = 7f;

        public const float chaosSpearCooldown = 6f;

        public const float chaosMarkDuration = 10f;
        #endregion

        #region Boost
        public const float boostListedSpeedCoefficient = 0.35f;

        public const float boostArmor = 50;
        #endregion

        #region Chaos Boost
        public const float chaosBoostDamageCoefficient = 10f;

        public const float chaosBoostRadius = 20f;

        public const RoR2.BlastAttack.FalloffModel chaosBoostFalloff = RoR2.BlastAttack.FalloffModel.Linear;

        public const float chaosBoostMaxDuration = 30f;

        public const int chaosBoostKillsToCharge = 10;
        #endregion
    }
}