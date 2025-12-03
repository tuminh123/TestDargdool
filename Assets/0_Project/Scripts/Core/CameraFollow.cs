using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("Offset so với target")]
    [SerializeField] Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Độ mượt khi theo dõi")]
    [Range(0f, 1f)]
    [SerializeField] float smoothSpeed = 0.15f;

    private Vector3 velocity = Vector3.zero;
 
    void LateUpdate()
    {
        if (SingletonManager.Instance.gameManager.CurrentState != GameState.PLAY) return;
        if (SingletonManager.Instance.gameManager.PlayerInstance == null) return;
        Transform target = SingletonManager.Instance.gameManager.PlayerInstance.transform;

        if (target == null) return;

        // Vị trí camera muốn tới
        Vector3 desiredPos = target.position + offset;

        // SmoothDamp tối ưu hơn Lerp (ít GC, chuyển động tự nhiên)
        Vector3 smoothedPos = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            smoothSpeed
        );

        transform.position = smoothedPos;
    }
}
