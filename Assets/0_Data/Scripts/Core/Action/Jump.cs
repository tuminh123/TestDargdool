using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Balance body;
    [SerializeField] private Balance leftLeg;
    [SerializeField] private Balance rightLeg;
    
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] float bodyForce = 2f;

    public void JumpHandle()
    {
        body.Rb.AddForce(Vector2.up * bodyForce, ForceMode2D.Impulse);
        leftLeg.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
        rightLeg.Rb.AddForce(Vector2.up * (jumpHeight * 1000));
    }
    
}