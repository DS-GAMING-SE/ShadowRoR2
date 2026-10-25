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
    public class ChaosSnapFinal : BaseSkillState, ISkillState
    {
        // not actually using overlapattack collision, only for networking
        protected OverlapAttack attack = new OverlapAttack();
        public HurtBox target;

        protected const float baseDuration = 0.4f;
        protected float duration;
        protected float attackPercentTime;
        protected bool attacked;
        private string playbackRateParam = "Slash.playbackRate";

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(true);
            if (target && NetworkServer.active) target.healthComponent.body.AddBuff(HedgehogUtils.Buffs.suppressDeathBuff);
            StartAimMode(duration * 1.3f);
            PlayAnimation();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                characterMotor.velocity.y = Mathf.Clamp(characterMotor.velocity.y, 3 * (-fixedAge / duration), 3 * (fixedAge / duration));
            }
            if (!attacked && fixedAge > duration * attackPercentTime)
            {
                Attack();
            }
            if (isAuthority && fixedAge > duration)
            {
                outer.SetNextStateToMain();
            }
        }
        protected void CreateAttack()
        {
            attack.attacker = gameObject;
            attack.inflictor = gameObject;
            attack.isCrit = RollCrit();
            attack.procCoefficient = 1f;
            attack.damage = ShadowStaticValues.chaosSnapFinalDamageCoefficient * characterBody.damage;
            attack.damageType = DamageSource.Primary;
            attack.damageType.AddModdedDamageType(HedgehogUtils.Launch.DamageTypes.launch);
            attack.damageType.AddModdedDamageType(HedgehogUtils.Launch.DamageTypes.removeLaunchAutoAim);
            attack.pushAwayForce = ShadowStaticValues.chaosSnapFinalPushForce;
        }
        protected virtual void ModifyAttack(OverlapAttack attack)
        {

        }
        protected void FireAttack()
        {
            if (target && target.healthComponent && !target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible))
            {
                if (NetworkServer.active) target.healthComponent.body.RemoveBuff(HedgehogUtils.Buffs.suppressDeathBuff);
                if (isAuthority) attack.ProcessHits([new OverlapAttack.OverlapInfo { hurtBox = target, hitPosition = target.collider.ClosestPoint(transform.position), pushDirection = GetAimRay().direction, hitOrigin = transform.position }]);
            }
        }
        private void Attack()
        {
            CreateAttack();
            ModifyAttack(attack);
            FireAttack();
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(false);
            attacked = true;
        }
        public override void OnExit()
        {
            if (!attacked) Attack();
            base.OnExit();
        }
        public virtual void PlayAnimation()
        {
            PlayCrossfade("Gesture, Override", "Slash1", playbackRateParam, duration, 0.1f * duration);
        }
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(HurtBoxReference.FromHurtBox(target));
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}
