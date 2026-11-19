using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates.DoomPowerUpgrades
{
    public class ShockStateDoomSpear : ShockStateChaosSpear
    {
        protected override Material overlayMaterial => ShadowAssets.doomSpearShockingOverlay;
    }
}
