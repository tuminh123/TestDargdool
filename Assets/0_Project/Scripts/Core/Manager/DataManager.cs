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

[DefaultExecutionOrder(-1100)]
public class DataManager : MonoBehaviour
{
    public static DataManager Instance {  get; private set; }
    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;
    [Space]
    [Header("Upgrade Data")]
    [SerializeField] private UpgradeData[] upgrades;
    [Space]
    [Header("Progress Data")]
    [SerializeField] private PlayerProgressData progressData;

    private const string PLAYER_KEY = "PlayerData";
    private const string UPGRADE_KEY = "UpgradeData";
    private const string PROGRESS_KEY = "PlayerProgressData";

    private const string SAVE_FILE = "SaveData.es3";
    public bool IsLoaded { get; private set; }
    // ================= GETTER =================
    public PlayerData PlayerData => playerData;
    public PlayerProgressData ProgressData => progressData;
    public UpgradeData[] Upgrades => upgrades;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        DataLoad();
        IsLoaded = true;
    }

    private void Start()
    {
        if (ZenManager.Instance == null || ZenManager.Instance.levelManager == null) return;
        ZenManager.Instance.levelManager.Init(progressData);
    }

    #region Play Data Save/Load
    public void DataSave()
    {
        if (!IsLoaded)
        {
            Debug.LogWarning("Save skipped – data not loaded yet");
            return;
        }

        ES3.Save(PLAYER_KEY, playerData, SAVE_FILE);
        ES3.Save(PROGRESS_KEY, progressData, SAVE_FILE);
        ES3.Save(UPGRADE_KEY, upgrades, SAVE_FILE);

        Debug.Log("Game Saved");
    }

    public void DataLoad()
    {
        if (!ES3.FileExists(SAVE_FILE))
        {
            CreateDefaultData();
            DataSave();
            return;
        }

        try
        {
            playerData = ES3.Load<PlayerData>(PLAYER_KEY, SAVE_FILE);
            upgrades = ES3.Load<UpgradeData[]>(UPGRADE_KEY, SAVE_FILE);
            progressData = ES3.Load<PlayerProgressData>(PROGRESS_KEY, SAVE_FILE);
        }
        catch
        {
            Debug.LogWarning("Save corrupted → reset");
            CreateDefaultData();
            DataSave();
        }
    }
    private void CreateDefaultData()
    {
        // Player stat
        playerData = new PlayerData();
        playerData.ResetData();

        // Level / EXP
        progressData = new PlayerProgressData();

        // Upgrade
        upgrades = new UpgradeData[]
        {
            new UpgradeData(UpgradeType.HEALTH,         15, 1.15f, 0.10f),
            new UpgradeData(UpgradeType.DAMAGEBASE,     20, 1.15f, 0.10f),
            new UpgradeData(UpgradeType.CRITMULTIPLIER, 25, 1.15f, 0.10f),
            new UpgradeData(UpgradeType.CRITCHANCE,     30, 1.15f, 0.10f)
        };
    }
    public void ResetData()
    {
        playerData.ResetData();
        progressData.Reset();

        ResetAllUpgrades();
        DataSave();

        Debug.Log("All data reset");
    }
    public void DeleteAllData()
    {
        if (ES3.FileExists(SAVE_FILE)) ES3.DeleteFile(SAVE_FILE);
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
