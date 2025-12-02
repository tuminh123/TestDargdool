using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance { get;private set; }

    //Component
    public GameManager gameManager {  get; private set; }
    public UIManager uiManager { get; private set; }
    public GoldManager goldManager { get; private set; }
    public SwipeManager swipeManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public ObjInGamePoolManager objInGamePoolManager { get; private set; }
    public WaveSpawner waveSpawner { get; private set; }
    private void Awake()
    {
        Instance = this;

        waveSpawner = GetComponentInChildren<WaveSpawner>();
        gameManager = GetComponentInChildren<GameManager>();
        uiManager = GetComponentInChildren<UIManager>();
        goldManager = GetComponentInChildren<GoldManager>();
        swipeManager = GetComponentInChildren<SwipeManager>();
        dataManager = GetComponentInChildren<DataManager>();
        objInGamePoolManager = GetComponentInChildren<ObjInGamePoolManager>();
    }
}
