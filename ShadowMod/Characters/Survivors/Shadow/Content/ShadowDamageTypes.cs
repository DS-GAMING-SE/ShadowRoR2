using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
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
        public static void ConsumeChaosMark(CharacterBody characterBody, GenericSkill skillSlot, bool recharge = true)
        {
            if (recharge && characterBody.hasAuthority && skillSlot.CanApplyAmmoPack())
            {
                skillSlot.ApplyAmmoPack();
            }
            if (NetworkServer.active && characterBody && characterBody.inventory)
            {
                characterBody.inventory.GiveItemRented(KillCooldownSkillDef.killCooldownChargeItemDef.itemIndex);
            }
            EffectManager.SpawnEffect(ShadowAssets.chaosMarkConsumeEffect, new EffectData { origin = skillSlot.characterBody.corePosition, rootObject = skillSlot.characterBody.mainHurtBox.gameObject }, false);
        }
    }
}
