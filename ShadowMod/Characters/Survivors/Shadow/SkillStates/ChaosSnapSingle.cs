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
    public class ChaosSnapSingle : BaseSkillState, ISkillState
    {
        // not actually using overlapattack collision, only for networking
        protected OverlapAttack attack = new OverlapAttack();
        public HurtBox target;

        protected const float baseDuration = 0.4f;
        protected float duration;
        protected float attackPercentTime;
        protected bool attacked;
        private string playbackRateParam = "Slash.playbackRate";
        private byte randomAnimationIndex;

        public override void OnEnter()
        {
            base.OnEnter();
            if (isAuthority) characterBody.isSprinting = true;
            duration = baseDuration / attackSpeedStat;
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(true);
            StartAimMode(duration * 1.3f);
            randomAnimationIndex = (byte)UnityEngine.Random.RandomRangeInt(0, 3);
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
            attack.damageType.AddModdedDamageType(HedgehogUtils.Launch.DamageTypes.launchOnKill);
            attack.pushAwayForce = ShadowStaticValues.chaosSnapFinalPushForce;
        }
        protected virtual void ModifyAttack(OverlapAttack attack)
        {

        }
        protected void FireAttack()
        {
            if (target && target.healthComponent && !target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible))
            {
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
            writer.Write(randomAnimationIndex);
            writer.Write(HurtBoxReference.FromHurtBox(target));
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            randomAnimationIndex = reader.ReadByte();
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}
