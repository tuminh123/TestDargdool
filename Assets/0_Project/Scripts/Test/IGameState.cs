using System.Collections;
using UnityEngine;

public interface IGameState 
{
    public void EnterState();
    public void ExitState();
    public void UpdateState();
}