using RoR2.Orbs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using R2API;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class ChaosSpearOrb : GenericDamageOrb
    {
        public static ChaosSpearOrb CreateChaosSpearOrb(CharacterBody attacker, Vector3 origin, HurtBox target)
        {
            ChaosSpearOrb orb = new ChaosSpearOrb();
            orb.speed = 120f;
            orb.damageType = new DamageTypeCombo(DamageType.Shock5s, DamageTypeExtended.Electrical, DamageSource.Secondary);
            orb.damageType.AddModdedDamageType(ShadowDamageTypes.chaosMark);
            orb.target = target;
            orb.origin = origin;
            orb.isCrit = attacker.RollCrit();
            orb.teamIndex = attacker.teamComponent.teamIndex;
            orb.damageValue = ShadowStaticValues.chaosSpearDamageCoefficient * attacker.damage;
            orb.attacker = attacker.gameObject;
            orb.procCoefficient = 1f;
            return orb;
        }
        
        public override GameObject GetOrbEffect()
        {
            return ShadowAssets.chaosSpearOrbEffect;
        }
    }
    public class KillCooldownOrb : Orb
    {
        public override void Begin()
        {
            base.duration = 0.25f;
            EffectData effectData = new EffectData
            {
                origin = this.origin,
                genericFloat = base.duration
            };
            effectData.SetHurtBoxReference(this.target);
            EffectManager.SpawnEffect(OrbStorageUtility.Get("Prefabs/Effects/OrbEffects/InfusionOrbEffect"), effectData, true);
        }
        public override void OnArrival()
        {
            if (target && target.healthComponent && target.healthComponent.body.skillLocator)
            {
                foreach (var skill in target.healthComponent.body.skillLocator.AllSkills)
                {
                    if (skill.skillDef is KillCooldownSkillDef)
                    {
                        skill.RunRecharge(1);
                    }
                }
            }
            base.OnArrival();
        }
    }
}
