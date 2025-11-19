using UnityEngine;

public class Idle : MonoBehaviour
{
    [SerializeField]private Balance rightLeg, leftLeg,body,rightDownLeg,leftDownLeg;
    [SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên
    public void IdelHandle()
    {
        leftLeg.SetPropertie(-15, 20);
        leftDownLeg.SetPropertie(-10, 15);
        rightLeg.SetPropertie(15, 20);
        rightDownLeg.SetPropertie(10, 15);
        body.Rb.linearVelocity = new Vector2(body.Rb.linearVelocity.x * damping, body.Rb.linearVelocity.y);
    }
}