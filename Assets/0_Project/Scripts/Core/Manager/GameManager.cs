
using Cysharp.Threading.Tasks;
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
                LoseHandle();
                break;
            case GameState.PAUSE:
                PauseHandle();
                break;
            case GameState.RESTART:
                ResetHandle();
                break;
        }
    }
    private void ResetHandle()
    {
        Time.timeScale = 1;

        SingletonManager.Instance.waveSpawner.ClearAllEnemies();

        ClearData();
        SetState(GameState.PLAY);
    }

   

    private void LoseHandle()
    {
        OnDeadWait().Forget();
        //SingletonManager.Instance.timeSlow.DoSlowmotion();
    }

    private async UniTask OnDeadWait()
    {
        SingletonManager.Instance.timeSlow.DoSlowmotion();
        await UniTask.Delay(1500);
        Time.timeScale = 0;
        ClearData();
        uiManager.SetUI(currentState);

    }

    private void PauseHandle()
    {
        Time.timeScale = 0;
        uiManager.SetUI(currentState);
        SingletonManager.Instance.dataManager.DataSave();
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

        ClearData();
    }
    private void ClearData()
    {
        ObjInGameBase[] objs = FindObjectsByType<ObjInGameBase>(FindObjectsSortMode.None);
        foreach (var obj in objs)
        {
            if (obj == null) continue;
            SingletonManager.Instance.objInGamePoolManager.DeSpawn(obj);
        }
        if (playerInstance != null)
        {
            Destroy(playerInstance);
            playerInstance = null;
        }
    }
    public void SetState(GameState newState)
    {
        if(currentState == newState) return;
        currentState = newState;
        GameEventBus.RaiseGameStateChanged(newState);
    }
    private void OnApplicationQuit()
    {
        SingletonManager.Instance.dataManager.DataSave();
    }
}
