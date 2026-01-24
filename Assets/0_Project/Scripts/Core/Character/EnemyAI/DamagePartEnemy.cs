using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject.SpaceFighter;

public class DamagePartEnemy : EnemyAI
{
    #region State
    public DamagePartEnemyCombatState damagePartEnemyCombatState { get; private set; }
    public DamagePartEnemyAttackState damagePartEnemyAttackState { get; private set; }
    public DamagePartEnemyChaseState damagePartEnemyChaseState { get;private set; }
    public DamagePartEnemyStundState damagePartEnemyStund { get; private set; }
    public DamagePartEnemyDeadState damagePartEnemyDeadState { get;private set;}
    public DamagePartEnemyIdleState damagePartEnemyIdleState { get; private set; }
    public DamagePartEnemyJumpState damagePartEnemyJumpState { get; private set; }
    public DamagePartEnemySummon damagePartEnemySummon { get; private set; }
    public DamagePartShooting partShooting { get; private set; }
    public DamagePartEnemyTeleportState teleportState { get; private set; }
    #endregion

    public event System.Action EndShooting;
    public event System.Action EndTeleport;
    public event System.Action EndSummon;

    [SerializeField] private Transform head;
    [SerializeField] private Transform shootPointLeft;
    [SerializeField] private Transform shootPointRight;

    [SerializeField] private float durationShoot = 10f;
    [SerializeField] private float durationTeleport = 8f;
    [SerializeField] private float distance = 10f;

    [SerializeField] private GameObject clone;

    [SerializeField] private int maxSummonCount = 5;
    [SerializeField] private float summonCooldown = 12f;

    private float lastSummonTime = -999f;
    private int currentAliveMinions = 0;


    [SerializeField] private RegenerationDamageArea damageArea;

    public float timeToShoot {  get; private set; }
    public float timeToTeleport{  get; private set; }
    public CharacterShoot shoot { get;private set; }    
    public Transform currentShootPoint { get; private set; }

    private CancellationTokenSource cts_Shoot;
    private CancellationTokenSource cts_Teleport;
    private CancellationTokenSource cts_Summon;
    //get
    public Transform Head => head;
    public RegenerationDamageArea DamageArea => damageArea;
    protected override void Awake()
    {
        base.Awake();
        damagePartEnemyAttackState = new DamagePartEnemyAttackState(this, stateMachine);
        damagePartEnemyChaseState = new DamagePartEnemyChaseState(this, stateMachine);
        damagePartEnemyCombatState = new DamagePartEnemyCombatState(this, stateMachine,attackDuration);
        damagePartEnemyStund = new DamagePartEnemyStundState(this, stateMachine, stunnedDuration);
        damagePartEnemyDeadState = new DamagePartEnemyDeadState(this, stateMachine, dieDuration);
        damagePartEnemyIdleState = new DamagePartEnemyIdleState(this, stateMachine);
        damagePartEnemyJumpState = new DamagePartEnemyJumpState(this, stateMachine);
        damagePartEnemySummon = new DamagePartEnemySummon(this, stateMachine);
        partShooting = new DamagePartShooting(this, stateMachine);
        teleportState = new DamagePartEnemyTeleportState(this, stateMachine);

        shoot = new SnowBossShoot(currentShootPoint);

    }
    protected override void Start()
    {
        base.Start();
        DontAddExp();
        stateMachine.InitState(damagePartEnemyIdleState);
        timeToShoot = durationShoot;
        timeToTeleport = durationTeleport;
    }

    protected override void Update()
    {
        timeToShoot -= Time.deltaTime;
        timeToTeleport -= Time.deltaTime;
        base.Update();

    }

    #region Summon

    public bool CanSummon()
    {
        if (Time.time - lastSummonTime < summonCooldown)
            return false;

        if (currentAliveMinions >= maxSummonCount)
            return false;

        return true;
    }

