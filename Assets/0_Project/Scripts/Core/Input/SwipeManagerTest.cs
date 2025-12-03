using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManagerTest : MonoBehaviour
{
    // ===== Outputs =====
    public static bool Tap;
    public static bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
    public static float MoveDirection = 0f;

    public static bool IsTouching = false;
    public static bool IsSwiping = false;

    public static Vector2 TapPosition;


    // ===== Settings =====
    private const float TAP_TIME = 0.25f;
    private const float TAP_MAX_MOVEMENT = 25f;

    private const float SWIPE_THRESHOLD = 90f;    // khoảng cách cần để gọi là swipe thật
    private const float DEADZONE = 6f;            // chống jitter frame đầu


    // ===== Internal =====
    private Vector2 startTouch;
    private float startTime;
    private bool tapCandidate;


    private void Update()
    {
        if (SingletonManager.Instance.gameManager.CurrentState != GameState.PLAY) return;

        // reset mỗi frame
        Tap = false;
        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
        MoveDirection = 0;

//        // Nếu đang chạm vào UI → bỏ qua hoàn toàn việc xử lý swipe/tap
//#if UNITY_EDITOR
//        if (EventSystem.current.IsPointerOverGameObject())
//            return;
//#else
//        if (Input.touchCount > 0)
//        {
//            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
//                return;
//        }
//#endif

#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif

        ProcessSwipe();
    }


    // --------------------
    // Input Handling
    // --------------------

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginTouch(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            IsTouching = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndTouch(Input.mousePosition);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 0)
            return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
            BeginTouch(t.position);
        else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            IsTouching = true;
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            EndTouch(t.position);
    }


    // --------------------
    // Touch Start & End
    // --------------------

    private void BeginTouch(Vector2 pos)
    {
        IsTouching = true;

        tapCandidate = true;
        startTime = Time.time;
        startTouch = pos;
    }


    private void EndTouch(Vector2 endPos)
    {
        IsTouching = false;

        float held = Time.time - startTime;
        float dist = (endPos - startTouch).magnitude;

        // --- TAP CHECK ---
        if (tapCandidate && held <= TAP_TIME && dist <= TAP_MAX_MOVEMENT)
        {
            RegisterTap(endPos);
        }

        IsSwiping = false;
    }


    private void RegisterTap(Vector2 pos)
    {
        Tap = true;
        TapPosition = pos;

        // tap không bao giờ dùng hướng cũ
        MoveDirection = 0;
        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
        IsSwiping = false;
    }


    // --------------------
    // Swipe Processing
    // --------------------

    private void ProcessSwipe()
    {
        if (!IsTouching)
            return;

        Vector2 currentPos =
#if UNITY_EDITOR
            Input.mousePosition;
#else
            Input.touchCount > 0 ? Input.GetTouch(0).position : startTouch;
#endif

        Vector2 delta = currentPos - startTouch;

        // chống jitter (frame đầu tiên)
        if (delta.magnitude < DEADZONE)
            return;

        // nếu đã di chuyển đáng kể → không phải tap nữa
        if (delta.magnitude > TAP_MAX_MOVEMENT)
            tapCandidate = false;

        // chưa đủ độ dài để coi là swipe thật
        if (delta.magnitude < SWIPE_THRESHOLD)
            return;

        // === SWIPE ===
        IsSwiping = true;

        float x = delta.x;
        float y = delta.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x < 0) SwipeLeft = true;
            else SwipeRight = true;
            MoveDirection = Mathf.Sign(x);
        }
        else
        {
            if (y < 0) SwipeDown = true;
            else SwipeUp = true;
        }
    }
}

#region Test_2
//public class SwipeManager : MonoBehaviour
//{
//    // ====== PUBLIC OUTPUT ======
//    public static bool tap;               // true trong vài frame
//    public static bool swipeLeft, swipeRight, swipeUp, swipeDown;

//    public static float moveDirection = 0;
//    public static bool isSwiping = false;
//    public static bool isTouching = false;

//    public static Vector2 tapPosition;


