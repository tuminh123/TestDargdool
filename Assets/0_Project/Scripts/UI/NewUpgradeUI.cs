using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewUpgradeUI : MonoBehaviour
{

    [SerializeField] private UpgradeType type;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Popup.Popup popup;

    private IUpgradeCommand command;

    private void Start()
    {
        UpgradeService service = new UpgradeService(DataManager.Instance);
        command = new UpgradeCommand(type, service);

        button.onClick.AddListener(OnClick);
        UpdateUI();
    }

    private void OnClick()
    {
        if (!command.Execute())
        {
            popup.OpenPopup();
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (DataManager.Instance == null) return;
        UpgradeData dataUpgrade = DataManager.Instance.GetUpgradeData(type);
        if (dataUpgrade == null) return;
        costText.text = dataUpgrade.UpgradeCost.ToString();
        statText.text = GetStatText();
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
}