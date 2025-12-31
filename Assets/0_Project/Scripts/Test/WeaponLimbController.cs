using UnityEngine;

public class WeaponLimbController : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public HingeJoint2D joint { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<HingeJoint2D>();
        joint.enabled = false;
    }

    public void AttachToHand(Rigidbody2D handRb)
    {
        joint.connectedBody = handRb;
        joint.enabled = true;
    }

    public void Detach()
    {
        joint.enabled = false;
        joint.connectedBody = null;
    }
}
