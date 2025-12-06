

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    public static event Action<Vector2> OnTap;      // Tap event kèm vị trí
    public static event Action<Vector2> OnSwipe;    // Swipe event kèm delta vector

    [Header("Settings")]
    public float tapTime = 0.25f;
    public float tapMaxMovement = 25f;
    public float swipeThreshold = 90f;
    public float deadzone = 6f;

    private Vector2 startTouch;
    private float startTime;
    private bool tapCandidate = false;
    private bool isTouching = false;

    private void Update()
    {
        if (SingletonManager.Instance.gameManager.CurrentState != GameState.PLAY) return;

#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }
    private void OnDisable()
    {
        OnTap = null;
    }
    private void OnDestroy()
    {
        OnTap = null;
    }
    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0)) BeginTouch(Input.mousePosition);
        else if (Input.GetMouseButton(0)) isTouching = true;
        else if (Input.GetMouseButtonUp(0)) EndTouch(Input.mousePosition);
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began) BeginTouch(t.position);
        else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) isTouching = true;
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) EndTouch(t.position);
    }

    private void BeginTouch(Vector2 pos)
    {
        startTouch = pos;
        startTime = Time.time;
        tapCandidate = true;
        isTouching = true;
    }

    private void EndTouch(Vector2 endPos)
    {
        isTouching = false;
        float held = Time.time - startTime;
        float dist = (endPos - startTouch).magnitude;

        if (tapCandidate && held <= tapTime && dist <= tapMaxMovement)
            OnTap?.Invoke(endPos);
        else
            ProcessSwipe(endPos);
    }

    private void ProcessSwipe(Vector2 currentPos)
    {
        Vector2 delta = currentPos - startTouch;
        if (delta.magnitude < deadzone) return;
        if (delta.magnitude < swipeThreshold) return;
        OnSwipe?.Invoke(delta);
    }
}


#region Test
//using UnityEngine;

//public class SwipeManager : MonoBehaviour
//{
//    // ===== Outputs =====
//    public static bool Tap;
//    public static bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
//    public static float MoveDirection = 0f;

//    public static bool IsTouching = false;
//    public static bool IsSwiping = false;

//    public static Vector2 TapPosition;


//    // ===== Settings =====
//    private const float TAP_TIME = 0.25f;
//    private const float TAP_MAX_MOVEMENT = 25f;

//    private const float SWIPE_THRESHOLD = 90f;    // khoảng cách cần để gọi là swipe thật
//    private const float DEADZONE = 6f;            // chống jitter frame đầu


//    // ===== Internal =====
//    private Vector2 startTouch;
//    private float startTime;
//    private bool tapCandidate;


//    private void Update()
//    {
//        // reset mỗi frame
//        Tap = false;
//        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
//        MoveDirection = 0;

//#if UNITY_EDITOR
//        HandleMouse();
//#else
//        HandleTouch();
//#endif

//        ProcessSwipe();
//    }


//    // --------------------
//    // Input Handling
//    // --------------------

//    private void HandleMouse()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            BeginTouch(Input.mousePosition);
//        }
//        else if (Input.GetMouseButton(0))
//        {
//            IsTouching = true;
//        }
//        else if (Input.GetMouseButtonUp(0))
//        {
//            EndTouch(Input.mousePosition);
//        }
//    }

//    private void HandleTouch()
//    {
//        if (Input.touchCount == 0)
//            return;

//        Touch t = Input.GetTouch(0);

//        if (t.phase == TouchPhase.Began)
//            BeginTouch(t.position);
//        else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
//            IsTouching = true;
//        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
//            EndTouch(t.position);
//    }


//    // --------------------
//    // Touch Start & End
//    // --------------------

//    private void BeginTouch(Vector2 pos)
//    {
//        IsTouching = true;

//        tapCandidate = true;
//        startTime = Time.time;
//        startTouch = pos;
//    }


//    private void EndTouch(Vector2 endPos)
//    {
//        IsTouching = false;

//        float held = Time.time - startTime;
//        float dist = (endPos - startTouch).magnitude;

//        // --- TAP CHECK ---
//        if (tapCandidate && held <= TAP_TIME && dist <= TAP_MAX_MOVEMENT)
//        {
//            RegisterTap(endPos);
//        }

//        IsSwiping = false;
//    }


//    private void RegisterTap(Vector2 pos)
//    {
//        Tap = true;
//        TapPosition = pos;

//        // tap không bao giờ dùng hướng cũ
//        MoveDirection = 0;
//        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
//        IsSwiping = false;
//    }


//    // --------------------
//    // Swipe Processing
//    // --------------------

//    private void ProcessSwipe()
//    {
//        if (!IsTouching)
//            return;

//        Vector2 currentPos =
//#if UNITY_EDITOR
//            Input.mousePosition;
//#else
//            Input.touchCount > 0 ? Input.GetTouch(0).position : startTouch;
//#endif

//        Vector2 delta = currentPos - startTouch;

//        // chống jitter (frame đầu tiên)
//        if (delta.magnitude < DEADZONE)
//            return;

//        // nếu đã di chuyển đáng kể → không phải tap nữa
//        if (delta.magnitude > TAP_MAX_MOVEMENT)
//            tapCandidate = false;

//        // chưa đủ độ dài để coi là swipe thật
//        if (delta.magnitude < SWIPE_THRESHOLD)
//            return;

//        // === SWIPE ===
//        IsSwiping = true;

//        float x = delta.x;
//        float y = delta.y;

//        if (Mathf.Abs(x) > Mathf.Abs(y))
//        {
//            if (x < 0) SwipeLeft = true;
//            else SwipeRight = true;
//            MoveDirection = Mathf.Sign(x);
//        }
//        else
//        {
//            if (y < 0) SwipeDown = true;
//            else SwipeUp = true;
//        }
//    }
//}
#endregion





