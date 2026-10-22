using EntityStates;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.Networking;
using HedgehogUtils.Boost;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class Boost : HedgehogUtils.Boost.EntityStates.Boost
    {
        protected override BuffDef buff => ShadowBuffs.boostBuff;
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.modelLocator)
            {
                modelLocator.normalizeToFloor = true;
            }
        }

        public override void OnExit()
        {
            if (base.modelLocator)
            {
                modelLocator.normalizeToFloor = false;
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