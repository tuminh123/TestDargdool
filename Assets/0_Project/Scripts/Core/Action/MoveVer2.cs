using System.Collections;
using UnityEngine;

public class MoveVer2 : MonoBehaviour
{
    [SerializeField] private MoveData data;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float legWait = 0.3f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float bodyForce = 2f;

    private Coroutine moveCoroutine;
    private Vector2 moveDir = Vector2.zero;

    public Vector2 CurrentDirection => moveDir;

    public void SetMoveDirection(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            StopMoveCoroutine();
        }
        else
        {
            moveDir = dir.normalized;
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveRoutine());
        }
    }

    private IEnumerator MoveRoutine()
    {
        bool stepToggle = false;

        while (moveDir != Vector2.zero)
        {
            // Thêm lực vào body
            data.Body.Rb.AddForce(moveDir * bodyForce, ForceMode2D.Force);

            // Animation bước chân
            if (stepToggle) data.Walk_1();
            else data.Walk_2();
            stepToggle = !stepToggle;

            // Di chuyển chân bằng SmoothMotion
            SmoothMotionHelper.SmoothMoveTowards(
                data.RightLeg.Rb,
                data.RightLeg.Rb.position + moveDir * speed * Time.fixedDeltaTime, maxSpeed
            );
            SmoothMotionHelper.SmoothMoveTowards(
                data.LeftLeg.Rb,
                data.LeftLeg.Rb.position + moveDir * speed * Time.fixedDeltaTime, maxSpeed
            );

            yield return new WaitForSeconds(legWait);
        }
    }

    public void StopMoveCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
}
