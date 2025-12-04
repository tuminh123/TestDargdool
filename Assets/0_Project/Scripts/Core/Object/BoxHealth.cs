using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoxHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHP = 1;
    [SerializeField] private float currentHP;
    private Box box;

    private void Awake()
    {
        box= GetComponentInParent<Box>();
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
        box.ani.Play("explosion");

        await UniTask.Delay(500);
        Gold gold = SingletonManager.Instance.objInGamePoolManager.Spawn(StringConst.GOLD, transform.position, Quaternion.identity) as Gold;
        gold.SetVelocity();

        await UniTask.Delay(200);
        SingletonManager.Instance.objInGamePoolManager.DeSpawn(box);
    }
    public void InitHealth()
    {
        currentHP = maxHP;
    }
}
