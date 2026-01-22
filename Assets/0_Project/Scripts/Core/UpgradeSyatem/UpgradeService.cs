using System.Collections.Generic;
using Core;

public class UpgradeService
{
    private DataManager dataManager;
    private readonly Dictionary<UpgradeType, IUpgradeStrategy> strategies;
    private readonly Dictionary<UpgradeType, UpgradeData> upgradeDatas;

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

        upgradeDatas = new Dictionary<UpgradeType, UpgradeData>();
        foreach (var upgrade in dataManager.Upgrades)
        {
            if(upgrade == null) continue;
            upgradeDatas[upgrade.Type] = upgrade;
        }
    }

    public bool TryUpgrade(UpgradeType type)
    {
        // 1. Lấy dữ liệu upgrade
        //UpgradeData upgrade = dataManager.GetUpgradeData(type);
        UpgradeData upgrade = GetUpgradeData(type);
        if (upgrade == null) return false;
        
        if(dataManager == null || dataManager.PlayerData == null) return false;
        PlayerData player = dataManager.PlayerData;

        // 2. Trừ vàng
        if(SingletonManager.Instance == null || SingletonManager.Instance.goldManager == null) return false;
        if (!SingletonManager.Instance.goldManager.MinusGold(upgrade.UpgradeCost)) return false;

        if (!strategies.TryGetValue(type, out var strategy)) return false;
        // 4. Áp dụng stat
        strategy.Apply(player, upgrade);

        // 5. Tăng giá upgrade
        upgrade.CostIncrease();
        return true;
    }
    public bool CanUpgrade(UpgradeType type)
    {
        if (dataManager == null || dataManager.PlayerData == null) return false;
        if (upgradeDatas.TryGetValue(type, out var upgrade))
        {
            return dataManager.PlayerData.GoldCount >= upgrade.UpgradeCost;
        }
        return false;
    }
    #region  Getters
    public UpgradeData GetUpgradeData(UpgradeType type)
    {
        if (upgradeDatas.TryGetValue(type, out var upgrade))
        {
            return upgrade;
        }
        return null;
    }
    public void GetStats(UpgradeType type, out float stat, out int cost )
    {
        #region Old code
        // stat = 0f;  
        // cost = 0;    

        // if (dataManager != null && dataManager.PlayerData != null)
        // {
        //     stat = dataManager.PlayerData.GetProperties(type);
        // }

        // UpgradeData upgrade = dataManager?.GetUpgradeData(type);
        // if(upgrade != null)
        // {
        //     cost = upgrade.UpgradeCost;
        // }
        #endregion

        stat = dataManager?.PlayerData?.GetProperties(type) ?? 0f;
        cost = GetUpgradeData(type)?.UpgradeCost ?? 0;
    }
   
    #endregion
}