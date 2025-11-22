
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
        
        private void Awake()
        {
                move = GetComponentInChildren<Move>();
                attack = GetComponentInChildren<Attack>();
                idle = GetComponentInChildren<Idle>();
                playerDetect = GetComponentInChildren<PlayerDetect>();

                //state init
                stateMachine = new StateMachine();
                enemyChaseState = new EnemyBasicChaseState(stateMachine, this,body);
                enemyAttackState = new EnemyBasicAttackState(stateMachine, this,body);
                enemyIdleState = new EnemyBasicIdleState(stateMachine, this,body);
        }

        private void Start()
        {
                stateMachine.InitState(enemyChaseState);
        }

        private void FixedUpdate()
        {
                stateMachine.UpdateState();
        }

    public string GetObjectName()
    {
        return StringConst.ENEMY;
    }
}