using UnityEngine;
using Zenject;

[DefaultExecutionOrder(-1010)]
public class ZenManager : MonoBehaviour
{
    /*public override void InstallBindings()
    {
        base.InstallBindings();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ObjInGamePoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<VfxPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ItemPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ProjectilePoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TimeSlow>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CameraShaker>().FromComponentInHierarchy().AsSingle();
    }*/
    public static ZenManager Instance { get; private set; }

    public UIManager uIManager {  get; private set; }
    public ObjInGamePoolManager objInGamePoolManager { get; private set; }
    public ProjectilePoolManager projectilePoolManager { get; private set; }
    public VfxPoolManager vfxPoolManager { get; private set; }
    public ItemPoolManager itemPoolManager { get; private set; }
    public TimeSlow timeSlow { get; private set; }
    public CameraShaker cameraShaker { get; private set; }
    public WaveSpawner waveSpawner { get; private set; }
    public LevelManager levelManager { get; private set; }
    private void Awake()
    {
        Instance = this;

        uIManager = GetComponentInChildren<UIManager>();
        objInGamePoolManager = GetComponentInChildren<ObjInGamePoolManager>();
        projectilePoolManager = GetComponentInChildren<ProjectilePoolManager>();
        vfxPoolManager = GetComponentInChildren<VfxPoolManager>();
        itemPoolManager = GetComponentInChildren<ItemPoolManager>();
        timeSlow = GetComponentInChildren<TimeSlow>();
        cameraShaker = GetComponentInChildren<CameraShaker>();
        waveSpawner = GetComponentInChildren<WaveSpawner>();
        levelManager = GetComponentInChildren<LevelManager>();
    }
    private void Start()
    {
        timeSlow.gameObject.SetActive(false);
    }
}
