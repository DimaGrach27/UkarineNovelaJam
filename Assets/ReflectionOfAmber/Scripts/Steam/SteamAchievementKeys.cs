namespace ReflectionOfAmber.Scripts.Steam
{
    public struct SteamAchievementKeys
    {
        public static string GetId(AchievementKeys achievementKey)
        {
            return achievementKey switch
            {
                AchievementKeys.Test => "test_achievement_1",
                _ => string.Empty
            };
        }
    }

    public enum AchievementKeys
    {
        Test = -1,
    }
}