using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BoxHealth : MonoBehaviour, IDamageable
{
    [InjectOptional] private ObjInGamePoolManager objInGamePoolManager;
    [InjectOptional] private ItemPoolManager itemPoolManager;
    [SerializeField] private float maxHP = 1;
    [SerializeField] private float currentHP;
    private Box box;

    private void Awake()
    {
        box= GetComponentInParent<Box>();
    }
    private void OnEnable()
    {
        InitHealth();
    }
    private void Start()
    {
        InitHealth();
    }

    public bool IsDead => currentHP <= 0;

    public void TakeDamaged(float damage)
    {
        if (IsDead ) return;
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0)
        {
            BoxDeSpawning().Forget();
        }
    }
    public async UniTask BoxDeSpawning()
    {
        try
        {
            box.ani.Play("explosion");

            await UniTask.Delay(500);

            ZenManager.Instance.itemPoolManager.SpawnRandomItem(transform.position);
            await UniTask.Delay(200);
            ZenManager.Instance.objInGamePoolManager.DeSpawn(box);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"BoxDeSpawning Error: {ex.Message}");
        }
    }
    public void InitHealth()
    {
        currentHP = maxHP;
    }
}
