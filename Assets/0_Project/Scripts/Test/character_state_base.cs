using UnityEngine;

public class character_state_base : IState
{
    protected character_test test;
    protected StateMachine stateMachine;

    protected string stateName;
    protected action_type type;     // <— thêm cái này

    protected BalanceType[] balanceParts = new BalanceType[0]; // <— tránh null

    protected float time;

    public character_state_base(character_test test, StateMachine stateMachine, string stateName, action_type type)
    {
        this.test = test;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
        this.type = type;
    }

    public virtual void Enter()
    {

        test.SetTrigget(true);
        foreach (var part in balanceParts)
        {
            test.Set(type,stateName, part);
        }
    }

    public virtual void Exit()
    {
       

        foreach (var part in balanceParts)
        {
            test.ResetBalance(part);
        }
        
    }

    public virtual void Update()
    {
        time -= Time.deltaTime;
    }

    public virtual void UpdatePhysic()
    {
       
    }
}
