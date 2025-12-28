using Cysharp.Threading.Tasks;
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
        if (DataManager.Instance == null) return;
        UpgradeData dataUpgrade = DataManager.Instance.GetUpgradeData(type);
        if (dataUpgrade == null) return; 
        goldText.text = dataUpgrade.UpgradeCost.ToString();
        propertieText.text = GetStatText();

    }

    private void Upgrade()
    {
        if (DataManager.Instance == null) return;
        UpgradeData dataUpgrade = DataManager.Instance.GetUpgradeData(type);
        if(dataUpgrade == null) return;

        var goldMgr = SingletonManager.Instance.goldManager;

        if (!goldMgr.MinusGold(dataUpgrade.UpgradeCost))
        {
            Debug.Log("Not enough gold!");
            return;
        }

        float stat = GetCurrentStat();
        float add = (stat/3)* dataUpgrade.StatIncreasePercent;

        int addInt = Mathf.RoundToInt(add);

        Debug.Log($"{stat} : {add}");

        DataManager.Instance.PlayerData.AddProperties(dataUpgrade.Type, addInt);

        dataUpgrade.CostIncrease();
        //SingletonManager.Instance.dataManager.DataSave();
        UpdateText();
    }

    private string GetStatText()
    {
        if (DataManager.Instance == null) return string.Empty;
        UpgradeData dataUpgrade = DataManager.Instance.GetUpgradeData(type);
        if (dataUpgrade == null) return string.Empty;
        var data = DataManager.Instance.PlayerData;

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
        if (DataManager.Instance == null) return 0;
        UpgradeData dataUpgrade = DataManager.Instance.GetUpgradeData(type);
        if (dataUpgrade == null) return 0;
        var data = DataManager.Instance.PlayerData;

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
