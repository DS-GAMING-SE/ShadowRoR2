using EntityStates;
using ShadowMod.Characters.Survivors.Shadow.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ShadowMainState : GenericCharacterMain
    {
        private HomingTracker homingTracker;
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                homingTracker = GetComponent<HomingTracker>();
                homingTracker.visible = true;
            }
            if (modelLocator)
            {
                modelLocator.normalizeToFloor = true;
            }
        }

        public override void OnExit()
        {
            if (base.isAuthority) homingTracker.visible = false;
            if (modelLocator)
            {
                modelLocator.normalizeToFloor = false;
            }
            base.OnExit();
        }
    }
}