//    // ====== SETTINGS ======
//    private const float TAP_TIME = 0.2f;          // tăng từ 0.1 → 0.2
//    private const float TAP_MAX_MOVEMENT = 30f;   // tăng từ 20 → 30
//    private const float SWIPE_THRESHOLD = 80f;

//    private const int TAP_BUFFER_FRAMES = 1;      // giữ tap trong 4 frame


//    // ====== INTERNAL STATE ======
//    private static Vector2 startTouch, swipeDelta;
//    private static float tapStartTime;
//    private static bool tapCandidate;

//    private static int tapBufferTimer;


//    private void Update()
//    {
//        // Reset swipe mỗi frame
//        swipeLeft = swipeRight = swipeUp = swipeDown = false;
//        moveDirection = 0;

//        isTouching = false;

//#if UNITY_EDITOR
//        HandleMouse();
//#else
//        HandleTouch();
//#endif        

//        ProcessSwipe();

//        ProcessTapBuffer();
//    }


//    // ---------------------------
//    // INPUT HANDLERS
//    // ---------------------------

//    private void HandleMouse()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            BeginTouch(Input.mousePosition);
//        }
//        else if (Input.GetMouseButton(0))
//        {
//            isTouching = true;
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
//            isTouching = true;
//        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
//            EndTouch(t.position);
//    }


//    // ---------------------------
//    // TOUCH START / END
//    // ---------------------------

//    private void BeginTouch(Vector2 pos)
//    {
//        isTouching = true;

//        tapCandidate = true;          // có thể là tap
//        tapStartTime = Time.time;     // bắt đầu tính thời gian
//        startTouch = pos;             // vị trí bắt đầu
//    }


//    private void EndTouch(Vector2 endPos)
//    {
//        isTouching = false;

//        if (tapCandidate)
//        {
//            float timeHeld = Time.time - tapStartTime;
//            float dist = (endPos - startTouch).magnitude;

//            // Điều kiện TAP
//            if (timeHeld <= TAP_TIME && dist < TAP_MAX_MOVEMENT)
//            {
//                RegisterTap(startTouch);
//            }
//        }

//        tapCandidate = false;
//        isSwiping = false;
//        moveDirection = 0;
//    }


//    private void RegisterTap(Vector2 pos)
//    {
//        tap = true;
//        tapPosition = pos;

//        tapBufferTimer = TAP_BUFFER_FRAMES; // giữ tap trong vài frame

//        moveDirection = 0;
//        swipeLeft = swipeRight = swipeUp = swipeDown = false;
//        isSwiping = false;

//    }


//    // ---------------------------
//    // SWIPE PROCESSOR
//    // ---------------------------

//    private void ProcessSwipe()
//    {
//        if (!isTouching)
//        {
//            isSwiping = false;
//            return;
//        }

//#if UNITY_EDITOR
//        swipeDelta = (Vector2)Input.mousePosition - startTouch;
//#else
//        if (Input.touchCount > 0)
//            swipeDelta = Input.GetTouch(0).position - startTouch;
//#endif

//        // Nếu di chuyển nhiều → không còn là tap
//        if (swipeDelta.magnitude > TAP_MAX_MOVEMENT)
//            tapCandidate = false;

//        if (swipeDelta.magnitude < SWIPE_THRESHOLD) return;

//        isSwiping = true;
//        tapCandidate = false; // chắc chắn không phải tap

//        float x = swipeDelta.x;
//        float y = swipeDelta.y;

//        if (Mathf.Abs(x) > Mathf.Abs(y))
//        {
//            if (x < 0) swipeLeft = true;
//            else swipeRight = true;

//            moveDirection = Mathf.Sign(x);
//        }
//        else
//        {
//            if (y < 0) swipeDown = true;
//            else swipeUp = true;
//        }
//    }


//    // ---------------------------
//    // TAP BUFFER HOLDER
//    // ---------------------------

//    private void ProcessTapBuffer()
//    {
//        if (tapBufferTimer > 0)
//        {
//            tap = true;       // giữ tap liên tục
//            tapBufferTimer--;
//        }
//        else
//        {
//            tap = false;
//        }
//    }
//}
#endregion

