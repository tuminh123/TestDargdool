using UnityEngine;

public class Idle : MonoBehaviour
{
    [SerializeField]private Balance rightLeg, leftLeg,body;
    [SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên
    public void IdelHandle()
    {
        //leftLeg.SetPropertie(0, 20);
        //rightLeg.SetPropertie(0, 20);
        body.Rb.linearVelocity = new Vector2(body.Rb.linearVelocity.x * damping, body.Rb.linearVelocity.y);
    }
}