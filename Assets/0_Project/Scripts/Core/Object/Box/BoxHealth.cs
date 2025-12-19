using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public abstract class BoxHealth : MonoBehaviour, IDamageable
{
    [InjectOptional] private ObjInGamePoolManager objInGamePoolManager;
    [InjectOptional] private ItemPoolManager itemPoolManager;
    [SerializeField] private float maxHP = 1;
    [SerializeField] private float currentHP;
    protected Box box;

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

            await Spawn();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"BoxDeSpawning Error: {ex.Message}");
        }
    }
    // Change the abstract method declaration to remove the 'async' modifier.
    // The 'async' modifier is not allowed on abstract methods, only on methods with a body.
    public abstract UniTask Spawn();
    public void InitHealth()
    {
        currentHP = maxHP;
    }
}
