using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Balance body;
    [SerializeField] private Balance leftLeg;
    [SerializeField] private Balance rightLeg;
    
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] float bodyForce = 2f;
    [SerializeField] float forwardForce = 3f;


    public void JumpHandle(float x)
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

         
        Vector2 upForce = Vector2.up * bodyForce;
        Vector2 forward = dirJump * forwardForce;

        body.Rb.AddForce(upForce + forward, ForceMode2D.Impulse);
        leftLeg.Rb.AddForce((upForce + forward) * jumpHeight * 100);
        rightLeg.Rb.AddForce((upForce + forward) * jumpHeight * 100);
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