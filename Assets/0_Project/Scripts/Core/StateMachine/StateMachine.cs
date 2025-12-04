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
        GameState currentGameState = SingletonManager.Instance.gameManager.CurrentState;
        if (currentGameState != GameState.PLAY) return;
        currentState?.Update();
        Debug.Log($"{currentState}");
    }
    public void UpdatePhysicState()
    {
        GameState currentGameState = SingletonManager.Instance.gameManager.CurrentState;
        if (currentGameState != GameState.PLAY) return;
        currentState?.UpdatePhysic();
    }
    public void ExitState()
    {
        currentState?.Exit();
    }
}
