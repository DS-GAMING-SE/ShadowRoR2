using EntityStates;
using RoR2;
using RoR2.Orbs;
using ShadowMod.Characters.Survivors.Shadow.Components;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates.DoomPowerUpgrades
{
    public class DoomSpear : ChaosSpear
    {
        public override int maxAttacks { get { return ShadowStaticValues.doomSpearCount; } }

        protected override void FireSpearVFX()
        {
            Util.PlaySound("Play_dsshadowthehedgehog_chaos_spear_fire", gameObject);
        }
        protected override Orb CreateSpear(HurtBox target)
        {
            return ChaosSpearOrb.CreateChaosSpearOrb(characterBody, transform.position, target, ShadowDamageTypes.doomSpearShocking, ShadowAssets.doomSpearOrbEffect);
        }
    }
}
