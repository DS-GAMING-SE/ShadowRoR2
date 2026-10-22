using RoR2;
using ShadowMod.Modules.Achievements;

namespace ShadowMod.Characters.Survivors.Shadow.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class ShadowMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ShadowSurvivor.SHADOW_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ShadowSurvivor.SHADOW_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ShadowSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}