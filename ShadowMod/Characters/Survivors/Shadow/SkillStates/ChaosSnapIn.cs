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

        public override void OnExit()
        {
            if (activatorSkillSlot) activatorSkillSlot.SetBlockedCooldownSkillState(false);
            if (target && target.healthComponent.body.HasBuff(ShadowBuffs.chaosMarkDebuff))
            {
                if (NetworkServer.active) target.healthComponent.body.RemoveBuff(ShadowBuffs.chaosMarkDebuff);
                if (isAuthority) activatorSkillSlot.ApplyAmmoPack();
            }
            base.OnExit();
        }
    }
}
