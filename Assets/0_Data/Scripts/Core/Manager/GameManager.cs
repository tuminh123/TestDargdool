
using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Playables;
using Zenject;

public class GameManager : MonoBehaviour
{
    private GameState currentState;

    [SerializeField] private GameObject player;


    private GameObject playerInstance;
    private UIManager uiManager;

    //get
    public GameState CurrentState => currentState;
    public GameObject PlayerInstance => playerInstance;

    private void Awake()
    {
        SingletonManager.Instance.dataManager.DataLoad();

        int goldCount = SingletonManager.Instance.dataManager.Data.goldCount;

        SingletonManager.Instance.goldManager.SetGoldCount(goldCount);
    }
    private void Start()
    {
        uiManager = SingletonManager.Instance.uiManager;
        currentState = GameState.MENU;
        GameEventBus.RaiseGameStateChanged(currentState);

    }

    private void OnEnable()
    {
        GameEventBus.OnGameStateChanged += GameEventBus_OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameEventBus.OnGameStateChanged -= GameEventBus_OnGameStateChanged;
    }

    private void GameEventBus_OnGameStateChanged(GameState obj)
    {
        Debug.Log(currentState);
        switch (obj)
        {
            default:
            case GameState.MENU:
                MenuHandle();
                break;
            case GameState.PLAY:
                PlayHandle();
                break;
            case GameState.LOSE:
                Time.timeScale = 0;
                uiManager.SetUI(currentState);
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                uiManager.SetUI(currentState);
                break;

        }
    }

    private void PlayHandle()
    {
        Time.timeScale = 1;
        uiManager.SetUI(currentState);
        
        if (playerInstance != null) return; 
        playerInstance = Instantiate(player,transform.position,Quaternion.identity);

        var ctrl = playerInstance.GetComponent<CharacterCtrl>();
        if (ctrl == null) return;

        GameEventBus.RaisePlayerSpawned(ctrl);
    }

    private void MenuHandle()
    {
        uiManager.SetUI(currentState);
        if (playerInstance == null) return;
        Destroy(playerInstance);
    }
    
    public void SetState(GameState newState)
    {
        if(currentState == newState) return;
        currentState = newState;
        GameEventBus.RaiseGameStateChanged(newState);
    }
}
