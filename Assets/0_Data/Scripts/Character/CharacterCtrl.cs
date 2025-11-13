using System.Runtime.CompilerServices;
using UnityEngine;

public enum state { idle = 0, walk = 1, jump = 2, attack = 3 }

public class CharacterCtrl : MonoBehaviour
{
    [SerializeField] private float timeDelay = 0.1f;
    private BalanceAbstract[] balances;

    private state currentState = state.idle;
    //component
    private Idling idling;
    private Moving moving;
    private AttackTest attacking;
    private AttackTest_2 attackTest_2;
    private Attack_Test_3 attack_3;
    protected Animator anim;
    //get
    public float TimeDelay => timeDelay;
    public BalanceAbstract[] Balances => balances;
    private void Awake()
    {
        idling = GetComponent<Idling>();
        moving = GetComponent<Moving>();
        anim = GetComponent<Animator>();
        attacking = GetComponent<AttackTest>();
        balances = GetComponentsInChildren<BalanceAbstract>();
        attackTest_2 = GetComponent<AttackTest_2>();
        attack_3 = GetComponent<Attack_Test_3>();
    }
    private void Start()
    {
        idling.IdleHandle();
    }
    private void Update()
    {
        AnimationHandle();
    }
    private void FixedUpdate()
    {
        ActionHandle();
    }
    private void AnimationHandle()
    {
        switch (currentState)
        {
            case state.idle:
                anim.Play("idel");
                break;
            case state.walk:
                float x = InputManager.Instance.xInput;
                if (x > 0)
                    anim.Play("walk_right");
                else
                    anim.Play("walk_left");
                    break;
            default:
                break;
        }
    }
    private void ActionHandle()
    {
        switch (currentState)
        {
            case state.idle:
                idling.IdleHandle();
                break;
            case state.walk:
                moving.MoveHandle();
                break;
            case state.jump:

                break;
            case state.attack:
                //attacking.AttackHandle();
                //attackTest_2.AttackHandle();
                attack_3.AttackHandle();
                break;
            default:
                break;
        }
    }

    public void ChangeState(state newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }
}
