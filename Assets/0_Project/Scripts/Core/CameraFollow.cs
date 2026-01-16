using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /*
        [Header("Offset so với target")]
        [SerializeField] Vector3 offset = new Vector3(0, 0, -10f);

        [Header("Độ mượt khi theo dõi")]
        [Range(0f, 1f)]
        [SerializeField] float smoothSpeed = 0.15f;
        [SerializeField] Transform target;

        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            target = CharacterCtrl.Instance.transform;

            GameEventBus.OnGameRestart += OnGameRestart;
        }

        private void OnGameRestart()
        {
            target = CharacterCtrl.Instance.transform;
        }

        private void OnDestroy()
        {
            GameEventBus.OnGameRestart -= OnGameRestart;
        }

        void LateUpdate()
        {
            //Transform target = SingletonManager.Instance.gameManager.PlayerInstance.transform;

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
        }*/

    [SerializeField] Vector3 offset = new(0, 0, -10);
    [SerializeField] float smoothTime = 0.15f;
    [SerializeField] float maxFollowSpeed = 20f;
    [SerializeField] Transform target;

    Rigidbody2D targetRb;
    Vector3 velocity;

    void Start()
    {
        //target = CharacterCtrl.Instance.transform;
        targetRb = target.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (targetRb == null) return;

        Vector3 desired = (Vector3)targetRb.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desired,
            ref velocity,
            smoothTime,
            maxFollowSpeed,
            Time.deltaTime
        );
    }
}
