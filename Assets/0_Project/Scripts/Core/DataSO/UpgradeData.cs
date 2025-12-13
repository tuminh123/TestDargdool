using System.Diagnostics;
using UnityEngine;
public enum UpgradeType
{
    NONE = 0,
    HEALTH = 1,
    DAMAGEBASE = 2,
    CRITCHANCE = 3,
    CRITMULTIPLIER = 4
}

[System.Serializable]
public class UpgradeData
{
    [SerializeField] private UpgradeType type;
    [SerializeField] private int upgradeCost;
    [SerializeField] private float costMultiplier = 1.15f;
    [SerializeField] private float statIncreasePercent = 0.10f; // 10%

    [Header("Static Original Value (never changes)")]
    [SerializeField] private int initialUpgradeCost;

    public UpgradeData(UpgradeType type, int upgradeCost, float statIncreasePercent, float costMultiplier)
    {
        this.type = type;
        this.upgradeCost = upgradeCost;
        this.statIncreasePercent = statIncreasePercent;
        this.costMultiplier = costMultiplier;
        this.initialUpgradeCost = upgradeCost;
    }

    public UpgradeType Type => type;
    public int UpgradeCost => upgradeCost;
    public float StatIncreasePercent => statIncreasePercent;
    public float CostMultiplier => costMultiplier;


    public void CostIncrease()
    {
        int cost = Mathf.RoundToInt(upgradeCost * costMultiplier);
        upgradeCost += cost;
    }
    public void SetUpgradeCost(int upgradeCost)
    {
        this.upgradeCost = upgradeCost;
    }
    public void ResetToDefault()
    {
        upgradeCost = initialUpgradeCost;
    }
}
