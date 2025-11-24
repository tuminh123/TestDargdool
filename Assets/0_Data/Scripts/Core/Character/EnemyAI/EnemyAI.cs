
using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAI : CharacterParent,IObjectPool
{

        #region State
        private StateMachine stateMachine;
        public EnemyBasicChaseState enemyChaseState { get; private set; }
        public EnemyBasicAttackState enemyAttackState { get; private set; }
        public EnemyBasicIdleState enemyIdleState { get; private set; }

    #endregion

        public PlayerDetect playerDetect { get; private set; }

    protected override void Awake()
        {
                base.Awake();
                playerDetect = GetComponentInChildren<PlayerDetect>();
                //state init
                stateMachine = new StateMachine();
                enemyChaseState = new EnemyBasicChaseState(stateMachine, this,bodyParent.transform);
                enemyAttackState = new EnemyBasicAttackState(stateMachine, this,bodyParent.transform);
                enemyIdleState = new EnemyBasicIdleState(stateMachine, this,bodyParent.transform);
        }

        private void Start()
        {
                stateMachine.InitState(enemyIdleState);
        }

        private void FixedUpdate()
        {
                stateMachine.UpdateState();
        }

    public override void OnDead()
    {
        StartCoroutine(EnemyDead());
    }
    private IEnumerator EnemyDead()
    {
        SetTriggerBalance(false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    protected override Vector2 GetKnockDir()
    {
        return transform.position - CharacterCtrl.Instance.transform.position;
    }

    public string GetObjectName()
    {
        return StringConst.ENEMY;
    }

  
}