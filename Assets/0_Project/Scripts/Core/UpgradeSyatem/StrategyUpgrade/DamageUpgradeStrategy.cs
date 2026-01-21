public class DamageUpgradeStrategy : IUpgradeStrategy
{
    public void Apply(PlayerData player, UpgradeData upgrade)
    {
        float addDamage = (player.Damage / 2f) * upgrade.StatIncreasePercent;
        player.AddProperties(UpgradeType.DAMAGEBASE, addDamage);
    }
}
