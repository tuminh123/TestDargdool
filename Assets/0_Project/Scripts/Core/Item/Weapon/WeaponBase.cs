using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

public enum WeaponType
{
    NONE = 0,
    MELE = 1,
    RANGE = 2,
}

public abstract class WeaponBase : ItemBase,IAttackContext,IObjSendDamage
{
    [SerializeField] protected WeaponType weaponType = WeaponType.MELE;
    [SerializeField] protected FixedJoint2D fixedJoint2D;
    [SerializeField] protected ItemDeSpawn weaponDeSpawn;

    public bool IsAttacking => throw new NotImplementedException();

    public GameObject OnjSend => throw new NotImplementedException();

    public event Action OnAttackStart;
    Coroutine moveCoroutine;
    private void OnEnable()
    {
        //ResetWeapon();
    }

    public void DisableAttack()
    {
        throw new NotImplementedException();
    }

    public void EnableAttack()
    {
        throw new NotImplementedException();
    }
    public void MoveToHand(HandController hand)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine(hand));
    }

    IEnumerator MoveRoutine(HandController hand)
    {
        rb.simulated = false;

        while (Vector2.Distance(transform.position, hand.transform.position) > 0.05f)
        {
            Vector3 targetPos = hand.transform.position - transform.position;

            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * 20
            );

            yield return null;
        }
    }

    public void ResetWeapon()
    {
       
        if (fixedJoint2D == null || weaponDeSpawn == null) return;
        fixedJoint2D.enabled = false;
        weaponDeSpawn.gameObject.SetActive(true);

        fixedJoint2D.connectedBody = null;
    }
    public void Equipping(Rigidbody2D rb)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (fixedJoint2D == null || weaponDeSpawn == null) return;
        fixedJoint2D.enabled = true;
        weaponDeSpawn.gameObject.SetActive(false);

        fixedJoint2D.connectedBody = rb;
    }
}
