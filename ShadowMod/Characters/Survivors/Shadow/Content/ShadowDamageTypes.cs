using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static R2API.DamageAPI;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowDamageTypes
    {
        //public static ModdedDamageType chaosMark;
        public static AttackIdentityIndex shadowChaosMarkAttackIdentityIndex;
        internal static void Initialize()
        {
            //chaosMark = ReserveDamageType();
        }

        [DamageMacroMethod(assignToStaticReference = "shadowChaosMarkAttackIdentityIndex", index = AttackIdentityIndex.AssignAtRuntime, phase = (DamageMacroPhase.PostDamageCalculation), searchableName = "ShadowChaosMark")]
        private static void DamageMacroShadowChaosMark(ref DamageMacroData damageMacroData)
        {
            if (/*damageMacroData.damageInfo.HasModdedDamageType(chaosMark) &&*/ damageMacroData.victimCharacterBody)
            {
                damageMacroData.victimCharacterBody.AddTimedBuff(ShadowBuffs.chaosMarkDebuff, ShadowStaticValues.chaosMarkDuration);
            }
        }
    }
}
