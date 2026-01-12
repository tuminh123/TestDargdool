using UnityEngine;

public class Jump : MonoBehaviour
{

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] float bodyForce = 2f;
    [SerializeField] float forwardForce = 3f;

    [SerializeField] RagdollController controller;

    public void JumpHandle(float x)
    {
        Vector2 upForce, forward;

        SetDirJump(x, out upForce, out forward);

        if(controller == null) return;
        Balance[] bodys = new Balance[]
        {
            controller?.actionBase?.GetBalance(BalanceType.body_up),
            controller?.actionBase?.GetBalance(BalanceType.body_bottom),
            controller ?.actionBase ?.GetBalance(BalanceType.hip),
        };
        Balance[] legs = new Balance[]
        {
            controller ?.actionBase ?.GetBalance(BalanceType.left_leg),
            controller ?.actionBase ?.GetBalance(BalanceType.left_lower_leg),
            controller ?.actionBase ?.GetBalance(BalanceType.right_leg),
            controller ?.actionBase ?.GetBalance(BalanceType.right_lower_leg)
        };

        if (bodys.Length <= 0 || legs.Length <= 0) return;

        foreach (var item in bodys)
        {
            if(item == null) continue;
            item.Rb.AddForce((upForce + forward) * jumpHeight,ForceMode2D.Impulse);
        }
        foreach (var item in legs)
        {
            if (item == null) continue;
            item.Rb.AddForce((upForce + forward) * jumpHeight*3, ForceMode2D.Impulse);
        }
    }

    private void SetDirJump(float x, out Vector2 upForce, out Vector2 forward)
    {
        Vector2 dirJump;
        if (x > 0)
        {
            dirJump = Vector2.right;
        }
        else if (x < 0)
        {
            dirJump = Vector2.left;
        }
        else
        {
            dirJump = Vector2.up;
        }


        upForce = Vector2.up * bodyForce;
        forward = dirJump * forwardForce;
    }

    public void SetJumpHeight(float jumpHeight)
    {
        this.jumpHeight = jumpHeight;
    }
    public void SetBodyForce(float bodyForce)
    {
        this.bodyForce = bodyForce;
    }
}