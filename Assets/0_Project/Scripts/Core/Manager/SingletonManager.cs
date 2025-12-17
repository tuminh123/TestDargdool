using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance { get;private set; }

    //Component
    public GameManager gameManager {  get; private set; }
    public SceneLoader sceneLoader { get; private set; }
    public GoldManager goldManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public SoundManager soundManager { get; private set; }
    public MusicManager musicManager { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        gameManager = GetComponentInChildren<GameManager>();
        goldManager = GetComponentInChildren<GoldManager>();
        dataManager = GetComponentInChildren<DataManager>();
        sceneLoader = GetComponentInChildren<SceneLoader>();
        soundManager = GetComponentInChildren<SoundManager>();
        musicManager = GetComponentInChildren<MusicManager>();
    }
}
