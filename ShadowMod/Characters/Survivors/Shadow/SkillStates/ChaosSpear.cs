using EntityStates;
using RoR2;
using RoR2.Orbs;
using ShadowMod.Characters.Survivors.Shadow.Components;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosSpear : BaseState
    {
        private HomingTracker homingTracker;
        private HurtBox[] targets;

        public virtual int numAttacks { get { return 1; } }

        protected const float baseDuration = 0.6f;
        protected const float baseTimeBetweenAttacks = 0.05f;

        protected float duration;
        protected float timeBetweenAttacks;

        protected int attackCount;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration * attackSpeedStat;
            timeBetweenAttacks = baseTimeBetweenAttacks * attackSpeedStat;
            if (isAuthority)
            {
                homingTracker = GetComponent<HomingTracker>();
                targets = new HurtBox[numAttacks];
                targets[0] = homingTracker.GetTrackingTarget();
                homingTracker.locked = true;
            }
            FireSpearVFX();
            if (NetworkServer.active)
            {
                if (targets[attackCount]) FireSpear(targets[attackCount]);
            }
            attackCount = 1;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            while (attackCount < numAttacks && fixedAge > timeBetweenAttacks * attackCount)
            {
                if (targets[attackCount] && NetworkServer.active) FireSpear(targets[attackCount]);
                attackCount++;
            }
            if (isAuthority)
            {
                if (attackCount >= numAttacks)
                {
                    homingTracker.locked = false;
                }

                if (fixedAge > duration)
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            while (attackCount < numAttacks)
            {
                if (targets[attackCount] && NetworkServer.active) FireSpear(targets[attackCount]);
                attackCount++;
            }
            if (isAuthority)
            {
                homingTracker.locked = false;
            }
            base.OnExit();
        }

        protected virtual void FireSpearVFX()
        {

        }
        protected virtual void FireSpear(HurtBox target)
        {
            OrbManager.instance.AddOrb(CreateSpear(target));
        }
        protected virtual Orb CreateSpear(HurtBox target)
        {
            return ChaosSpearOrb.CreateChaosSpearOrb(characterBody, transform.position, target);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.WritePackedIndex32(numAttacks);
            writer.Write(homingTracker.GetTrackingTarget());
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            targets = new HurtBox[reader.ReadPackedIndex32()];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = reader.ReadHurtBoxReference().ResolveHurtBox();
            }
        }
    }
}
