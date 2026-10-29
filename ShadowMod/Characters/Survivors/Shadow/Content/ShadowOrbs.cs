using RoR2.Orbs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using R2API;
using R2API.Networking.Interfaces;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class ChaosSpearOrb : GenericDamageOrb
    {
        public static ChaosSpearOrb CreateChaosSpearOrb(CharacterBody attacker, Vector3 origin, HurtBox target)
        {
            ChaosSpearOrb orb = new ChaosSpearOrb();
            orb.speed = 120f;
            orb.damageType = new DamageTypeCombo(DamageType.Shock5s, DamageTypeExtended.Electrical, DamageSource.Secondary);
            //orb.damageType.AddModdedDamageType(ShadowDamageTypes.chaosMark);
            orb.damageType.attackIdentityIndex = ShadowDamageTypes.shadowChaosMarkAttackIdentityIndex;
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
            if (target && target.healthComponent) new NetworkKillCooldown(target.healthComponent.netId).Send(R2API.Networking.NetworkDestination.Clients);
            base.OnArrival();
        }
    }
}
