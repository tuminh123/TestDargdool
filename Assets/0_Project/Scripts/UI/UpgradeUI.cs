using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI propertieText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button increaseButton;
    [SerializeField] private UpgradeType type;


    private void Start()
    {
        UpdateText();
        increaseButton.onClick.AddListener(Upgrade);
    }

    private void UpdateText()
    {
        UpgradeData dataUpgrade = SingletonManager.Instance.dataManager.GetUpgradeData(type);
        goldText.text = dataUpgrade.UpgradeCost.ToString();
        propertieText.text = GetStatText();
    }

    private void Upgrade()
    {
        UpgradeData dataUpgrade = SingletonManager.Instance.dataManager.GetUpgradeData(type);
        var goldMgr = SingletonManager.Instance.goldManager;

        if (!goldMgr.MinusGold(dataUpgrade.UpgradeCost))
        {
            Debug.Log("Not enough gold!");
            return;
        }

        float stat = GetCurrentStat();
        float add = stat * dataUpgrade.StatIncreasePercent;

        SingletonManager.Instance.dataManager.Data.AddProperties(dataUpgrade.Type, add);

        dataUpgrade.CostIncrease();
        //SingletonManager.Instance.dataManager.DataSave();
        UpdateText();
    }

    private string GetStatText()
    {
        UpgradeData dataUpgrade = SingletonManager.Instance.dataManager.GetUpgradeData(type);
        var data = SingletonManager.Instance.dataManager.Data;

        return dataUpgrade.Type switch
        {
            UpgradeType.HEALTH => $"Health : {data.Health}",
            UpgradeType.DAMAGEBASE => $"Damage : {data.Damage}",
            UpgradeType.CRITCHANCE => $"Crit : {data.CritChance}",
            UpgradeType.CRITMULTIPLIER => $"Crit Multiplier : {data.CritMultiplier}",
            _ => "Unknown"
        };
    }

    private float GetCurrentStat()
    {
        UpgradeData dataUpgrade = SingletonManager.Instance.dataManager.GetUpgradeData(type);
        var data = SingletonManager.Instance.dataManager.Data;

        return dataUpgrade.Type switch
        {
            UpgradeType.HEALTH => data.Health,
            UpgradeType.DAMAGEBASE => data.Damage,
            UpgradeType.CRITCHANCE => data.CritChance,
            UpgradeType.CRITMULTIPLIER => data.CritMultiplier,
            _ => 0f
        };
    }

}
