using UnityEngine;

public class EnemyBasicState : IState
{
    protected EnemyAI enemyAI;
    protected StateMachine stateMachine;
    protected Balance bodyBalance;
    protected GameObject player;
    
    protected float distanceToPlayer;
    protected float attackDistance;
    protected Vector2 attackDir;

    public EnemyBasicState(StateMachine stateMachine, EnemyAI enemyAI, GameObject player,float attackDistance,Balance bodyBalance)
    {
        this.stateMachine = stateMachine;
        this.enemyAI = enemyAI;
        this.player = player;
        this.attackDistance = attackDistance;
        this.bodyBalance = bodyBalance;
    }
    
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
      
    }

    public virtual void Update()
    {
        distanceToPlayer = Vector2.Distance(bodyBalance.transform.position, player.transform.position);
        attackDir = (player.transform.position - enemyAI.transform.position).normalized;
    }
}