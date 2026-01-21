using System.Collections.Generic;

public class UpgradeService
{
    private DataManager dataManager;
    private readonly Dictionary<UpgradeType, IUpgradeStrategy> strategies;

    public UpgradeService(DataManager dataManager)
    {
        this.dataManager = dataManager;

        strategies = new Dictionary<UpgradeType, IUpgradeStrategy>
        {
            { UpgradeType.HEALTH, new HealthUpgradeStrategy() },
            { UpgradeType.DAMAGEBASE, new DamageUpgradeStrategy() },
            { UpgradeType.CRITCHANCE, new CritChanceUpgradeStrategy() },
            { UpgradeType.CRITMULTIPLIER, new CritMultiplierUpgradeStrategy() }
        };
    }

    public bool TryUpgrade(UpgradeType type)
    {
        // 1. Lấy dữ liệu upgrade
        UpgradeData upgrade = dataManager.GetUpgradeData(type);
        if (upgrade == null) return false;

        PlayerData player = dataManager.PlayerData;

        // 2. Trừ vàng
        if (!player.MinusGold(upgrade.UpgradeCost)) return false;

        if (!strategies.TryGetValue(type, out var strategy)) return false;
        // 4. Áp dụng stat
        strategy.Apply(player, upgrade);

        // 5. Tăng giá upgrade
        upgrade.CostIncrease();

        return true;
    }
}