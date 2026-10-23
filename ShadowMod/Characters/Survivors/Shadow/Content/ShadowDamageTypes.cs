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
        public static ModdedDamageType chaosMark;
        internal static void Initialize()
        {
            chaosMark = ReserveDamageType();

            On.RoR2.GlobalEventManager.ProcessHitEnemy += ProcessHitEnemy;
        }

        private static void ProcessHitEnemy(On.RoR2.GlobalEventManager.orig_ProcessHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if (damageInfo.HasModdedDamageType(chaosMark) && victim.TryGetComponent<CharacterBody>(out var body))
            {
                body.AddTimedBuff(ShadowBuffs.chaosMarkDebuff, ShadowStaticValues.chaosMarkDuration);
            }
        }
    }
}
