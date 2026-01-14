using UnityEngine;


public class Idle : MonoBehaviour
{
    public void IdleHandle(IPostAction action)
    {
        action.SetAction(StringConst.IDLE);
    }
}