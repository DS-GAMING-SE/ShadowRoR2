using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using ShadowMod.Characters.Survivors.Shadow.Components;
using UnityEngine.Networking;
using HedgehogUtils.Miscellaneous;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public abstract class TeleportIntoState : BaseSkillState
    {
        protected HomingTracker homingTracker;
        protected HurtBox target;
        protected Vector3 offset;
        protected virtual float teleportInBaseDuration { get { return 0.35f; } }
        protected float teleportInDuration;
        protected virtual float baseDuration { get { return 0.5f; } }
        protected float duration;

        public abstract Type nextStateType { get; }
        private bool invincible;
        private ChaosSnapManager.ChaosSnapInstanceHandle handle;

        public override void OnEnter()
        {
            base.OnEnter();
            homingTracker = GetComponent<HomingTracker>();
            homingTracker.locked = true;
            target = homingTracker.GetTrackingTarget();
            if (!target) // this shouldn't be happening, yet it has. Probably need to rewrite trackingskilldef
            {
                outer.SetNextStateToMain(); 
                return;
            }
            teleportInDuration = teleportInBaseDuration / attackSpeedStat;
            duration = baseDuration / attackSpeedStat;
            offset = StickToTargetState.GetStickOffset(target, transform);
            handle = ChaosSnapManager.instance.Teleport(characterBody, target.transform.position + offset, teleportInDuration, teleportInDuration);
            PlayAnimation();
            var grav = characterMotor.gravityParameters;
            grav.channeledAntiGravityGranterCount++;
        }
        public override void Update()
        {
            base.Update();
            if (isAuthority)
            {
                if (age < teleportInDuration)
                {
                    if (target && handle != null)
                    {
                        handle.targetPosition = target.transform.position + offset;
                    }
                }
                else
                {
                    if (target && characterMotor)
                    {
                        characterMotor.Motor.MoveCharacter(target.transform.position + offset);
                    }
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active)
            {
                if (fixedAge >= teleportInDuration * 0.5f && !invincible)
                {
                    invincible = true;
                    characterBody.AddBuff(RoR2Content.Buffs.Intangible);
                }
                else if (fixedAge < teleportInDuration * 1.5f && invincible)
                {
                    invincible = false;
                    characterBody.RemoveBuff(RoR2Content.Buffs.Intangible);
                }
            }
            if (isAuthority)
            {
                characterMotor.velocity = Vector3.Lerp(characterMotor.velocity, Vector3.zero, fixedAge / teleportInDuration);
                if (fixedAge >= duration)
                {
                    EntityState state = EntityStateCatalog.InstantiateState(nextStateType);
                    if (state is StickToTargetState stick)
                    {
                        stick.target = target;
                        stick.offset = offset;
                        stick.activatorSkillSlot = activatorSkillSlot;
                        outer.SetNextState(stick);
                        return;
                    }
                    outer.SetNextState(state);
                }
            }
        }
        protected virtual void PlayAnimation()
        {

        }
        public override void OnExit()
        {
            homingTracker.locked = false;
            var grav = characterMotor.gravityParameters;
            grav.channeledAntiGravityGranterCount--;
            if (NetworkServer.active && invincible) characterBody.RemoveBuff(RoR2Content.Buffs.Intangible);
            base.OnExit();
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
