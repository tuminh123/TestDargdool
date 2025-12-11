using UnityEngine;
public enum UpgradeType
{
    NONE = 0,
    HEALTH = 1,
    DAMAGEBASE = 2,
    CRITCHANCE = 3,
    CRITMULTIPLIER = 4
}
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Data SO/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private UpgradeType type;
    [SerializeField] private int upgradeCost;
    [SerializeField] private float costMultiplier = 1.15f;
    [SerializeField] private float statIncreasePercent = 0.10f; // 10%

    [Header("Static Original Value (never changes)")]
    [SerializeField] private int initialUpgradeCost;


    public UpgradeType Type => type;
    public int UpgradeCost => upgradeCost;
    public float StatIncreasePercent => statIncreasePercent;
    public float CostMultiplier => costMultiplier;

    private void OnEnable()
    {
        // Lưu lại giá trị ban đầu chỉ lần đầu tiên
        if (initialUpgradeCost == 0) initialUpgradeCost = upgradeCost;
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
