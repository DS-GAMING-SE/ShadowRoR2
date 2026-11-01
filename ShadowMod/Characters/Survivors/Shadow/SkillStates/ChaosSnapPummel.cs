using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosSnapPummel : StickToTargetState, ISkillState
    {
        protected float attackTimer;
        protected OverlapAttack attack = new OverlapAttack();

        protected virtual Type nextStateType { get { return typeof(ChaosSnapFinal); } }

        private ICharacterGravityParameterProvider enemyGrav;
        protected bool canPushTarget;

        private CharacterCameraParamsData pummelCameraParams = new CharacterCameraParamsData
        {
            maxPitch = 80f,
            minPitch = -80f,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(-1.5f, 0f, -8),
            wallCushion = 0.1f
        };
        private CameraTargetParams.CameraParamsOverrideHandle camOverrideHandle;
        public override void OnEnter()
        {
            base.OnEnter();
            if (isAuthority) characterBody.isSprinting = false;
            if (cameraTargetParams) camOverrideHandle = cameraTargetParams.AddParamsOverride(new CameraTargetParams.CameraParamsOverrideRequest
            { 
                cameraParamsData = pummelCameraParams,
                priority = 1f}, 0.85f);
            attackTimer = ShadowStaticValues.chaosSnapPummelInterval / characterBody.attackSpeed;
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(true);
            if (target && target.healthComponent)
            {
                if (NetworkServer.active) target.healthComponent.body.AddBuff(HedgehogUtils.Buffs.suppressDeathBuff);
                enemyGrav = target.healthComponent.GetComponent<ICharacterGravityParameterProvider>();
                if (enemyGrav != null)
                {
                    var param = enemyGrav.gravityParameters;
                    param.channeledAntiGravityGranterCount++;
                    enemyGrav.gravityParameters = param;
                }
                canPushTarget = GetCanPushTarget();
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            attackTimer -= Time.fixedDeltaTime;
            while (attackTimer <= 0 && target && target.healthComponent && !target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible))
            {
                attackTimer += ShadowStaticValues.chaosSnapPummelInterval / characterBody.attackSpeed;
                if (isAuthority)
                {
                    CreateAttack();
                    ModifyAttack(attack);
                    FireAttack();
                }
            }
            if (isAuthority && (!target || !target.healthComponent || target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible) || !inputBank.skill1.down || fixedAge > ShadowStaticValues.chaosSnapPummelMaxDuration))
            {
                SetNextState(); // final hit
            }
        }
        protected virtual void SetNextState()
        {
            EntityState state = EntityStateCatalog.InstantiateState(nextStateType);
            if (state is ChaosSnapFinal final)
            {
                final.target = target;
                final.activatorSkillSlot = activatorSkillSlot;
                outer.SetNextState(final);
                return;
            }
            outer.SetNextState(state);
        }
        protected virtual bool GetCanPushTarget()
        {
            return HedgehogUtils.Launch.LaunchManager.AttackCanLaunch(target.healthComponent.body, characterBody, false, 1f, new Vector3(0, ShadowStaticValues.chaosSnapFinalPushForce, 0))
                    && target.healthComponent.body.hullClassification != HullClassification.BeetleQueen && !target.healthComponent.body.isChampion;
        }
        protected void CreateAttack()
        {
            attack.attacker = gameObject;
            attack.inflictor = gameObject;
            attack.isCrit = RollCrit();
            attack.procCoefficient = ShadowStaticValues.chaosSnapPummelProcCoefficient;
            attack.damage = ShadowStaticValues.chaosSnapPummelDamageCoefficient * characterBody.damage;
            attack.damageType = new DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.Primary);
            if (canPushTarget)
            {
                attack.physForceFlags = PhysForceFlags.resetVelocity | PhysForceFlags.disableAirControlUntilCollision | PhysForceFlags.massIsOne | PhysForceFlags.ignoreGroundStick;
                attack.pushAwayForce = ((Vector3.Dot(inputBank.moveVector.normalized, -offset.normalized) / 2) + 0.5f) * inputBank.moveVector.magnitude * 2f * characterBody.moveSpeed;
                attack.pushAwayForce *= target.healthComponent.body.hullClassification == HullClassification.Golem ? 0.75f : 1f;
                // doesn't work when enemy dies
            }
        }
        protected virtual void ModifyAttack(OverlapAttack attack)
        {

        }
        protected void FireAttack()
        {
            if (target && target.healthComponent && !target.healthComponent.body.HasBuff(RoR2Content.Buffs.Intangible))
            {
                attack.ProcessHits([new OverlapAttack.OverlapInfo { hurtBox = target, hitPosition = target.collider.ClosestPoint(transform.position), pushDirection = GetAimRay().direction, hitOrigin = transform.position }]);
            }
        }
        public override void OnExit()
        {
            if (target && NetworkServer.active) target.healthComponent.body.RemoveBuff(HedgehogUtils.Buffs.suppressDeathBuff);
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(false);
            if (enemyGrav != null)
            {
                var param = enemyGrav.gravityParameters;
                param.channeledAntiGravityGranterCount--;
                enemyGrav.gravityParameters = param;
            }
            if (cameraTargetParams) cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.7f);
            base.OnExit();
        }
    }
}
