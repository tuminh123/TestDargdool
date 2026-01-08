using UnityEngine;

public class CharacterT : MonoBehaviour
{
    #region State
    public StateMachine stateMachine { get; private set; }
    public CharacterIdleT idleT { get; private set; }
    public CharacterMoveT moveT { get; private set; }
    public CharacterAttackT attackT { get; private set; }
    #endregion

    protected Vector2 attackDir;

    public MoveT moving { get; private set; }
    public PhysicAttackSystem attack { get; private set; }
    public IdleT idling { get; private set; }
    //get
    public Vector2 AttackDir => attackDir;

    private void Awake()
    {
        idling = GetComponentInChildren<IdleT>();
        moving = GetComponentInChildren<MoveT>();
        attack = GetComponentInChildren<PhysicAttackSystem>();

        stateMachine = new StateMachine();
        idleT = new CharacterIdleT(this, stateMachine);
        moveT = new CharacterMoveT(this, stateMachine);
        attackT = new CharacterAttackT(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.InitState(idleT);
    }
    private void Update()
    {
        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }

    public void SetAttackDirection(Vector2 pos)
    {
        Vector3 tapWorldPos = Camera.main.ScreenToWorldPoint(pos);
        tapWorldPos.z = 0;
        attackDir = (tapWorldPos - transform.position).normalized;
    }


}
