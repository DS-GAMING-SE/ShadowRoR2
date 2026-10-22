namespace ShadowMod.Modules
{
    internal static class Tokens
    {
        public const string agilePrefix = "<style=cIsUtility>Agile.</style> ";

        public static string DamageText(string text)
        {
            return $"<style=cIsDamage>{text}</style>";
        }
        public static string DamageValueText(float value)
        {
            return $"<style=cIsDamage>{value * 100}% damage</style>";
        }
        public static string DamageValueText(float value, float value2)
        {
            return $"<style=cIsDamage>{value * 100}%-{value2 * 100}% damage</style>";
        }
        public static string DamageValueRepeatText(float value, int repeat)
        {
            return $"<style=cIsDamage>{repeat}x{value * 100}% damage</style>";
        }
        public static string DamageMultiplierText(float value)
        {
            return $"<style=cIsDamage>{value}%</style>";
        }
        public static string UtilityText(string text)
        {
            return $"<style=cIsUtility>{text}</style>";
        }
        public static string RedText(string text) => DeathText(text);
        public static string HealthText(string text)
        {
            return $"<style=cIsHealth>{text}</style>";
        }
        public static string DeathText(string text)
        {
            return $"<style=cDeath>{text}</style>";
        }
        public static string HealingText(string text)
        {
            return $"<style=cIsHealing>{text}</style>";
        }
        public static string KeywordText(string keyword, string sub)
        {
            return $"<style=cKeywordName>{keyword}</style><style=cSub>{sub}</style>";
        }
        public static string StackingText(string stack)
        {
            return $"<style=cStack>{stack}</style>";
        }
        public static string ScepterDescription(string desc)
        {
            return $"\n<color=#d299ff>SCEPTER: {desc}</color>";
        }
        public static string LogbookMono(string stack)
        {
            return $"<style=cMono>{stack}</style>";
        }

        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementNameToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementDescriptionToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}