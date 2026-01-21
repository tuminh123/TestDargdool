public class CritChanceUpgradeStrategy : IUpgradeStrategy
{
    public void Apply(PlayerData player, UpgradeData upgrade)
    {
        float addCrit = upgrade.StatIncreasePercent * 0.01f;
        player.AddProperties(UpgradeType.CRITCHANCE, addCrit);
    }
}
