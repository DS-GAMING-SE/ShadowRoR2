using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosSnapIn : TeleportIntoState, ISkillState
    {
        public override Type nextStateType { get { return typeof(ChaosSnapPummel); } }
        public virtual Type nextSingleStateType { get { return typeof(ChaosSnapSingle); } }

        public bool rechargeFromChaosMark = true;

        public override void OnExit()
        {
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(false);
            if (target && target.healthComponent.body.HasBuff(ShadowBuffs.chaosMarkDebuff))
            {
                if (NetworkServer.active) target.healthComponent.body.RemoveOldestTimedBuff(ShadowBuffs.chaosMarkDebuff);
                ShadowDamageTypes.ConsumeChaosMark(characterBody, activatorSkillSlot, rechargeFromChaosMark);
            }
            base.OnExit();
        }
        protected override void SetNextState()
        {
            if (inputBank && inputBank.skill1.down)
            {
                base.SetNextState();
            }
            else
            {
                EntityState state = EntityStateCatalog.InstantiateState(nextSingleStateType);
                if (state is ChaosSnapSingle final)
                {
                    final.target = target;
                    final.activatorSkillSlot = activatorSkillSlot;
                    outer.SetNextState(final);
                    return;
                }
                outer.SetNextState(state);
            }
        }
    }
}
