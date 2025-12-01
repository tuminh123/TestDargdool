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
    private void Awake()
    {
        Instance = this;

        gameManager = GetComponentInChildren<GameManager>();
        uiManager = GetComponentInChildren<UIManager>();
        goldManager = GetComponentInChildren<GoldManager>();
        swipeManager = GetComponentInChildren<SwipeManager>();
        dataManager = GetComponentInChildren<DataManager>();
    }
}