#region Test_1
//    // ====== SETTINGS ======
//    private const float TAP_TIME = 0.2f;          // tăng từ 0.1 → 0.2
//    private const float TAP_MAX_MOVEMENT = 30f;   // tăng từ 20 → 30
//    private const float SWIPE_THRESHOLD = 80f;

//    private const int TAP_BUFFER_FRAMES = 4;      // giữ tap trong 4 frame

//    public static bool tap;               // tap thật sự
//    public static bool swipeLeft, swipeRight, swipeUp, swipeDown;

//    public static float moveDirection = 0; // -1 trái, +1 phải, 0 đứng
//    public static bool isSwiping = false;  // đang vuốt
//    public static bool isTouching = false; // đang chạm màn hình

//    // ====== INTERNAL STATE ======
//    private static float tapStartTime;
//    private static bool tapCandidate;

//    //position tap
//    private static Vector2 startTouch, swipeDelta;
//    public static Vector2 tapPosition;

//    private void Update()
//    {
//        // Reset input mỗi frame
//        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
//        moveDirection = 0;

//        isTouching = false;

//#if UNITY_EDITOR
//        HandleMouse();
//#else
//        HandleTouch();
//#endif        

//        ProcessSwipe();
//    }

//    private void HandleMouse()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            isTouching = true;
//            tapCandidate = true;
//            tapStartTime = Time.time;
//            startTouch = Input.mousePosition;

//        }
//        else if (Input.GetMouseButton(0))
//        {
//            isTouching = true;
//        }
//        else if (Input.GetMouseButtonUp(0))
//        {
//            EndTouch(Input.mousePosition);
//        }
//    }

//    private void HandleTouch()
//    {
//        if (Input.touches.Length > 0)
//        {
//            Touch t = Input.touches[0];

//            if (t.phase == TouchPhase.Began)
//            {
//                isTouching = true;
//                tapCandidate = true;
//                tapStartTime = Time.time;
//                startTouch = t.position;
//            }
//            else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
//            {
//                isTouching = true;
//            }
//            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
//            {
//                EndTouch(t.position);
//            }
//        }
//    }

//    private void EndTouch(Vector2 endPos)
//    {
//        isTouching = false;

//        // Nếu vẫn là tapCandidate và không vuốt
//        if (tapCandidate)
//        {
//            float timeHeld = Time.time - tapStartTime;
//            float dist = (endPos - startTouch).magnitude;

//            if (timeHeld <= TAP_TIME && dist < TAP_MAX_MOVEMENT)
//            {
//                tap = true; // TAP CHÍNH THỨC
//                tapPosition = startTouch;
//            }
//        }

//        tapCandidate = false;
//        isSwiping = false;
//        moveDirection = 0;
//    }

//    private void ProcessSwipe()
//    {
//        if (!isTouching)
//        {
//            isSwiping = false;
//            return;
//        }

//        swipeDelta = Vector2.zero;

//#if UNITY_EDITOR
//        swipeDelta = (Vector2)Input.mousePosition - startTouch;
//#else
//        if (Input.touches.Length > 0)
//            swipeDelta = Input.touches[0].position - startTouch;
//#endif

//        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
//        {
//            tapCandidate = false; // không cho tap nữa
//            tap = false;

//            float x = swipeDelta.x;
//            float y = swipeDelta.y;

//            isSwiping = true;

//            if (Mathf.Abs(x) > Mathf.Abs(y))
//            {
//                if (x < 0) { swipeLeft = true; moveDirection = -1; }
//                else { swipeRight = true; moveDirection = 1; }
//            }
//            else
//            {
//                if (y < 0) swipeDown = true;
//                else swipeUp = true;
//            }
//        }

//        // nếu đang vuốt liên tục → giữ hướng moveDirection
//        if (isSwiping)
//        {
//            moveDirection = Mathf.Sign(swipeDelta.x);
//        }
//    }
#endregion