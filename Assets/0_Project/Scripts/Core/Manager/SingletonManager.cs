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
    private void Awake()
    {
        Instance = this;

        gameManager = GetComponentInChildren<GameManager>();
        goldManager = GetComponentInChildren<GoldManager>();
        dataManager = GetComponentInChildren<DataManager>();
        sceneLoader = GetComponentInChildren<SceneLoader>();
    }
}
