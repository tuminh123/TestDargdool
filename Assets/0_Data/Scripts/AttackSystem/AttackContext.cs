using UnityEngine;

public class AttackContext
{
    public Vector2 attackDir;
    public BodyBalance body;

    // references (may be null)
    public BalanceAbstract rightArm;
    public BalanceAbstract rightElbow;
    public BalanceAbstract rightHand;
    public BalanceAbstract leftArm;
    public BalanceAbstract leftElbow;
    public BalanceAbstract leftHand;
    public BalanceAbstract rightLeg;
    public BalanceAbstract rightFoot;
    public BalanceAbstract leftLeg;
    public BalanceAbstract leftFoot;
    public BalanceAbstract rightPillow;
    public BalanceAbstract leftPillow;
}