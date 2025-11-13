using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackSystem : ActionBase
{
    [Header("Profile database (create assets under Assets/0_Data/... )")]
    [SerializeField] private AttackProfileDatabase profileDatabase;

    [Header("References - Right side")]
    [SerializeField] private RightArmBalance rightArmBalance;
    [SerializeField] private RightElbowBalance rightElbowBalance;
    [SerializeField] private RightHandBalance rightHandBalance;
    [SerializeField] private RightLegBalance rightLegBalance;
    [SerializeField] private RightFootBalance rightFootBalance;
    [SerializeField] private RightPillowBalance rightPillowBalance;

    [Header("References - Left side")]
    [SerializeField] private LeftArmBalance leftArmBalance;
    [SerializeField] private LeftElbowBalance leftElbowBalance;
    [SerializeField] private LeftHandBalance leftHandBalance;
    [SerializeField] private LeftLegBalance leftLegBalance;
    [SerializeField] private LeftFootBalance leftFootBalance;
    [SerializeField] private LeftPillowBalance leftPillowBalance;

    private BodyBalance body;
    private Vector2 attackDir;
    private bool isAttacking;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponentInChildren<BodyBalance>();
    }

    private void Update()
    {
        if (Camera.main == null || body == null) return;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;
        Debug.DrawLine(body.transform.position, mouseWorld, Color.blue);
    }

    public void AttackHandle()
    {
        if (isAttacking) return;
        if (profileDatabase == null || profileDatabase.profiles == null || profileDatabase.profiles.Count == 0) return;

        isAttacking = true;

        // choose side by attackDir.x
        AttackProfile.Side desiredSide = attackDir.x >= 0f ? AttackProfile.Side.Right : AttackProfile.Side.Left;

        // pick profiles matching side or Any
        var candidates = profileDatabase.profiles.Where(p => p.side == desiredSide || p.side == AttackProfile.Side.Any).ToArray();
        if (candidates.Length == 0) { isAttacking = false; return; }

        var profile = candidates[Random.Range(0, candidates.Length)];

        // create action and run
        var action = AttackActionFactory.Create(profile);

        var ctx = new AttackContext
        {
            attackDir = attackDir,
            body = body,
            rightArm = rightArmBalance,
            rightElbow = rightElbowBalance,
            rightHand = rightHandBalance,
            leftArm = leftArmBalance,
            leftElbow = leftElbowBalance,
            leftHand = leftHandBalance,
            rightLeg = rightLegBalance,
            rightFoot = rightFootBalance,
            leftLeg = leftLegBalance,
            leftFoot = leftFootBalance,
            rightPillow = rightPillowBalance,
            leftPillow = leftPillowBalance
        };

        StartCoroutine(RunAction(action, ctx, profile));
    }

    private IEnumerator RunAction(IAttackAction action, AttackContext ctx, AttackProfile profile)
    {
        if (action == null || ctx == null || profile == null) { isAttacking = false; yield break; }

        // body impulse
        if (ctx.body != null) { ctx.body.Rb.AddForce(ctx.attackDir * profile.bodyImpulse, ForceMode2D.Impulse); ctx.body.ModifyGravityTemporarily(0.9f, profile.attackDuration); }

        yield return StartCoroutine(action.Execute(ctx, profile));

        // final soft reset
        if (ctx.rightArm != null) ctx.rightArm.LerpToActive(0.12f);
        if (ctx.leftArm != null) ctx.leftArm.LerpToActive(0.12f);
        if (ctx.rightLeg != null) ctx.rightLeg.LerpToActive(0.12f);
        if (ctx.leftLeg != null) ctx.leftLeg.LerpToActive(0.12f);
        if (ctx.body != null) ctx.body.Rb.linearVelocity = Vector2.zero;

        isAttacking = false;
        characterCtrl.ChangeState(state.idle);
    }
}
