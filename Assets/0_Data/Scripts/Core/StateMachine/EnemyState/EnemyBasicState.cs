using System;
using UnityEngine;

public class EnemyBasicState : IState
{
    protected EnemyAI enemyAI;
    protected StateMachine stateMachine;
    protected Transform body;

    protected float distance;
    protected Vector2 dir;

    protected float timeAttack;

    public EnemyBasicState(StateMachine stateMachine, EnemyAI enemyAI, Transform body)
    {
        this.stateMachine = stateMachine;
        this.enemyAI = enemyAI;
        this.body = body;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update()
    {
        timeAttack -= Time.deltaTime;

        if (enemyAI.healthBase.IsDead || enemyAI.IsStunned) return;

        HandleProperties();
    }

    private void HandleProperties()
    {
        Transform player = CharacterCtrl.Instance.transform;

        dir = (player.position - body.position).normalized;
        distance = Vector2.Distance(body.position, player.position);
    }

    public virtual void UpdatePhysic() 
    {
    }
}