using System.Collections;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [Header("Character Parts")]
    [SerializeField] BalanceTest left_up_arm;
    [SerializeField] BalanceTest left_down_arm;
    [SerializeField] BalanceTest left_hand;
    [SerializeField] BalanceTest body;
    [SerializeField] BalanceTest body_2;
    [SerializeField] BalanceTest hip;

    [Header("Attack Settings")]
    [SerializeField] float attackDuration = 1f; // thời gian giữ pose tấn công

    private Vector2 attackDir;

    private void FixedUpdate()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body_2.transform.position).normalized;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        // ------------------------------
        // 1. Set góc tấn công dựa theo hướng
        // ------------------------------
        if (attackDir.x < 0) // sang trái
        {
            left_up_arm.SetRotation(-115f);
            left_down_arm.SetRotation(50f);
            left_hand.SetRotation(50f);
            body.SetRotation(25f);
            hip.SetRotation(20f);
            body_2.SetRotation(25f);
        }
        else if (attackDir.x > 0) // sang phải
        {
            left_up_arm.SetRotation(115f);
            left_down_arm.SetRotation(-50f);
            left_hand.SetRotation(-50f);
            body.SetRotation(-25f);
            hip.SetRotation(-20f);
            body_2.SetRotation(-25f);
        }

        // ------------------------------
        // 2. Giữ tư thế tấn công trong attackDuration giây
        // ------------------------------
        yield return new WaitForSeconds(attackDuration);

        // ------------------------------
        // 3. Trở về tư thế idle (0 độ)
        // ------------------------------
        left_up_arm.SetRotation(0f);
        left_down_arm.SetRotation(0f);
        left_hand.SetRotation(0f);
        body.SetRotation(0f);
        hip.SetRotation(0f);
        body_2.SetRotation(0f);
    }
}
