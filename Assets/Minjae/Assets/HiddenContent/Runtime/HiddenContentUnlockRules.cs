public static class HiddenContentUnlockRules
{
    public static bool CanUnlock(
        bool twoGamesCompleted,
        bool twoRoomUpgradesPurchased,
        bool sleepCompleted,
        bool developerReached)
    {
        return twoGamesCompleted
            && twoRoomUpgradesPurchased
            && sleepCompleted
            && developerReached;
    }
}