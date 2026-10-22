using ShadowMod.Characters.Survivors.Shadow.Achievements;
using RoR2;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ShadowMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ShadowMasteryAchievement.identifier), 
                ShadowAssets.masterySkinIcon);
        }
    }
}
