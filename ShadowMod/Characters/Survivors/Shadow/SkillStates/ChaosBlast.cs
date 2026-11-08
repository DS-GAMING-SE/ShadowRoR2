using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using RoR2;
using EntityStates;
using UnityEngine;
using ShadowMod.Characters.Survivors.Shadow.Components;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosBlast : BaseSkillState, ISkillState
    {
        protected float attackInterval;
        protected int numAttacks;
        protected int maxAttacks;
        private CameraTargetParams.AimRequest aimRequest;
        protected BlastAttack attack; // switch to overlap?
        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            attackInterval = ShadowStaticValues.chaosBlastInterval / attackSpeedStat;
            maxAttacks = Mathf.FloorToInt(ShadowStaticValues.chaosBlastDuration / attackInterval);
            if (cameraTargetParams)
            {
                aimRequest = cameraTargetParams.RequestAimWithData(GetCameraOffset(), 0f, 1.5f);
            }
            var grav = characterMotor.gravityParameters;
            grav.channeledAntiGravityGranterCount++;
            ExplosionEffect();
            if (isAuthority)
            {
                CreateAttack();
                ModifyAttack(attack);
                FireAttack();
            }
            numAttacks = 1;
        }
        public virtual Vector3 GetCameraOffset()
        {
            return new Vector3(0f, 2.3f, -ShadowStaticValues.chaosBlastRadius * 2f);
        }
        protected virtual void ExplosionEffect()
        {
            EffectManager.SpawnEffect(ShadowAssets.chaosBlastExplosionEffect, new EffectData { origin = transform.position, rootObject = gameObject, scale = ShadowStaticValues.chaosBlastRadius }, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                characterMotor.velocity = Vector3.zero;
                if (numAttacks < maxAttacks && fixedAge > attackInterval * numAttacks)
                {
                    CreateAttack();
                    ModifyAttack(attack);
                    FireAttack();
                    numAttacks++;
                }
                if (fixedAge > ShadowStaticValues.chaosBlastDuration)
                {
                    outer.SetNextStateToMain();
                }
            }
        }
        protected void CreateAttack()
        {
            attack = new BlastAttack();
            attack.attacker = gameObject;
            attack.inflictor = gameObject;
            attack.crit = RollCrit();
            attack.procCoefficient = 1f;
            attack.baseDamage = ShadowStaticValues.chaosBlastDamageCoefficient * characterBody.damage;
            attack.damageType = DamageSource.Special;
            attack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            attack.baseForce = 500f;
            attack.falloffModel = BlastAttack.FalloffModel.None;
            attack.position = transform.position;
            attack.radius = ShadowStaticValues.chaosBlastRadius;
            attack.teamIndex = teamComponent.teamIndex;
        }
        protected virtual void ModifyAttack(BlastAttack attack)
        {

        }
        protected virtual void FireAttack()
        {
            attack.Fire();
        }

        public override void OnExit()
        {
            if (NetworkServer.active) 
            { 
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.7f);
            }
            while (isAuthority && numAttacks < maxAttacks)
            {
                CreateAttack();
                ModifyAttack(attack);
                FireAttack();
                numAttacks++;
            }
            if (cameraTargetParams && aimRequest != null)
            {
                aimRequest.Dispose();
            }
            var grav = characterMotor.gravityParameters;
            grav.channeledAntiGravityGranterCount--;
            base.OnExit();
        }
    }
}
