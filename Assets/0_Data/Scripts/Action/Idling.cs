using UnityEngine;

public class Idling : ActionBase
{
    [Header("action index")]
    [SerializeField] private float damping = 0.85f;   // giảm tốc khi thả phím
    [SerializeField] private float maxSpeed = 5f;

    private BodyBalance bodyBalance;
    protected override void Awake()
    {
        base.Awake();
        bodyBalance = GetComponentInChildren<BodyBalance>();

    }

    public void IdleHandle()
    {
        Debug.Log("Idle");
        // Thả phím → giảm tốc mượt
        bodyBalance.Rb.linearVelocity = new Vector2(bodyBalance.Rb.linearVelocity.x * damping, bodyBalance.Rb.linearVelocity.y);
        if (InputManager.Instance.xInput != 0)
        {
            characterCtrl.ChangeState(state.walk);
        }
        else if (InputManager.Instance.attackInput)
        {
            characterCtrl.ChangeState(state.attack);
        }
    }

}