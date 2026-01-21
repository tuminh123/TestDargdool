public class HealthUpgradeStrategy : IUpgradeStrategy
{
    public void Apply(PlayerData player, UpgradeData upgrade)
    {
        float addHp = player.Health * upgrade.StatIncreasePercent;
        player.AddProperties(UpgradeType.HEALTH, addHp);
    }
}
