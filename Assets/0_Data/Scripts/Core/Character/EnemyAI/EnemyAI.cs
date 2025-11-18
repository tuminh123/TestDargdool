
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
        #region Child component
        public MovementController move { get; private set; }
        public Attack attack { get; private set; }
        #endregion

        #region State
        private StateMachine stateMachine;
        public EnemyBasicChaseState enemyChaseState { get; private set; }
        public EnemyBasicAttackState enemyAttackState { get; private set; }
     
        #endregion

        [SerializeField] private GameObject player;
        [SerializeField] private Balance body;
        [SerializeField] private float attackDistance;
        
        private void Awake()
        {
                move = GetComponentInChildren<MovementController>();
                attack = GetComponentInChildren<Attack>();

                //state init
                stateMachine = new StateMachine();
                enemyChaseState = new EnemyBasicChaseState(stateMachine, this, player,attackDistance,body);
                enemyAttackState = new EnemyBasicAttackState(stateMachine, this, player,attackDistance,body);
        }

        private void Start()
        {
                stateMachine.InitState(enemyChaseState);
        }

        private void FixedUpdate()
        {
                stateMachine.UpdateState();
        }
}