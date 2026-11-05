using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ChaosBlastIn : TeleportIntoState, ISkillState
    {
        public override Type nextStateType { get { return typeof(ChaosBlastCharge); } }
        protected override float teleportInBaseDuration { get { return 0.45f; } }
        protected override float baseDuration { get { return 0.5f; } }
    }
}
