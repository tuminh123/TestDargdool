using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string name = "player";
    [SerializeField] private int goldCount = 100;
    [SerializeField] private float maxHp = 500;
    [SerializeField] private float damageBase = 30;

    // --- Chỉ số Crit ---
    [SerializeField] private float critChance = 0.05f;      // 5% ban đầu
    [SerializeField] private float critMultiplier = 1.5f;   // 150% damage khi Crit

    //get
    public int GoldCount => goldCount;
    public float Health => maxHp;
    public float Damage => damageBase;
    public float CritChance => critChance;
    public float CritMultiplier => critMultiplier;

    public void AddProperties(UpgradeType type, float amount)
    {
        switch (type)
        {
            case UpgradeType.HEALTH:
                maxHp += amount;
                break;
            case UpgradeType.DAMAGEBASE:
                damageBase += amount;
                break;
            case UpgradeType.CRITCHANCE:
                critChance += amount;
                break;
            case UpgradeType.CRITMULTIPLIER:
                critMultiplier += amount;
                break;
        }
    }
    public bool AddGold(int amount)
    {
        goldCount += amount;
        return true;
    }
    public bool MinusGold(int amount)
    {
        if (amount > goldCount || goldCount == 0) return false;
        goldCount -= amount;
        return true;
    }

    public void ResetData()
    {
        name = "player";
        goldCount = 100;
        maxHp = 200;
        damageBase = 30;
        critChance = 0.05f;
        critMultiplier = 1.5f;
    }

    public float CalculateDamage()
    {
        bool isCrit = UnityEngine.Random.value < critChance;
        float damage = damageBase;

        if (isCrit) damage *= critMultiplier;

        return damage;
    }
}

public class DataManager : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private PlayerData data;
    [Space]
    [Header("Upgrade Data")]
    [SerializeField] private UpgradeData[] upgrades;

    private string keyName = "PlayerData";
    private string keyUpgrdeName = "UpgradeData";
    private string mainFile = "SaveData.txt";
    private string mainUpgradeFile = "SaveUpgradeData.txt";

    public PlayerData Data => data;

    #region Play Data Save/Load
    public void DataSave()
    {
        ES3.Save(keyName, data, mainFile);
        ES3.Save(keyUpgrdeName, upgrades, mainUpgradeFile);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(mainFile))
        {
            try
            {
                // Load dữ liệu từ file vào data
                data = ES3.Load<PlayerData>(keyName, mainFile);
                upgrades = ES3.Load<UpgradeData[]>(keyUpgrdeName, mainUpgradeFile);
                Debug.Log("Data loaded successfully!");
                return;
            }
            catch
            {
                Debug.LogWarning("Main save file corrupted, creating new save...");
            }
        }
        else
        {
            Debug.Log("Save file not found. Creating new save...");
        }

        // Nếu file không tồn tại hoặc bị hỏng, tạo mới dữ liệu mặc định
        data = new PlayerData(); // khởi tạo dữ liệu mặc định
        upgrades = new UpgradeData[]
        {
            new UpgradeData(UpgradeType.HEALTH,15,1.15f,0.10f),
            new UpgradeData(UpgradeType.DAMAGEBASE,20,1.15f,0.10f),
            new UpgradeData(UpgradeType.CRITMULTIPLIER, 25, 1.15f, 0.10f),
            new UpgradeData(UpgradeType.CRITCHANCE, 30, 1.15f, 0.10f)
        };
        DataSave(); // lưu lại file
    }
    public void ResetData()
    {
        data.ResetData();
        ResetAllUpgrades();
        DataSave();
    }
    public void DeleteAllData()
    {
        if (ES3.FileExists(mainFile)) ES3.DeleteFile(mainFile);
    }
    #endregion

    #region Data Upgrades
    public void ResetAllUpgrades()
    {
        foreach (var up in upgrades)
        {
            if (up == null) continue;
            up.ResetToDefault();
        }
    }
    public UpgradeData GetUpgradeData(UpgradeType type)
    {
        foreach (var up in upgrades)
        {
            if(up == null) continue;
            if (up.Type == type) return up;
        }
        return null;
    }
    #endregion
}
