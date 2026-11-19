using EntityStates;
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
        public static ModdedDamageType chaosSpearShocking;
        public static ModdedDamageType doomSpearShocking;
        internal static void Initialize()
        {
            //chaosMark = ReserveDamageType();
            chaosSpearShocking = ReserveDamageType();
            doomSpearShocking = ReserveDamageType();
            On.RoR2.GlobalEventManager.OnHitEnemy += OnHitEnemy;
        }

        private static void OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if (damageInfo.damageType.HasModdedDamageType(chaosSpearShocking))
            {
                if (victim.TryGetComponent<SetStateOnHurt>(out var stun) && stun.canBeStunned)
                {
                    stun.SetCustomState(EntityStateCatalog.GetStateIndex(typeof(SkillStates.ShockStateChaosSpear)), InterruptPriority.Pain,
                        new SetStateOnHurt.CustomStateInfo { duration = 5f * damageInfo.procCoefficient });
                }
            }
            else if (damageInfo.damageType.HasModdedDamageType(doomSpearShocking))
            {
                if (victim.TryGetComponent<SetStateOnHurt>(out var stun) && stun.canBeStunned)
                {
                    stun.SetCustomState(EntityStateCatalog.GetStateIndex(typeof(SkillStates.DoomPowerUpgrades.ShockStateDoomSpear)), InterruptPriority.Pain,
                        new SetStateOnHurt.CustomStateInfo { duration = 5f * damageInfo.procCoefficient });
                }
            }
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
            if (recharge && characterBody.hasAuthority && skillSlot.skillDef is ChaosSnapSkillDef && skillSlot.CanApplyAmmoPack())
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
