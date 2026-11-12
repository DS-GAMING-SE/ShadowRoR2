using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using EntityStates;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosBlastCharge : StickToTargetState
    {
        protected float chargeDuration;
        public virtual Type nextStateType { get { return typeof(ChaosBlast); } }


        private CameraTargetParams.AimRequest aimRequest;

        public override void OnEnter()
        {
            base.OnEnter();
            chargeDuration = ShadowStaticValues.chaosBlastMinChargeDuration + ((ShadowStaticValues.chaosBlastChargeDuration - ShadowStaticValues.chaosBlastMinChargeDuration) / attackSpeedStat);
            
            if (cameraTargetParams)
            {
                aimRequest = cameraTargetParams.RequestAimWithData(GetCameraOffset(), chargeDuration, 0.6f);
            }
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                if (target && target.healthComponent)
                {
                    target.healthComponent.body.AddBuff(HedgehogUtils.Buffs.hiddenRootBuff);
                }
            }
            if (target && target.healthComponent && Util.HasEffectiveAuthority(target.healthComponent.gameObject) && target.healthComponent.TryGetComponent<SetStateOnHurt>(out var hurt) && hurt.canBeStunned)
            {
                hurt.SetStun(chargeDuration * 1.1f);
            }
        }
        public virtual Vector3 GetCameraOffset()
        {
            return new Vector3(0f, 2.3f, -ShadowStaticValues.chaosBlastRadius * 2);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && fixedAge > chargeDuration)
            {
                BaseSkillState nextState = (BaseSkillState)EntityStateCatalog.InstantiateState(nextStateType);
                nextState.activatorSkillSlot = activatorSkillSlot;
                outer.SetNextState(nextState);
            }
        }
        public override void OnExit()
        {
            if (NetworkServer.active) 
            { 
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                if (target && target.healthComponent)
                {
                    target.healthComponent.body.RemoveBuff(HedgehogUtils.Buffs.hiddenRootBuff);
                }
            }
            if (cameraTargetParams && aimRequest != null)
            {
                aimRequest.Dispose();
            }
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Vehicle;
        }
    }
}
