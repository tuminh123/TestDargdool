using System;
using System.Collections;
using AssetKits.ParticleImage;
using Core;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour //GameElement, IReceive<SignalUpgrade>
{

    [SerializeField] private UpgradeType type;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Popup.Popup popup;
    [SerializeField] private GameObject particleImage;
    
    private UpgradeService service;
    private IUpgradeCommand command;

    private void Awake() {
        particleImage.SetActive(false);
        if(DataManager.Instance == null) return;
        service = new UpgradeService(DataManager.Instance);
        command = new UpgradeCommand(type, service);
    }

    private void Start()
    {
        InitUpgradeUI();
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        if (!service.CanUpgrade(type))
        {
            popup.OpenPopup();
            return;
        }
        if (command.Execute())
        {
            InitUpgradeUI();
            
            UniTaskSafe.Forget
            (
                tc => EffectHandle(),
                this.GetCancellationTokenOnDestroy(),
                $"{type.ToString()} upgrade effect"
            );

        }
    }

    private async UniTask EffectHandle()
    {
        particleImage.SetActive(true);
        await UniTask.Delay(2000);
        particleImage.SetActive(false);
    }
    public void InitUpgradeUI()
    {
        if(service == null) return;
        service.GetStats(type, out float stat, out int cost);
        costText.text = cost.ToString();
        statText.text = stat.ToString();
    }

    // public void Receive(in SignalUpgrade signal)
    // {
    //     costText.text = signal.NewCost.ToString();
    //     statText.text = signal.NewStat.ToString();
    // }
}