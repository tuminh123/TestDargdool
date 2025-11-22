using UnityEngine;

[System.Serializable]
public class IdleData
{
    [SerializeField] private Balance rightLeg, leftLeg, body, rightDownLeg, leftDownLeg;
    [SerializeField] private Balance rightArmUp, rightArmDown, rightHand;
    [SerializeField] private Balance leftArmUp, leftArmDown, leftHand;
    public Balance Body => body;

    public void ResetData()
    {
        rightLeg.ResetData();
        leftLeg.ResetData();
        body.ResetData();
        rightDownLeg.ResetData();
        leftDownLeg.ResetData();

        rightArmUp.ResetData();
        rightArmDown.ResetData();
        rightHand.ResetData();
        leftArmUp.ResetData();
        leftArmDown.ResetData();
        leftHand.ResetData();
    }
}

public class Idle : MonoBehaviour
{
    [SerializeField] private IdleData data;
    [SerializeField] float damping = 0.85f; // giảm nhẹ, tự nhiên
    public void IdelHandle()
    {
        data.ResetData();
        data.Body.Rb.linearVelocity = new Vector2(data.Body.Rb.linearVelocity.x * damping, data.Body.Rb.linearVelocity.y);
    }
}