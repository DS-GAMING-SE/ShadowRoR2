using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosSnapIn : TeleportIntoState, ISkillState
    {
        public override Type nextStateType { get { return typeof(ChaosSnapPummel); } }
        public bool rechargeFromChaosMark = true;

        public override void OnExit()
        {
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(false);
            if (target && target.healthComponent.body.HasBuff(ShadowBuffs.chaosMarkDebuff))
            {
                if (NetworkServer.active) target.healthComponent.body.RemoveOldestTimedBuff(ShadowBuffs.chaosMarkDebuff);
                if (isAuthority && activatorSkillSlot.CanApplyAmmoPack() && rechargeFromChaosMark) activatorSkillSlot.ApplyAmmoPack();
            }
            base.OnExit();
        }
    }
}
