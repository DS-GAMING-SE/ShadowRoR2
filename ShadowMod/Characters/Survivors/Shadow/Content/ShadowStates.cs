using ShadowMod.Characters.Survivors.Shadow.SkillStates;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ShadowMainState));

            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(Boost));
            Modules.Content.AddEntityState(typeof(BoostIdle));
            Modules.Content.AddEntityState(typeof(Brake));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}
