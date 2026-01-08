using Core;
using TMPro;
using UnityEngine;

public class SurvivalWaveUIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldCountText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI waveCountText;

    [SerializeField] private SurvivalSystem survivalSystem;
    private void OnEnable()
    {
        goldCountText.text = $"                        Gold received : {survivalSystem.GoldCount}";
        enemyCountText.text =$"                        Enemy killed : {survivalSystem.EnemyCount}";
        waveCountText.text = $"                        Survived {survivalSystem.WaveCount} waves";
    }

   
}
