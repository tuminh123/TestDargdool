using UnityEngine;

public class StateMachine
{
    private IState currentState;
    public IState CurrentState=>currentState;

    public void InitState(IState stateInit)
    {
        if (stateInit == currentState) return; 
        currentState = stateInit;
        currentState?.Enter();
    }
    public void ChangeState(IState stateNew)
    {
        if(stateNew == currentState) return;
        currentState?.Exit();
        currentState = stateNew;
        currentState?.Enter();
        Debug.Log($"{currentState}");
    }
    public void UpdateState()
    {
        currentState?.Update();
        Debug.Log($"{currentState}");
    }
    public void UpdatePhysicState()
    {
        currentState?.UpdatePhysic();
    }
    public void ExitState()
    {
        currentState?.Exit();
    }
}
