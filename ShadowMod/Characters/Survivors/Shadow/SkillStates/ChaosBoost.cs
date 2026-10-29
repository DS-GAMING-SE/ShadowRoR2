using EntityStates;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosBoost : BaseSkillState, ISkillState
    {
        protected const float baseDuration = 0.5f;
        protected float duration;
        private BlastAttack attack;
        private CharacterMotor.WalkSpeedPenaltyModifier walkSpeedPenalty;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            PlayAnimation();
            ExplosionEffect();
            walkSpeedPenalty = characterMotor.AddWalkSpeedPenalty(0.5f);
            if (NetworkServer.active)
            {
                CreateAttack();
                ModifyAttack(attack);
                FireAttack();
                float buffDuration = ShadowStaticValues.chaosBoostMaxDuration
                    + (activatorSkillSlot.bonusStockFromBody * ShadowStaticValues.chaosBoostDurationPerExtraStock);
                characterBody.AddTimedBuff(ShadowBuffs.chaosBoostBuff, buffDuration, 3);
                characterBody.SetTimedBuffDurationIfPresent(ShadowBuffs.chaosBoostBuff, buffDuration, true);
                
            }
            StartAimMode(duration * 1.5f, true);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                walkSpeedPenalty.penalty = Mathf.Lerp(0.5f, 1f, fixedAge / duration);
                characterMotor.moveDirection = inputBank.moveVector;
                float clamp = fixedAge / duration;
                characterMotor.velocity.y = Mathf.Clamp(characterMotor.velocity.y, -clamp, clamp);
                if (fixedAge > duration)
                {
                    outer.SetNextStateToMain();
                }
            }
        }
        public override void OnExit()
        {
            characterBody.characterMotor.RemoveWalkSpeedPenalty(walkSpeedPenalty);
            base.OnExit();
        }
        protected virtual void PlayAnimation()
        {

        }
        protected virtual void ExplosionEffect()
        {
            EffectManager.SpawnEffect(ShadowAssets.chaosBoostActivationEffect, new EffectData { origin = transform.position, rootObject = gameObject, scale = ShadowStaticValues.chaosBoostRadius }, false);
        }
        protected void CreateAttack()
        {
            attack = new BlastAttack();
            attack.attacker = gameObject;
            attack.inflictor = gameObject;
            attack.crit = RollCrit();
            attack.procCoefficient = 1f;
            attack.baseDamage = ShadowStaticValues.chaosBoostDamageCoefficient * characterBody.damage;
            attack.damageType = DamageSource.Special;
            //attack.damageType.AddModdedDamageType(ShadowDamageTypes.chaosMark);
            attack.damageType.attackIdentityIndex = ShadowDamageTypes.shadowChaosMarkAttackIdentityIndex;
            attack.baseForce = 500f;
            attack.falloffModel = ShadowStaticValues.chaosBoostFalloff;
            attack.position = transform.position;
            attack.radius = ShadowStaticValues.chaosBoostRadius;
            attack.teamIndex = teamComponent.teamIndex;
        }
        protected virtual void ModifyAttack(BlastAttack attack)
        {

        }
        protected virtual void FireAttack()
        {
            attack.Fire();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Stun;
        }
    }
}
