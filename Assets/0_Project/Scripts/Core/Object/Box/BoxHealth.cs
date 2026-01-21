using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class BoxHealth : MonoBehaviour, IDamageable
{
    [InjectOptional] protected ObjInGamePoolManager objInGamePoolManager;
    [InjectOptional] private ItemPoolManager itemPoolManager;
    [SerializeField] private float maxHP = 1;
    [SerializeField] private float currentHP;
    protected Box box;
    private CancellationTokenSource tc;

    private void Awake()
    {
        box= GetComponentInParent<Box>();
    }
    private void Start()
    {
        InitHealth();
    }
    private void OnEnable()
    {
        InitHealth();

    }
    private void OnDisable()
    {
        if(tc != null)
        {
            if(!tc.IsCancellationRequested)tc.Cancel();
            tc.Dispose();
            tc = null;
        }
    }

    public bool IsDead => currentHP <= 0;

    public void TakeDamaged(float damage)
    {
        if (IsDead ) return;
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0)
        {
            tc = new CancellationTokenSource();
            var tcl = CancellationTokenSource.CreateLinkedTokenSource(tc.Token, this.destroyCancellationToken).Token;

            UniTaskSafe.Forget
            (
                tc => BoxDeSpawning(tc),
                tcl,
                $"{gameObject.name} despawn"
            );
        }
    }
    public async UniTask BoxDeSpawning(CancellationToken token)
    {
        try
        {
            if (box == null)
            {
                Debug.LogError("BoxDeSpawning: box is NULL");
                return;
            }

            if (box.ani == null)
            {
                Debug.LogError("BoxDeSpawning: box.ani is NULL");
                return;
            }
            box.ani.Play("explosion");

            await UniTask.Delay(500,cancellationToken: token);

            await Spawn();
        }
        catch (OperationCanceledException e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
    }
    public abstract UniTask Spawn();
    public void InitHealth()
    {
        currentHP = maxHP;
    }
}
