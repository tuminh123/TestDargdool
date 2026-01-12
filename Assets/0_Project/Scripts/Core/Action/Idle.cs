using UnityEngine;


public class Idle : MonoBehaviour
{
    public void IdleHandle(ActionPostBase action)
    {
        action.SetAction("Idle");
    }
}