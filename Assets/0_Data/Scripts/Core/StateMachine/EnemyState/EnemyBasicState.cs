using UnityEngine;

public class EnemyBasicState : IState
{
    protected EnemyAI enemyAI;
    protected StateMachine stateMachine;
    protected Vector2 attackDir;
    private Transform body;

    public EnemyBasicState(StateMachine stateMachine, EnemyAI enemyAI,Transform body)
    {
        this.stateMachine = stateMachine;
        this.enemyAI = enemyAI;
        this.body = body;
    }
    
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
      
    }

    public virtual void Update()
    {
        //Debug.Log($"{stateMachine.CurrentState}");

        Transform player = CharacterCtrl.Instance.Body.transform;
        attackDir = (player.position - body.position).normalized;
        
    }
}