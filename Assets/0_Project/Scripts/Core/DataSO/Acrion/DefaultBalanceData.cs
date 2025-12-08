using UnityEngine;

public enum BalanceType
{
    none = 0,
    head = 1,
    body_up = 2,
    right_arm = 3,
    right_forearm = 4,
    right_hand = 5,
    left_arm = 6,
    left_forearm = 7,
    left_hand = 8,
    right_leg = 9,
    right_lower_leg = 10,
    right_foot = 11,
    left_leg = 12,
    left_lower_leg = 13,
    left_foot = 14,
    body_bottom = 15,
    hip = 16,
    weapon_arm_left = 17,
    weapon_arm_right = 18,
}


[CreateAssetMenu(fileName = "Default Balance SO", menuName = "Data SO/Default Balance Data")]
public class DefaultBalanceData :ScriptableObject
{
    [SerializeField] private BalanceType balanceType;
    [SerializeField] private float rot;
    [SerializeField] private float force;

    //get
    public float Rot => rot;
    public float Force => force;
    public BalanceType Type => balanceType;

    public void Change(float rot,float force)
    {
        this.rot = rot;
        this.force = force;
    }
    public void Init(out float rot,out float force)
    {
        rot = this.rot;
        force = this.force;
    }
}
