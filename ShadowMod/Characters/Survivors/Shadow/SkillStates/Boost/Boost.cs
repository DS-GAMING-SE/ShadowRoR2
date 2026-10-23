using EntityStates;
using HedgehogUtils.Boost;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using ShadowMod.Characters.Survivors.Shadow.Components;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class Boost : HedgehogUtils.Boost.EntityStates.Boost
    {
        protected override BuffDef buff => ShadowBuffs.boostBuff;
        private HomingTracker homingTracker;
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.modelLocator)
            {
                modelLocator.normalizeToFloor = true;
            }
            if (base.isAuthority)
            {
                homingTracker = GetComponent<HomingTracker>();
                homingTracker.visible = true;
            }
        }

        public override void OnExit()
        {
            if (base.modelLocator)
            {
                modelLocator.normalizeToFloor = false;
            }
            if (base.isAuthority)
            {
                homingTracker.visible = false;
            }
            base.OnExit();
        }
        public override GameObject GetFlashPrefab()
        {
            return ShadowAssets.boostFlashEffect;
        }
        public override GameObject GetAuraPrefab()
        {
            return ShadowAssets.boostAuraEffect;
        }
    }
}