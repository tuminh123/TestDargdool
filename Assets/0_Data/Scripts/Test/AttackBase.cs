using System.Collections;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackForce = 15f;
    [SerializeField] protected float bodyForce = 2f;
    [SerializeField] protected float damping = 0.9f;//giam toc do
    [SerializeField] protected float attackDuration = 0.25f;   // attack time

    protected BodyBalance body;
    protected Vector2 attackDir;
    protected bool isAttacking;

    protected virtual void Awake()
    {
        body = GetComponentInChildren<BodyBalance>();
    }
    private void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;

        Debug.DrawLine(body.transform.position, mouseWorld, Color.blue);
    }

    public void AttackHandle(CharacterCtrl characterController)
    {
        if (isAttacking) return;
        isAttacking = true;

        StartCoroutine(DoAttack(characterController));
    }

    public abstract IEnumerator DoAttack(CharacterCtrl characterController);


}
