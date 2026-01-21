public class CritMultiplierUpgradeStrategy : IUpgradeStrategy
{
    public void Apply(PlayerData player, UpgradeData upgrade)
    {
        player.AddProperties(UpgradeType.CRITMULTIPLIER, upgrade.StatIncreasePercent);
    }
}
