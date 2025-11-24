
using System;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour,IObjectPool
{
        #region Child component
        public Move move { get; private set; }
        public Attack attack { get; private set; }
        public Idle idle { get; private set; }
        #endregion

        #region State
        private StateMachine stateMachine;
        public EnemyBasicChaseState enemyChaseState { get; private set; }
        public EnemyBasicAttackState enemyAttackState { get; private set; }
        public EnemyBasicIdleState enemyIdleState { get; private set; }

    #endregion

        [SerializeField] private Transform body;
        public PlayerDetect playerDetect { get; private set; }
    public HealthBase healthBase { get; private set; }

    private Coroutine damageCoroutine;

    private void Awake()
        {
                move = GetComponentInChildren<Move>();
                attack = GetComponentInChildren<Attack>();
                idle = GetComponentInChildren<Idle>();
                playerDetect = GetComponentInChildren<PlayerDetect>();
        healthBase = GetComponent<HealthBase>();

                //state init
                stateMachine = new StateMachine();
                enemyChaseState = new EnemyBasicChaseState(stateMachine, this,body);
                enemyAttackState = new EnemyBasicAttackState(stateMachine, this,body);
                enemyIdleState = new EnemyBasicIdleState(stateMachine, this,body);
        }

        private void Start()
        {
                stateMachine.InitState(enemyIdleState);
        }

        private void FixedUpdate()
        {
                stateMachine.UpdateState();
        }

    #region Health event
    private void OnEnable()
    {
        if (healthBase == null) return;
        healthBase.OnDead += OnEnmyDead;
        healthBase.OnTakeDamage += OnTakeDamage;
    }

    private void OnDisable()
    {
        if (healthBase == null) return;
        healthBase.OnDead -= OnEnmyDead;
        healthBase.OnTakeDamage += OnTakeDamage;

        StopDamageCoroutine();
    }
    private void OnDestroy()
    {
        StopDamageCoroutine();
    }
    private void OnEnmyDead()
    {
        EnemyObjectPool.Instance.DeSpawn(this);
    }
    private void OnTakeDamage()
    {
        Balance body = transform.GetComponent<Balance>();
        Balance[] childBalance = transform.GetComponentsInChildren<Balance>();


        damageCoroutine = StartCoroutine(SetBalanceTrigger(body, childBalance));
        
    }
    private IEnumerator SetBalanceTrigger(Balance body, Balance[] childBalance)
    {
        foreach (var item in childBalance)
        {
            item.SetIsTrigger(false);
        }
        body.SetIsTrigger(false);
        yield return new WaitForSeconds(1);

        foreach (var item in childBalance)
        {
            item.SetIsTrigger(true);
        }
        body.SetIsTrigger(true);
    }
    public void StopDamageCoroutine()
    {
        if(damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    #endregion
    public void ResetEnemyPhysics()
    {
        Rigidbody2D[] rb = transform.GetComponentsInChildren<Rigidbody2D>();
        Collider2D[] col = transform.GetComponentsInChildren<Collider2D>();

        foreach (var item in col)
        {
            item.enabled = true;
        }
        foreach (var item in rb)
        {
            item.simulated = true;
            item.linearVelocity = Vector2.zero;
            item.angularVelocity = 0f;
            item.WakeUp();
        }

        // sync
        Physics2D.SyncTransforms();
    }

    public string GetObjectName()
    {
        return StringConst.ENEMY;
    }
}