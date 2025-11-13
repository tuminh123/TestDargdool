using UnityEngine;

public class AttackContext
{
    public Vector2 attackDir;
    public BodyBalance body;

    // references (may be null)
    public RightArmBalance rightArm;
    public RightElbowBalance rightElbow;
    public RightHandBalance rightHand;
    public LeftArmBalance leftArm;
    public LeftElbowBalance leftElbow;
    public LeftHandBalance leftHand;
    public RightLegBalance rightLeg;
    public RightFootBalance rightFoot;
    public LeftLegBalance leftLeg;
    public LeftFootBalance leftFoot;
    public RightPillowBalance rightPillow;
    public LeftPillowBalance leftPillow;
}