    public void SummonEnemy()
    {
        if (!CanSummon())
        {
            EndSummon?.Invoke();
            return;
        }

        cts_Summon = new CancellationTokenSource();
        var ltcs = CancellationTokenSource.CreateLinkedTokenSource(cts_Summon.Token, this.GetCancellationTokenOnDestroy()).Token;

        UniTaskSafe.Forget
        (
            ct => SummonHandle(ct),
            ltcs,
            $"{gameObject.name} is summon"
        );
    }
    private async UniTask SummonHandle(CancellationToken token)
    {
        lastSummonTime = Time.time;

        Vector3 spawnPos = SpawnPoint.Instance.GetSpawnPos();
        
        int summonAmount = maxSummonCount - currentAliveMinions;
        summonAmount = Mathf.Min(summonAmount, maxSummonCount);

        for (int i = 0; i < summonAmount; i++)
        {

            var minion = Instantiate(clone, spawnPos, Quaternion.identity).GetComponent<EnemyBasic>();
            minion.DontAddExp();
            if (minion != null)
            {
                currentAliveMinions++;

                minion.healthBase.OnDead += ()=> currentAliveMinions--;

            }

            await UniTask.WaitForSeconds(3f, cancellationToken : token);
        }

        EndSummon?.Invoke();
    }

    public void CancelSummon()
    {
        if (cts_Summon != null)
        {
            if (!cts_Summon.IsCancellationRequested) cts_Summon.Cancel();
            cts_Summon.Dispose();
            cts_Summon = null;
        }
    }

    #endregion

    #region Teleport
    public bool CanTeleport => timeToTeleport <= 0;

    public void Teleport()
    {
        cts_Teleport = new CancellationTokenSource();
        var ltcs = CancellationTokenSource.CreateLinkedTokenSource(cts_Teleport.Token, this.GetCancellationTokenOnDestroy()).Token;

        UniTaskSafe.Forget
        (
            ct => TeleportHandle(ct),
            ltcs,
            $"{gameObject.name} is shooting"
        );
    }

    private async UniTask TeleportHandle(CancellationToken ct)
    {
        if (characterCtrl == null) return;

        Vector3 enemyPos = transform.position;
        Vector3 playerPos = characterCtrl.transform.position;

        float newX;

        // Enemy ở bên trái player
        if (enemyPos.x < playerPos.x)
            newX = playerPos.x + distance;
        else
            newX = playerPos.x - distance;
        await UniTask.WaitForSeconds(0.5f, cancellationToken: ct);

        transform.position = new Vector3(newX, enemyPos.y, enemyPos.z);

        await UniTask.WaitForSeconds(0.5f, cancellationToken: ct);

        timeToTeleport = durationTeleport;
        FlipSystem(newX,head);
        EndTeleport?.Invoke();
    }

    public void CancelTeleport()
    {
        if (cts_Teleport != null)
        {
            if (!cts_Teleport.IsCancellationRequested) cts_Teleport.Cancel();
            cts_Teleport.Dispose();
            cts_Teleport = null;
        }
    }

    #endregion

    #region Shoot
    public bool CanShoot => timeToShoot <= 0;
    public void ShootHandle()
    {
        cts_Shoot = new CancellationTokenSource();
        var ltcs = CancellationTokenSource.CreateLinkedTokenSource(cts_Shoot.Token,this.GetCancellationTokenOnDestroy()).Token;

        UniTaskSafe.Forget
        (
            ct=>Shooting(ct),
            ltcs,
            $"{gameObject.name} is shooting"
        );
    }
    private async UniTask Shooting(CancellationToken ct)
    {
        string nameAction = attackDir.x > 0 ? StringConst.ICE_SHOOT_RIGHT : StringConst.ICE_SHOOT_LEFT;
        currentShootPoint = attackDir.x > 0 ? shootPointRight : shootPointLeft;
        ragdollController?.postContext?.GetAction(nameAction);

        shoot.SetShootPoint(currentShootPoint);

        await UniTask.WaitForSeconds(1,cancellationToken:  ct);

        shoot.Shoot(attackDir);

        timeToShoot = durationShoot;

        EndShooting?.Invoke();
    }

    public void CancelShoot()
    {
        if(cts_Shoot != null)
        {
            if(!cts_Shoot.IsCancellationRequested) cts_Shoot.Cancel();
            cts_Shoot.Dispose();
            cts_Shoot = null;
        }
    }

    #endregion

}
