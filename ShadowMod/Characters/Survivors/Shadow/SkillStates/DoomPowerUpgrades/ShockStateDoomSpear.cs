using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates.DoomPowerUpgrades
{
    public class ShockStateDoomSpear : ShockStateChaosSpear
    {
        protected override GameObject stunVfxPrefab => ShadowAssets.doomSpearShockedEffect;
        protected override Material overlayMaterial => ShadowAssets.doomSpearShockingOverlay;
    }
}
